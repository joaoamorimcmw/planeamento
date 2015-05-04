using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class Rebarbagem
    {
        private static string NoRout = "NoRout";

        private DataTable Produtos;
        private DataTable Plano;

        private int semana = 1;
        private int dia = 1;
        private int turno = 1;
        private decimal tempoUsado = 0;
        private Dictionary<String, Decimal> CapacidadeUsada;

        private int nTurnos;
        private int FuncionariosTurno;
        private decimal Horario;

        private Dictionary<String, Int32> nPostos;

        private Dictionary<String, Int32> NPostos
        {
            get { return nPostos; }
            set { nPostos = value; }
        }

        private Dictionary<String, String> postosAssociados;

        public Dictionary<String, String> PostosAssociados
        {
            get { return postosAssociados; }
            set { postosAssociados = value; }
        }

        public Rebarbagem()
        {
            Produtos = new DataTable("Produtos");
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Qtd", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Posto", typeof(string)));
            Produtos.Columns.Add(new DataColumn("Tempo", typeof(int)));
            Produtos.Columns.Add(new DataColumn("SemanaFusao", typeof(int)));
            Produtos.Columns.Add(new DataColumn("DiaFusao", typeof(int)));
            Produtos.Columns.Add(new DataColumn("TurnoFusao", typeof(int)));

            Plano = new DataTable("Plano");
            Plano.Columns.Add(new DataColumn("Id", typeof(int)));
            Plano.Columns.Add(new DataColumn("Semana",typeof(int)));
            Plano.Columns.Add(new DataColumn("Dia",typeof(int)));
            Plano.Columns.Add(new DataColumn("Turno",typeof(int)));
            Plano.Columns.Add(new DataColumn("Posto", typeof(string)));
            Plano.Columns.Add(new DataColumn("QtdPecas", typeof(int)));
            Plano.Columns.Add(new DataColumn("Tempo", typeof(decimal)));

            nPostos = new Dictionary<string, int>();
            postosAssociados = new Dictionary<string, string>();
            CapacidadeUsada = new Dictionary<string, decimal>();

            String query = "select CodCentroMaquina,CodPosto,QtdPosto from PlanCMW$PostosRebarbagem";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string centro = reader["CodCentroMaquina"].ToString();
                string posto = reader["CodPosto"].ToString();
                int qtd = Convert.ToInt32(reader["QtdPosto"]);

                postosAssociados.Add(centro, posto);
                if (!nPostos.Keys.Contains(posto))
                    nPostos.Add(posto, qtd);
                if (!CapacidadeUsada.Keys.Contains(posto))
                    CapacidadeUsada.Add(posto, 0);
            }

            reader.Close();
            connection.Close();
        }

        public void GetParametros()
        {
            FuncionariosTurno = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.FuncionariosTurnoRebarbagem));
            nTurnos = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Turnos));
            Horario = Convert.ToDecimal(ParametrosBD.GetParametro(ParametrosBD.Horario));
        }

        public void LimpaBDRebarbagem()
        {
            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Rebarbagem]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Rebarbagem");

            connection.Close();
        }

        public void LeituraBD()
        {
            string query = "select Prod.Id as Id, Prod.QtdVazadas as Qtd,isnull(Rout.[No_],@NoRout) as Posto,Rout.[Run Time] as Tempo, SemanaFusao, DiaFusao, TurnoFusao from " +
            "(select * from PlanCMW$Produtos where QtdVazadas > 0) Prod " +
            "left join " +
            "(select * from Navision.dbo.[CMW$Routing Line] where No_ in (select CodCentroMaquina from PlanCMW$PostosRebarbagem)) Rout " +
            "on Prod.NoProd = Rout.[Routing No_] " +
            "where Rout.[Run Time] > 0" +
            "order by Id";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NoRout", NoRout);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DataRow row = Produtos.NewRow();

                string Posto = reader["Posto"].ToString();

                if (Posto == NoRout)
                    continue;

                row["Id"] = Convert.ToInt32(reader["Id"]);
                row["Qtd"] = Convert.ToInt32(reader["Qtd"]);
                row["Posto"] = Posto;
                row["Tempo"] = Convert.ToDecimal(reader["Tempo"]);
                row["SemanaFusao"] = Convert.ToInt32(reader["SemanaFusao"]);
                row["DiaFusao"] = Convert.ToInt32(reader["DiaFusao"]);
                row["TurnoFusao"] = Convert.ToInt32(reader["TurnoFusao"]);

                Produtos.Rows.Add(row);
            }

            reader.Close();
            connection.Close();
        }

        public void Executa()
        {
            List<DataRow> list = new List<DataRow>();
            foreach (DataRow row in Produtos.Rows)
                list.Add(row);

            PlaneamentoTurno(list);
        }

        private void PlaneamentoTurno(List<DataRow> list)
        {
            if (list.Count == 0)
                return;

            int lastId = -1;
            bool lastAdded = true;

            List<DataRow> toRemove = new List<DataRow>();
            
            foreach (DataRow row in list)
            {
                int id = Convert.ToInt32(row["Id"]);

                if ((id == lastId && !lastAdded) || !RespeitaPrecedencia(row))
                    lastAdded = false;
                else
                {
                    string maquina = row["Posto"].ToString();
                    string posto = postosAssociados[maquina];

                    decimal tempo = Convert.ToDecimal(row["Tempo"]);
                    int qtd = Convert.ToInt32(row["Qtd"]);
                    decimal tempoTotal = tempo * qtd;

                    decimal tempoRestante = Horario * FuncionariosTurno - tempoUsado;
                    decimal tempoPostoRestante = nPostos[posto] * Horario - CapacidadeUsada[posto];
                    decimal tempoLivre = Math.Min(tempoRestante, tempoPostoRestante);

                    if (tempoTotal <= tempoLivre)
                    {
                        InsereLinhaPlaneamento(id, posto, qtd, tempoTotal);
                        toRemove.Add(row);
                        tempoUsado += tempoTotal;
                        CapacidadeUsada[posto] += tempoTotal;
                        lastAdded = true;
                    }

                    else
                    {
                        int qtdLivre = (int) Math.Floor(tempoLivre / tempo);
                        InsereLinhaPlaneamento(id, posto, qtdLivre, qtdLivre * tempo);
                        tempoUsado += qtdLivre * tempo;
                        CapacidadeUsada[posto] += qtdLivre * tempo;
                        row["Qtd"] = qtd - qtdLivre;
                        lastAdded = false;
                    }
                }

                lastId = id;
            }

            foreach (DataRow row in toRemove)
                list.Remove(row);

            Util.ProximoTurno(ref turno, ref dia, ref semana, nTurnos);
            ResetCapacidades();
            PlaneamentoTurno(list);
        }

        private bool RespeitaPrecedencia(DataRow row)
        {
            int semanaFusao = Convert.ToInt32(row["SemanaFusao"]);
            int diaFusao = Convert.ToInt32(row["DiaFusao"]);

            if (semana < semanaFusao)
                return false;

            if (semana == semanaFusao)
            {
                if (dia < diaFusao)
                    return false;
                if (dia == diaFusao)
                    return turno > 1;
            }

            return true;
        }

        private void ResetCapacidades()
        {
            foreach (string key in CapacidadeUsada.Keys.ToList())
                CapacidadeUsada[key] = 0;
            tempoUsado = 0;
        }

        private void InsereLinhaPlaneamento(int Id, string Posto, int QtdPecas, decimal Tempo)
        {
            DataRow row = Plano.NewRow();
            row["Id"] = Id;
            row["Semana"] = semana;
            row["Dia"] = dia;
            row["Turno"] = turno;
            row["Posto"] = Posto;
            row["QtdPecas"] = QtdPecas;
            row["Tempo"] = Tempo;

            Plano.Rows.Add(row);

        }

        public void EscreveBD()
        {
            SqlConnection connection = Util.AbreBD();
            int linhas = 0;

            foreach (DataRow row in Plano.Rows)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[PlanCMW$Rebarbagem] VALUES(@Id,@Semana,@Dia,@Turno,@Posto,@QtdPecas,@Tempo)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.Parameters.AddWithValue("@Posto", row["Posto"]);
                cmd.Parameters.AddWithValue("@QtdPecas", row["QtdPecas"]);
                cmd.Parameters.AddWithValue("@Tempo", row["Tempo"]);
                
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE Planeamento.dbo.[PlanCMW$Produtos] SET TurnoRebarbagem = @Turno, DiaRebarbagem = @Dia, SemanaRebarbagem = @Semana WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.ExecuteNonQuery();
                linhas++;
            }

            Console.WriteLine(linhas + " linhas inseridas na tabela Rebarbagem");
        }

        public void LimpaTabelas()
        {
            Produtos.Clear();
            Plano.Clear();
        }
    }
}
