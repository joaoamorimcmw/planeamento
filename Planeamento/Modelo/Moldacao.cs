using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class Moldacao
    {
        #region Variáveis e Construtor

        private DataTable MoldesGF;
        private DataTable MoldesIMF;
        private DataTable MoldesManual;
        private DataTable PlanoGF;
        private DataTable PlanoIMF;
        private DataTable PlanoManual;

        private int accCaixas = 0;
        private int dia = 1;
        private int semana = 1;
        private int turno = 1;

        /**** Parametros ****/

        private int CapacidadeGF;
        private int CapacidadeIMF;
        private int CapacidadeManual;

        private int nTurnosGF;
        private int nTurnosIMF;
        private int nTurnosManual;
        
        /*********************/

        public Moldacao()
        {
            MoldesGF = new DataTable("Moldes GF"); //Contem informação sobre os moldes a planear
            MoldesGF.Columns.Add(new DataColumn("Id", typeof(int))); //Linha da tabela Produtos associada
            MoldesGF.Columns.Add(new DataColumn("Qtd", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("NoMoldes", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("SemanaMacharia", typeof(int)));
            MoldesGF.Columns.Add(new DataColumn("DiaMacharia", typeof(int)));

            MoldesIMF = new DataTable("Moldes IMF");
            MoldesIMF.Columns.Add(new DataColumn("Id", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("Qtd", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("NoMoldes", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("SemanaMacharia", typeof(int)));
            MoldesIMF.Columns.Add(new DataColumn("DiaMacharia", typeof(int)));

            MoldesManual = new DataTable("Moldes Manual");
            MoldesManual.Columns.Add(new DataColumn("Id", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("Qtd", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("NoMoldes", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("Caixas", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("SemanaMacharia", typeof(int)));
            MoldesManual.Columns.Add(new DataColumn("DiaMacharia", typeof(int)));

            PlanoGF = new DataTable("Plano GF"); //Contem informação sobre os machos a planear
            PlanoGF.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Local", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            PlanoGF.Columns.Add(new DataColumn("CaixasAcc", typeof(int)));

            PlanoIMF = new DataTable("Plano IMF");
            PlanoIMF.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Local", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("Caixas", typeof(int)));
            PlanoIMF.Columns.Add(new DataColumn("CaixasAcc", typeof(int)));

            PlanoManual = new DataTable("Plano Manual");
            PlanoManual.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Local", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("Caixas", typeof(int)));
            PlanoManual.Columns.Add(new DataColumn("CaixasAcc", typeof(int)));
        }

        #endregion

        #region Pré-Execução

        public void LimpaBDMoldacao()
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

        public void GetParametros()
        {
            CapacidadeGF = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MoldacaoGF));
            CapacidadeIMF = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MoldacaoIMF));
            CapacidadeManual = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MoldacaoManual));
            int turnos = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Turnos));
            nTurnosGF = turnos;
            nTurnosIMF = turnos;
            nTurnosManual = turnos;
        }

        #endregion

        #region Execução

        public void Executa(int Local)
        {
            LeituraBD(Local);
            Planeamento(Local);
            EscreveBD(Local);
        }

        private void LeituraBD(int Local)
        {
            String query = "select Id,QtdPendente,NoMoldes,SemanaMacharia,DiaMacharia from dbo.PlanCMW$Produtos where Include = 1 and Local = " + Local + " order by Id asc";

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

                int qtd = Convert.ToInt32(reader["QtdPendente"]);
                int nMoldes = Convert.ToInt32(reader["NoMoldes"]);

                row["Id"] = Convert.ToInt32(reader["Id"]);
                row["Qtd"] = qtd;
                row["NoMoldes"] = nMoldes;
                row["Caixas"] = (int)Math.Ceiling((decimal)qtd / (decimal)nMoldes);
                row["SemanaMacharia"] = Convert.ToInt32(reader["SemanaMacharia"]);
                row["DiaMacharia"] = Convert.ToInt32(reader["DiaMacharia"]);

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

        private void Planeamento(int Local)
        {
            ResetGlobais();
            if (Local == 1)
                PlaneamentoLocal(1, CapacidadeGF, nTurnosGF,0,MoldesGF,new LinkedList<DataRow>());
            else if (Local == 2)
                PlaneamentoLocal(2, CapacidadeIMF, nTurnosIMF, 0, MoldesIMF, new LinkedList<DataRow>());
            else
                PlaneamentoLocal(3, CapacidadeManual, nTurnosManual, 0, MoldesManual, new LinkedList<DataRow>());
            
        }

        private void PlaneamentoLocal(int Local, int Capacidade, int nTurnos, int Index, DataTable Table, LinkedList<DataRow> RowList)
        {
            DataRow row;

            if (RowList.Count > 0 && RespeitaPrecedencia(RowList.First.Value)){
                row = RowList.First.Value;
                RowList.RemoveFirst();
            }

            else 
            {
                while (Index < Table.Rows.Count && !RespeitaPrecedencia(Table.Rows[Index]))
                {
                    RowList.AddLast(Table.Rows[Index]);
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

            int caixas = Convert.ToInt32(row["Caixas"]);
            int id = Convert.ToInt32(row["Id"]);

            if (accCaixas == Capacidade) //se o acumulado for exactamente igual à capacidade
            {
                MudaTurno(nTurnos);
                RowList.AddFirst(row);
                PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
            }

            else if (accCaixas + caixas > Capacidade) //se tiver de separar
            {
                int qtd = Convert.ToInt32(row["Qtd"]);
                int nMoldes = Convert.ToInt32(row["NoMoldes"]);
                int caixasNovo = Capacidade - accCaixas;
                accCaixas = Capacidade;
                InsereLinhaPlaneamento(id, Local, caixasNovo);

                row["Caixas"] = caixas - caixasNovo;
                MudaTurno(nTurnos);
                RowList.AddFirst(row);
                PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
            }

            else //se couber tudo no dia
            {
                accCaixas += caixas;
                InsereLinhaPlaneamento(id, Local, caixas);
                PlaneamentoLocal(Local, Capacidade, nTurnos, Index, Table, RowList);
            }

        }

        private void ResetGlobais()
        {
            accCaixas = 0;
            dia = 1;
            semana = 1;
            turno = 1;
        }

        private void MudaTurno(int nTurnos)
        {
            Util.ProximoTurno(ref turno, ref dia, ref semana, nTurnos);
            accCaixas = 0;
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

        private void InsereLinhaPlaneamento(int Id, int Local, int Caixas)
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
            row["CaixasAcc"] = accCaixas;

            if (Local == 1)
                PlanoGF.Rows.Add(row);
            else if (Local == 2)
                PlanoIMF.Rows.Add(row);
            else
                PlanoManual.Rows.Add(row);
        }

        #endregion

        #region Pós-Execução

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
                SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[PlanCMW$Moldacao] VALUES(@Id,@Local,@Semana,@Dia,@Turno,@Caixas,@CaixasAcc)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Local", row["Local"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.Parameters.AddWithValue("@Caixas", row["Caixas"]);
                cmd.Parameters.AddWithValue("@CaixasAcc", row["CaixasAcc"]);
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

        public void LimpaTabelas()
        {
            MoldesGF.Clear();
            MoldesIMF.Clear();
            MoldesManual.Clear();
            PlanoGF.Clear();
            PlanoIMF.Clear();
            PlanoManual.Clear();
        }

        #endregion

    }
}
