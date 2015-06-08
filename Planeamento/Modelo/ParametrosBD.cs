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
        public static string PercentagemGitos = "Gitos";
        public static string Horario = "Horas";
        public static string MinimoFusao = "Mínimo Fusão";

        public static string TurnosCMW1 = "Turnos CMW1";
        public static string TurnosCMW2 = "Turnos CMW2";
        public static string TurnosRebarbagem = "Turnos Rebarbagem";

        public static string MachariaCMW1 = "Capacidade Macharia CMW1";
        public static string CaixasGF = "Caixas GF";
        public static string FusoesCMW1 = "Fusões CMW1";

        public static string Forno1CMW1 = "Capacidade Forno 1 CMW1";
        public static string Forno2CMW1 = "Capacidade Forno 2 CMW1";
        public static string Forno3CMW1 = "Capacidade Forno 3 CMW1";
        public static string Forno4CMW1 = "Capacidade Forno 4 CMW1";

        public static string MachariaCMW2 = "Capacidade Macharia CMW2";
        public static string CaixasIMF = "Caixas IMF";
        public static string CaixasManual = "Caixas Manual";
        public static string FusoesCMW2 = "Fusões CMW2";

        public static string Forno1CMW2 = "Capacidade Forno 1 CMW2";
        public static string Forno2CMW2 = "Capacidade Forno 2 CMW2";
        public static string Forno3CMW2 = "Capacidade Forno 3 CMW2";
        public static string Forno4CMW2 = "Capacidade Forno 4 CMW2";

        public static string FuncionariosTurnoRebarbagem = "Funcionários por turno rebarbagem";

        //Adiciona os valores default à tabela de parametros caso estes não estejam definidos
        public static void ParametrosDefault()
        {
            AddParametroIfNull(PercentagemGitos, 1.45);
            AddParametroIfNull(Horario, 400);
            AddParametroIfNull(MinimoFusao, 0.66);

            AddParametroIfNull(TurnosCMW1, 2);
            AddParametroIfNull(TurnosCMW2, 3);
            AddParametroIfNull(TurnosRebarbagem, 3);

            AddParametroIfNull(MachariaCMW1, 6);
            AddParametroIfNull(CaixasGF, 550);
            AddParametroIfNull(FusoesCMW1, 10);
            
            AddParametroIfNull(Forno1CMW1, 3000);
            AddParametroIfNull(Forno2CMW1, 3000);
            AddParametroIfNull(Forno3CMW1, 1000);
            AddParametroIfNull(Forno4CMW1, 1000);

            AddParametroIfNull(MachariaCMW2, 4);
            AddParametroIfNull(CaixasIMF, 250);
            AddParametroIfNull(CaixasManual, 20);
            AddParametroIfNull(FusoesCMW2, 8);

            AddParametroIfNull(Forno1CMW2, 1750);
            AddParametroIfNull(Forno2CMW2, 1100);
            AddParametroIfNull(Forno3CMW2, 1100);
            AddParametroIfNull(Forno4CMW2, 800);

            AddParametroIfNull(FuncionariosTurnoRebarbagem, 25);
        }

        //Vai buscar o valor de um parametro
        public static Object GetParametro (string parametro)
        {
            String query = "select Valor from " + Util.TabelaParametros + " where Parametro = @Param";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Param", parametro);
            Object valor = cmd.ExecuteScalar();
            con.Close();

            return valor;
        }

        //Altera o valor de um parametro
        public static void SetParametro(string parametro, Object valor)
        {
            String query = "update " + Util.TabelaParametros + " set Valor = @Valor where Parametro = @Param";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Param", parametro);
            cmd.Parameters.AddWithValue("@Valor", valor);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //Adiciona o valor de um parametro caso este nao exista
        private static void AddParametroIfNull(string parametro, Object valor)
        {
            if (GetParametro(parametro) == null)
                AddParametro(parametro, valor);
        }

        //Adiciona um parametro novo
        private static void AddParametro(string parametro, Object valor)
        {
            String query = "insert into " + Util.TabelaParametros + " values (@Param,@Valor)";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Param", parametro);
            cmd.Parameters.AddWithValue("@Valor", valor);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static decimal CalculaMinimoFusao(int Fabrica)
        {
            decimal forno1, forno2, forno3, forno4;

            if (Fabrica == 1)
            {
                forno1 = (decimal)GetParametro(Forno1CMW1);
                forno2 = (decimal)GetParametro(Forno2CMW1);
                forno3 = (decimal)GetParametro(Forno3CMW1);
                forno4 = (decimal)GetParametro(Forno4CMW1);
            }

            else
            {
                forno1 = (decimal)GetParametro(Forno1CMW2);
                forno2 = (decimal)GetParametro(Forno2CMW2);
                forno3 = (decimal)GetParametro(Forno3CMW2);
                forno4 = (decimal)GetParametro(Forno4CMW2);
            }

            decimal minimo = (decimal)GetParametro(MinimoFusao);
            decimal [] caps = {forno1,forno2,forno3,forno4};
            return caps.Min() * minimo;
        }
    }
}
