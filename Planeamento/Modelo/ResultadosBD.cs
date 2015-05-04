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

        public static DataTable GetMacharia(int fabrica, int semana, int dia)
        {
            String query = "select Prod.NoEnc as Encomenda,Prod.NoProd as Produto,Macho.CodMach as Macho,Macho.Qtd as Quantidade " +
            "from Planeamento.dbo.[PlanCMW$Produtos] Prod " +
            "inner join Planeamento.dbo.[PlanCMW$Macharia] Macho " +
            "on Prod.Id = Macho.Id " +
            "where Macho.Fabrica = @fabrica and Macho.Semana = @semana and Macho.Dia = @dia";

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

        public static DataTable GetMoldacao(int local, int semana, int dia, int turno)
        {
            String query = "select Prod.NoEnc as Encomenda,Prod.NoProd as Produto,Mold.Caixas " +
            "from Planeamento.dbo.[PlanCMW$Produtos] Prod " +
            "inner join Planeamento.dbo.[PlanCMW$Moldacao] Mold " +
            "on Prod.Id = Mold.Id " +
            "where Mold.Local = @local and Mold.Semana = @semana and Mold.Dia = @dia and Mold.Turno = @turno";

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

        public static DataTable GetFusao(int fabrica, int semana, int dia, int turno)
        {
            String query = "select Forno, NoFusao, Liga, PesoTotal as Carga " +
            "from Planeamento.dbo.[PlanCMW$Fusao] " +
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
            String query = "select Prod.NoEnc as Encomenda,Prod.NoProd as Produto,Reb.Posto as Posto, Reb.QtdPecas as Quantidade, Reb.Tempo as Tempo " +
            "from Planeamento.dbo.[PlanCMW$Produtos] Prod " +
            "inner join Planeamento.dbo.[PlanCMW$Rebarbagem] Reb " +
            "on Prod.Id = Reb.Id " +
            "where Reb.Semana = @semana and Reb.Dia = @dia and Reb.Turno = @turno";

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
