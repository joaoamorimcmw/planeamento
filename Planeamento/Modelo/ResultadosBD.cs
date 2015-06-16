using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class ResultadosBD
    {
        public static int MaxSemanaCMW1()
        {
            String query = "select isnull(max(Semana),0) from " + Util.TabelaPlano + " where Local = 1";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int) cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        public static DataTable GetPlanoCMW1(int semana)
        {
            String query = "select Prd.NoEnc as Encomenda,Prd.DataPrevista as [Data Prevista],Prd.NoProd as Produto,Prd.NoMolde as Molde,Prd.[Descricao Liga] as Liga,Plano.Caixas,Plano.Caixas * Prd.[Peso Gitos] as [Peso Total (kg)], Plano.Caixas * Prd.TempoMachos as [Macharia (min)] from " +
            "(select Id,Caixas from " + Util.TabelaPlano + " where Local = @Local and Semana = @Semana) Plano " +
            "inner join " + Util.TabelaProduto + " Prd " +
            "on Plano.Id = Prd.Id";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static DataTable LigasCMW1(int semana)
        {
            String query = "select Prd.[Descricao Liga] as Liga, sum(Plano.Caixas * Prd.[Peso Gitos]) as [Peso Total (kg)] from " +
            "(select Id,Caixas from " + Util.TabelaPlano + " where Local = @Local and Semana = @Semana) Plano " +
            "inner join " + Util.TabelaProduto + " Prd " +
            "on Plano.Id = Prd.Id " +
            "group by Prd.[Descricao Liga]";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static int MachariaCMW1(int semana)
        {
            String query = "select isnull(sum(Plano.Caixas * Prd.TempoMachos),0) from " +
            "(select Id,Caixas from " + Util.TabelaPlano + " where Local = @Local and Semana = @Semana) Plano " +
            "inner join " + Util.TabelaProduto + " Prd " +
            "on Plano.Id = Prd.Id " +
            "group by Prd.[Descricao Liga]";

            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1);
            int caixas = (int)cmd.ExecuteScalar();
            con.Close();

            return caixas;
        }

        public static int CaixasGF(int semana)
        {
            string query = "select isnull(sum(Caixas),0) from " + Util.TabelaPlano + " where Local = @Local and Semana = @Semana";

            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1);
            int caixas = (int)cmd.ExecuteScalar();
            con.Close();

            return caixas;
        }

        public static int MaxSemanaCMW2()
        {
            String query = "select isnull(max(Semana),0) from " + Util.TabelaPlano + " where Local > 1";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int)cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        public static DataTable GetPlanoCMW2(int semana)
        {
            String query = "select Prd.NoEnc as Encomenda,Prd.DataPrevista as [Data Prevista],Prd.NoProd as Produto,Prd.NoMolde as Molde,Prd.[Descricao Liga] as Liga,Plano.Caixas,Plano.Caixas * Prd.[Peso Gitos] as [Peso Total (kg)], Plano.Caixas * Prd.TempoMachos as [Macharia (min)] from " +
            "(select Id,Caixas from " + Util.TabelaPlano + " where Local > @Local and Semana = @Semana) Plano " +
            "inner join " + Util.TabelaProduto + " Prd " +
            "on Plano.Id = Prd.Id";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1); //na query muda Local = 1 para Local > 1

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static DataTable LigasCMW2(int semana)
        {
            String query = "select Prd.[Descricao Liga] as Liga, sum(Plano.Caixas * Prd.[Peso Gitos]) as [Peso Total (kg)] from " +
            "(select Id,Caixas from " + Util.TabelaPlano + " where Local > @Local and Semana = @Semana) Plano " +
            "inner join " + Util.TabelaProduto + " Prd " +
            "on Plano.Id = Prd.Id " +
            "group by Prd.[Descricao Liga]";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1); //na query muda Local = 1 para Local > 1

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static int MachariaCMW2(int semana)
        {
            String query = "select isnull(sum(Plano.Caixas * Prd.TempoMachos),0) from " +
            "(select Id,Caixas from " + Util.TabelaPlano + " where Local > @Local and Semana = @Semana) Plano " +
            "inner join " + Util.TabelaProduto + " Prd " +
            "on Plano.Id = Prd.Id " +
            "group by Prd.[Descricao Liga]";

            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 1);
            int caixas = (int) cmd.ExecuteScalar();
            con.Close();

            return caixas;
        }

        public static int CaixasIMF(int semana)
        {
            string query = "select isnull(sum(Caixas),0) from " + Util.TabelaPlano + " where Local = @Local and Semana = @Semana";

            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 2);
            int caixas = (int)cmd.ExecuteScalar();
            con.Close();

            return caixas;
        }

        public static int CaixasManual(int semana)
        {
            string query = "select isnull(sum(Caixas),0) from " + Util.TabelaPlano + " where Local = @Local and Semana = @Semana";

            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);
            cmd.Parameters.AddWithValue("@Local", 3);
            int caixas = (int)cmd.ExecuteScalar();
            con.Close();

            return caixas;
        }


        //Procura na tabela do plano da Rebarbagem qual a maior Semana
        public static int MaxSemanaRebarbagem()
        {
            String query = "select isnull(max(Semana),0) from PlanCMW$Rebarbagem";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int)cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        //Devolve todas as linhas na tabela do plano da Rebarbagem
        public static DataTable GetRebarbagem(int semana, int dia, int turno)
        {
            String query = "select Encomenda, Produto, Posto, Quantidade, Tempo " +
            "from Planeamento.dbo.PlanoRebarbagem " +
            "where Semana = @semana and Dia = @dia and Turno = @turno";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@semana", semana);
            cmd.Parameters.AddWithValue("@dia", dia);
            cmd.Parameters.AddWithValue("@turno", turno);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static DataTable GetEncomenda(string encomenda)
        {
            String query = "select Prd.NoProd,Pln.Semana,dbo.NomeLocal(Prd.Local) as Local,Pln.Caixas, Pln.Caixas * NoMoldes as Pecas, Pln.Caixas * NoMoldes * PesoPeca  as [Peso Total (kg)] from " +
            "(select * from " + Util.TabelaProduto + " where NoEnc = @NoEnc) Prd " +
            "inner join " + Util.TabelaPlano + " Pln " +
            "on Prd.Id = Pln.Id " +
            "order by Semana";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@NoEnc", encomenda.ToUpper());

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static int MaxSemana()
        {
            String query = "select isnull(max(Semana),0) from " + Util.TabelaPlano;
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int)cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        public static DataTable CargaRebarbagem(int semana)
        {
            String query = "select Rout.No_ as [Código],Mac.Name as [Máquina],sum(Prd.NoMoldes * Pln.Caixas) as [Total Peças],sum(Prd.NoMoldes * Pln.Caixas * Rout.[Run Time]) as [Tempo Total (min)], sum(Prd.NoMoldes * Pln.Caixas * Prd.[PesoPeca]) as [Peso Total (kg)] from " +
            Util.TabelaProduto + " Prd " +
            "inner join " +
            "(select * from " + Util.TabelaPlano + " where Semana = @Semana) Pln " +
            "on Prd.Id = Pln.Id " +
            "inner join " +
            Util.NavisionRoutingLine + " Rout " +
            "on Prd.NoProd = Rout.[Routing No_] " +
            "inner join " +
            Util.NavisionMachineCenter + " Mac " +
            "on Rout.No_ = Mac.No_ " +
            "group by Rout.No_,Mac.Name " +
            "having sum(Prd.NoMoldes * Pln.Caixas * Rout.[Run Time]) > 0 " +
            "order by dbo.OrdenaMaquinas(Rout.No_ )";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Semana", semana);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }
    }
}
