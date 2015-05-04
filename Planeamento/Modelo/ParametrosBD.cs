using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    class ParametrosBD
    {
        public static string Horario = "Horas";
        public static string Turnos = "Turnos";

        public static string Macharia1 = "Capacidade Macharia CMW1";
        public static string Macharia2 = "Capacidade CMW2";

        public static string MoldacaoGF = "Capacidade Moldação GF";
        public static string MoldacaoIMF = "Capacidade Moldação IMF";
        public static string MoldacaoManual = "Capacidade Moldação Manual";

        public static string FusoesTurnoForno = "Fusões por forno";
        public static string FusoesTurnoTotal = "Fusões por turno";
        public static string MinimoFusao = "Mínimo Fusão";
        public static string Forno1CMW1 = "Capacidade Forno 1 CMW1";
        public static string Forno2CMW1 = "Capacidade Forno 2 CMW1";
        public static string Forno3CMW1 = "Capacidade Forno 3 CMW1";
        public static string Forno4CMW1 = "Capacidade Forno 4 CMW1";
        public static string Forno1CMW2 = "Capacidade Forno 1 CMW2";
        public static string Forno2CMW2 = "Capacidade Forno 2 CMW2";
        public static string Forno3CMW2 = "Capacidade Forno 3 CMW2";
        public static string Forno4CMW2 = "Capacidade Forno 4 CMW2";

        public static string FuncionariosTurnoRebarbagem = "Funcionários por turno rebarbagem";

        public static void ParametrosDefault()
        {
            AddParametroIfNull(Horario, 400);
            AddParametroIfNull(Turnos, 3);
            AddParametroIfNull(Macharia1, 5);
            AddParametroIfNull(Macharia2, 4);
            AddParametroIfNull(MoldacaoGF, 420);
            AddParametroIfNull(MoldacaoIMF, 95);
            AddParametroIfNull(MoldacaoManual, 12);
            AddParametroIfNull(FusoesTurnoForno, 2);
            AddParametroIfNull(FusoesTurnoTotal, 8);
            AddParametroIfNull(MinimoFusao, 0.66);
            AddParametroIfNull(Forno1CMW1, 3000);
            AddParametroIfNull(Forno2CMW1, 3000);
            AddParametroIfNull(Forno3CMW1, 1000);
            AddParametroIfNull(Forno4CMW1, 1000);
            AddParametroIfNull(Forno1CMW2, 1750);
            AddParametroIfNull(Forno2CMW2, 1100);
            AddParametroIfNull(Forno3CMW2, 1100);
            AddParametroIfNull(Forno4CMW2, 800);
            AddParametroIfNull(FuncionariosTurnoRebarbagem, 25);
        }

        public static Object GetParametro (string parametro)
        {
            String query = "select Valor from PlanCMW$Parametros where Parametro = @Param";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Param", parametro);
            Object valor = cmd.ExecuteScalar();
            con.Close();

            return valor;
        }

        public static void SetParametro(string parametro, Object valor)
        {
            String query = "update PlanCMW$Parametros set Valor = @Valor where Parametro = @Param";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Param", parametro);
            cmd.Parameters.AddWithValue("@Valor", valor);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private static void AddParametroIfNull(string parametro, Object valor)
        {
            if (GetParametro(parametro) == null)
                AddParametro(parametro, valor);
        }

        private static void AddParametro(string parametro, Object valor)
        {
            String query = "insert into PlanCMW$Parametros values (@Param,@Valor)";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Param", parametro);
            cmd.Parameters.AddWithValue("@Valor", valor);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
