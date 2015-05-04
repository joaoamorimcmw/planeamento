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
        public static int MaxSemanaMacharia()
        {
            String query = "select isnull(max(Semana),0) from PlanCMW$Macharia";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int) cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        public static DataTable GetMacharia(string fabrica, int semana, int dia)
        {
            String query = "select Encomenda, Produto, Macho, Quantidade " +
            "from Planeamento.dbo.PlanoMacharia " +
            "where Fabrica = @fabrica and Semana = @semana and Dia = @dia";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@fabrica", fabrica);
            cmd.Parameters.AddWithValue("@semana", semana);
            cmd.Parameters.AddWithValue("@dia", dia);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static int MaxSemanaMoldacao()
        {
            String query = "select isnull(max(Semana),0) from PlanCMW$Moldacao";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int)cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        public static DataTable GetMoldacao(string local, int semana, int dia, int turno)
        {
            String query = "select Encomenda, Produto, Caixas " +
            "from Planeamento.dbo.PlanoMoldacao " +
            "where Local = @local and Semana = @semana and Dia = @dia and Turno = @turno";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@local", local);
            cmd.Parameters.AddWithValue("@semana", semana);
            cmd.Parameters.AddWithValue("@dia", dia);
            cmd.Parameters.AddWithValue("@turno", turno);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static int MaxSemanaFusao()
        {
            String query = "select isnull(max(Semana),0) from PlanCMW$Fusao";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int)cmd.ExecuteScalar();
            con.Close();

            return max;
        }

        public static DataTable GetFusao(string fabrica, int semana, int dia, int turno)
        {
            String query = "select Forno, NoFusao, Liga, Descricao, Carga " +
            "from Planeamento.dbo.PlanoFusao " +
            "where Fabrica = @fabrica and Semana = @semana and Dia = @dia and Turno = @turno";

            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@fabrica", fabrica);
            cmd.Parameters.AddWithValue("@semana", semana);
            cmd.Parameters.AddWithValue("@dia", dia);
            cmd.Parameters.AddWithValue("@turno", turno);

            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        public static int MaxSemanaRebarbagem()
        {
            String query = "select isnull(max(Semana),0) from PlanCMW$Rebarbagem";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            int max = (int)cmd.ExecuteScalar();
            con.Close();

            return max;
        }

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
    }
}
