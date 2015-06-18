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
        public static string NavisionRoutingLine = "Navision.dbo.[CMW$Routing Line]";
        public static string NavisionMachineCenter = "Navision.dbo.[CMW$Machine Center]";

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
    }
}
