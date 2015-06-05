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

        //Gera uma query à tabela Sales Line
        public static String QuerySalesLine(String colunas, int local = 0, String data = "01-01-14")
        {
            String strLocal = "> 0";
            if (local == 1)
                strLocal = "= 1";
            if (local == 2)
                strLocal = "> 1";

            String sql = "SELECT " + colunas + " FROM Planeamento.dbo.[CMW$Sales Line] WHERE ([Outstanding Quantity]>0) AND ([Posting Group]='PROD.ACABA') AND [Planned Delivery Date] >= '" + data + "' AND ([Local de Producao]" + strLocal + ") ORDER BY [Local de Producao] ASC,Urgente DESC,[Planned Delivery Date] ASC";

            return sql;
        }
    }
}
