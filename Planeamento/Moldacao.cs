using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    class Moldacao
    {
        private DataTable MoldesGF;
        private DataTable MoldesIMF;
        private DataTable MoldesManual;
        private DataTable PlanoGF;
        private DataTable PlanoIMF;
        private DataTable PlanoManual;

        private int capacidadeGF;
        private int capacidadeIMF;
        private int capacidadeManual;

        private int accCaixas = 0;
        private int accPeso = 0;
        private int dia = 1;
        private int semana = 1;
        private int turno = 1;

        private static int nTurnosGF = 1;
        private static int nTurnosIMF = 1;
        private static int nTurnosManual = 1;

        public Moldacao()
        {
            MoldesGF = new DataTable("Moldes GF"); //Contem informação sobre os moldes a planear
            MoldesGF.Columns.Add(new DataColumn("Id", typeof(int))); //Linha da tabela Produtos associada
            MoldesGF.Columns.Add(new DataColumn("Qtd", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("Peso", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("NoMoldes", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("PesoTotal", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("SemanaMacharia", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("DiaMacharia", typeof(int)));

            MoldesIMF = new DataTable("Moldes IMF");
            MoldesIMF.Columns.Add(new DataColumn("Id", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("Qtd", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("Peso", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("NoMoldes", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("PesoTotal", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("SemanaMacharia", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("DiaMacharia", typeof(int)));

            MoldesManual = new DataTable("Moldes Manual");
            MoldesManual.Columns.Add(new DataColumn("Id", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("Qtd", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("Peso", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("NoMoldes", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("Caixas", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("PesoTotal", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("SemanaMacharia", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("DiaMacharia", typeof(int)));

            PlanoGF = new DataTable("Plano GF"); //Contem informação sobre os machos a planear
            PlanoGF.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Local", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("PesoTotal", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("CaixasAcc", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("PesoAcc", typeof(int)));

            PlanoIMF = new DataTable("Plano IMF");
            PlanoIMF.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Local", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("PesoTotal", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("CaixasAcc", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("PesoAcc", typeof(int)));

            PlanoManual = new DataTable("Plano Manual");
            PlanoManual.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Local", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Caixas", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("PesoTotal", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("CaixasAcc", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("PesoAcc", typeof(int)));
        }

        public void Executa()
        {
            LimpaBDMoldacao();

            LeituraBD(1);
            LeituraBD(2);
            LeituraBD(3);

            GetCapacidades();
            Planeamento();

            EscreveBD(1);
            EscreveBD(2);
            EscreveBD(3);

            LimpaTabelas();
        }

        private void LimpaBDMoldacao() 
        {
            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Moldacao]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Moldacao");
            connection.Close();
        }

        public void LeituraBD(int Local)
        {
            List<int> produtosSemMacho = new List<int>();
            String query = "select Id,QtdPendente,PesoPeca,NoMoldes,SemanaMacharia,DiaMacharia from dbo.PlanCMW$Produtos where Local = " + Local + "order by Id asc";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DataRow row;
                if (Local == 1)
                    row = MoldesGF.NewRow();
                else if (Local == 2)
                    row = MoldesIMF.NewRow();
                else 
                    row = MoldesManual.NewRow();

                decimal qtd = reader.GetInt32(1);
                int peso = (int)reader.GetDecimal(2);
                int nMoldes = reader.GetInt32(3);

                row["Id"] = reader.GetInt32(0);
                row["Qtd"] = qtd;
                row["Peso"] = peso;
                row["NoMoldes"] = nMoldes;
                row["Caixas"] = (int)Math.Ceiling((decimal)qtd / (decimal)nMoldes);
                row["PesoTotal"] = (int)qtd * peso;
                row["SemanaMacharia"] = reader.GetInt32(4);
                row["DiaMacharia"] = reader.GetInt32(5);

                if (Local == 1)
                    MoldesGF.Rows.Add(row);
                else if (Local == 2)
                    MoldesIMF.Rows.Add(row);
                else
                    MoldesManual.Rows.Add(row);
            }

            reader.Close();
            connection.Close();
        }

        private void GetCapacidades()
        {
            SqlConnection connection = Util.AbreBD();

            SqlCommand cmd;

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Mold GF'", connection);
            cmd.CommandType = CommandType.Text;
            capacidadeGF = (int)cmd.ExecuteScalar(); ///420

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Mold IMF'", connection);
            cmd.CommandType = CommandType.Text;
            capacidadeIMF = (int)cmd.ExecuteScalar(); //95

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Mold Manual'", connection);
            cmd.CommandType = CommandType.Text;
            capacidadeManual = (int)cmd.ExecuteScalar(); //12

            connection.Close();
        }


        private void Planeamento()
        {
            ResetGlobais();
            PlaneamentoLocal(1,capacidadeGF, nTurnosGF,0,MoldesGF,new LinkedList<DataRow>());
            
        }

        private void PlaneamentoLocal(int Local, int Capacidade, int nTurnos, int Index, DataTable Table, LinkedList<DataRow> RowList)
        {
            Console.WriteLine("Index: " + Index + ", LinkedList : " + RowList.Count);

            DataRow row;

            if (RowList.Count > 0 && RespeitaPrecedencia(RowList.First.Value))
                row = Queue.Dequeue();

            else 
            {
                while (Index < Table.Rows.Count && !RespeitaPrecedencia(Table.Rows[Index]))
                {
                    Queue.Enqueue(Table.Rows[Index]);
                    Index++;
                }

                if (Index < Table.Rows.Count)
                {
                    row = Table.Rows[Index];
                    Index++;
                }
                else
                {
                    if (RowList.Count > 0)
                    {
                        MudaTurno(nTurnos);
                        PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
                    }
                    return;
                }
            }

            if (accCaixas == Capacidade) //se o acumulado for exactamente igual à capacidade
            {
                MudaTurno(nTurnos);
                PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
            }

            int caixas = Convert.ToInt32(row["Caixas"]);
            int pesoTotal = Convert.ToInt32(row["PesoTotal"]);
            int id = Convert.ToInt32(row["Id"]);

            if (accCaixas + caixas > Capacidade)
            {
                int qtd = Convert.ToInt32(row["Qtd"]);
                int nMoldes = Convert.ToInt32(row["NoMoldes"]);
                int caixasNovo = Capacidade - accCaixas;
                int pesoNovo = caixasNovo * Convert.ToInt32(row["NoMoldes"]) * Convert.ToInt32(row["Peso"]);
                InsereLinhaPlaneamento(id, Local, caixasNovo, pesoNovo);

                row["Caixas"] = caixas - caixasNovo;
                row["PesoTotal"] = pesoTotal - pesoNovo;
                MudaTurno(nTurnos);
                //Coloca na frente da queue
                PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
            }

            else
            {
                InsereLinhaPlaneamento(id, Local, caixas, pesoTotal);
                PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
            }

        }

        private void ResetGlobais()
        {
            accCaixas = 0;
            accPeso = 0;
            dia = 1;
            semana = 1;
            turno = 1;
        }

        private void MudaTurno(int nTurnos)
        {
            Util.ProximoTurno(ref turno, ref dia, ref semana, nTurnos);
            accCaixas = 0;
            accPeso = 0;
        }


        private bool RespeitaPrecedencia(DataRow row)
        {
            int semanaMacharia = Convert.ToInt32(row["SemanaMacharia"]);
            int diaMacharia = Convert.ToInt32(row["DiaMacharia"]);

            if (semana < semanaMacharia)
                return false;

            if (semana == semanaMacharia)
            {
                if (dia < diaMacharia)
                    return false;
                if (dia == diaMacharia)
                    return turno > 1;
            }

            return true;
        }

        private void InsereLinhaPlaneamento(int Id, int Local, int Caixas, int PesoTotal)
        {
            DataRow row;

            if (Local == 1)
                row = PlanoGF.NewRow();
            else if (Local == 2)
                row = PlanoIMF.NewRow();
            else
                row = PlanoManual.NewRow();

            row["Id"] = Id;
            row["Local"] = Local;
            row["Semana"] = semana;
            row["Dia"] = dia;
            row["Turno"] = turno;
            row["Caixas"] = Caixas;
            row["PesoTotal"] = PesoTotal;
            row["CaixasAcc"] = accCaixas;
            row["PesoAcc"] = accPeso;

            if (Local == 1)
                PlanoGF.Rows.Add(row);
            else if (Local == 2)
                PlanoIMF.Rows.Add(row);
            else
                PlanoManual.Rows.Add(row);
        }

        //Escreve as linhas de plano de Macharia na tabela Macharia, e actualiza a tabela Produtos com o Dia e Semana da Macharia
        private void EscreveBD(int Local)
        {
            DataTable Plano;
            if (Local == 1)
                Plano = PlanoGF;
            else if (Local == 2)
                Plano = PlanoIMF;
            else
                Plano = PlanoManual;

            SqlConnection connection = Util.AbreBD();
            int linhas = 0;

            foreach (DataRow row in Plano.Rows)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[PlanCMW$Moldacao] VALUES(@Id,@Local,@Semana,@Dia,@Turno,@Caixas,@PesoTotal,@CaixasAcc,@PesoAcc)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Local", row["Local"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.Parameters.AddWithValue("@Caixas", row["Caixas"]);
                cmd.Parameters.AddWithValue("@PesoTotal", row["PesoTotal"]);
                cmd.Parameters.AddWithValue("@CaixasAcc", row["CaixasAcc"]);
                cmd.Parameters.AddWithValue("@PesoAcc", row["PesoAcc"]);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE Planeamento.dbo.[PlanCMW$Produtos] SET DiaMoldacao = @Dia, SemanaMoldacao = @Semana, TurnoMoldacao = @Turno WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.ExecuteNonQuery();
                linhas++;
            }

            String nome;

            if (Local == 1)
                nome = "GF";
            else if (Local == 2)
                nome = "IMF";
            else
                nome = "Manual";

            Console.WriteLine(nome +": " + linhas + " linhas inseridas na tabela Moldacao");
        }

        private void LimpaTabelas()
        {
            MoldesGF.Clear();
            MoldesIMF.Clear();
            MoldesManual.Clear();
            PlanoGF.Clear();
            PlanoIMF.Clear();
            PlanoManual.Clear();
        }

    }
}
