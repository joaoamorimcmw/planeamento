using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class Util
    {
        public static string TabelaProduto = "Planeamento.dbo.PlanCMWv2$Produtos";
        public static string TabelaPlano = "Planeamento.dbo.PlanCMWv2$PlanoProducao";
        public static string TabelaParametros = "Planeamento.dbo.PlanCMWv2$Parametros";

        public static string NavisionItem = "Navision.dbo.CMW$Item";
        public static string NavisionSalesLine = "Navision.dbo.[CMW$Sales Line]";
        public static string NavisionBOMLine = "Navision.dbo.[CMW$Production BOM Line]";

        //Função auxiliar para abrir a ligação à BD de planeamento
        public static SqlConnection AbreBD ()
        {
            SqlConnection connection;

            try
            {
                connection = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
                System.Console.WriteLine("Erro na conexão.");
                connection = null;
            }

            return connection;
        }

        //Calcula o proximo turno (se necessário passa para o próximo dia e semana)
        public static void ProximoTurno(ref int turno, ref int dia, ref int semana, int nTurnos)
        {
            turno = (turno % nTurnos) + 1;
            if (turno == 1)
                ProximoDia(ref dia, ref semana);
        }
        
        //Calcula o próximo dia (se necessário passa para a próxima semana)
        public static void ProximoDia(ref int dia, ref int semana) 
        {
            dia = (dia % 5) + 1;
            if (dia == 1)
                semana++;
        }

        //Função auxiliar para fechar uma ligação à BD
        public static void FechaBD (SqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
                System.Console.WriteLine("Erro na conexão.");
            }
        }

        #region stuff
        private double Clean(double vbase, double tirs, double tss, double st, double smin)
        {
            double txd = vbase * (1 - tirs - tss);
            double vst = st * (txd - smin);
            return txd - vst;
        }

        private void doStuff()
        {
            double clean = Clean(1250, 0.165, 0.11, 0.035, 505);
            int yearDays = 52 * 5 - 21;
            double totalClean = clean * 14;
            double totalSa = 6.83 * yearDays;
            double total = totalClean + totalSa;
            Predict(4700, total, 0.05, 0.5, 0.08);
        }

        private void Predict(double start, double aBase, double aRaise, double spend, double rate)
        {
            double acc = start;
            Console.WriteLine("Year\tSavings \tProfit  \tAcc");
            Console.WriteLine("2015\t  ----  \t  ----  \t" + FormatM(start));
            for (int i = 0; i <= 14; i++)
            {
                double profit = acc * rate;
                double savings = (1 - spend) * aBase * Math.Pow(1 + aRaise, (double)i);
                acc += profit + savings;
                int year = 2016 + i;
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", year, FormatN(savings, 9), FormatN(profit, 8), FormatM(acc));
            }

        }

        private static string FormatN(double value, int chars)
        {
            string formatted = FormatM(value);
            while (formatted.Length < chars)
                formatted += " ";
            return formatted;
        }

        private static string FormatM(double value, string cur = "")
        {
            return value.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + cur;
        }

        #endregion
    }
}
