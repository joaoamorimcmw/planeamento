using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class Init
    {
        private static string CodEncomenda = "VE1%";
        private static string CodMacho = "M%";
        private static string CodSemMacho = "M002204";
        private static string CodProduto = "PROD.ACABA";
        private static string DataInicio = "01-01-15";

        //Carrega a tabela Produtos da BD do Planeamento para um DataTable
        public static DataTable GetProdutos()
        {
            String query = "select Include,Id,dbo.NomeLocal(Local) as Local,NoEnc as Encomenda,NoProd as Produto,NoMolde as Molde,[Descricao Liga] as Liga,QtdPendente as Quantidade,CaixasPendente as Caixas,DataPrevista " +
            "from " + Util.TabelaProduto + " Produtos " +
            "order by Local,Id";
            DataTable table = new DataTable();
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query,con);
            table.Load(cmd.ExecuteReader());
            con.Close();
            return table;
        }

        //Faz o reset da tabela dos produtos com base nas encomendas abertas do Navision
        public static void UpdateProdutos()
        {
            SqlConnection con = Util.AbreBD();
            LimpaProdutos(con);
            ReseedProdutos(con);
            InicializaProdutos(con);
            CalcularGitos(con);
            CalculaTempoMachos(con);
            con.Close();
        }

        //Limpa a tabela de Produtos
        private static void LimpaProdutos(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("DELETE " + Util.TabelaProduto, con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Produtos");
        }

        //Reinicia o ID da tabela Produtos
        private static void ReseedProdutos(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT ('" + Util.TabelaProduto + "', RESEED, 0)", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        //Inicializa Produtos e Plano
        private static void InicializaProdutos(SqlConnection con)
        {
            String query = "INSERT INTO " + Util.TabelaProduto + " (NoEnc,NoProd,NoMolde,Liga,[Descricao Liga],PesoPeca,[Peso Gitos],NoMoldes,Local,QtdPendente,CaixasPendente,DataPrevista,Urgente)" +
            "SELECT A.[Document No_],A.[No_],B.[No_ Molde],B.[Liga Metalica],C.Description,A.[Peso Peça [Kg]]],B.[Peso com Gitos [Kg]]],A.[NumeroMoldes],A.[Local de Producao],A.[Outstanding Quantity],CEILING(A.[Outstanding Quantity]/A.[NumeroMoldes]),A.[Planned Delivery Date],A.[Urgente] " +
            "FROM (select * from Navision.dbo.[CMW$Sales Line] WHERE ([Document No_] LIKE @CodEncomenda) AND ([Outstanding Quantity] > 0) AND ([Posting Group] = @CodProduto) AND ([Planned Delivery Date] >= @Data) AND ([Local de Producao] > 0)) as A " +
            "INNER JOIN Navision.dbo.[CMW$Item] as B " +
            "on A.[No_] = B.[No_] " +
            "INNER JOIN (select No_,Description from Navision.dbo.[CMW$Item] where No_ like 'LIG%') as C " +
            "on B.[Liga Metalica] = C.[No_] " +
            "ORDER BY Urgente DESC,[Planned Delivery Date] ASC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CodEncomenda", CodEncomenda);
            cmd.Parameters.AddWithValue("@CodProduto", CodProduto);
            cmd.Parameters.AddWithValue("@Data", DataInicio);
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas inseridas na tabela Produtos");
        }

        //Calcula o peso com gitos para os produtos que não têm
        //Peso com gitos = PesoPeca * NoMoldes * 145%
        private static void CalcularGitos(SqlConnection con)
        {
            decimal percentagemGitos = (decimal)ParametrosBD.GetParametro(ParametrosBD.PercentagemGitos);
            String query = "update " + Util.TabelaProduto + " " +
            "set [Peso Gitos] = PesoPeca * NoMoldes * @Percentagem " +
            "where [Peso Gitos] = 0";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Percentagem", percentagemGitos);
            command.ExecuteNonQuery();
        }

        private static void CalculaTempoMachos(SqlConnection con)
        {
            String query = "update " + Util.TabelaProduto + " " +
            "set TempoMachos = ceiling(A.TempoCaixa) " +
            "from " +
            Util.TabelaProduto + " as Prd " +
            "inner join " +
            "(select Prd.Id as Id, sum(Prd.NoMoldes * Bom.Quantity * Itm.[Tempo Fabrico Machos] / 60) as TempoCaixa from " +
            "(select Id,NoProd,NoMoldes from " + Util.TabelaProduto + ") as Prd " +
            "left join (select [Production BOM No_],[No_],Quantity from Navision.dbo.[CMW$Production BOM Line] where No_ like @CodMacho) as Bom " +
            "on Prd.NoProd + '#' = Bom.[Production BOM No_] " +
            "inner join (select [No_], [Tempo Fabrico Machos] from Navision.dbo.CMW$Item where [No_] like @CodMacho) as Itm " +
            "on isnull(Bom.[No_],@CodSemMacho) = Itm.[No_] " +
            "group by Id) as A " +
            "on Prd.Id = A.Id";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@CodMacho", CodMacho);
            command.Parameters.AddWithValue("@CodSemMacho", CodSemMacho);
            command.ExecuteNonQuery();
        }

        //Recebe um DataTable com a lista de produtos e exclui os produtos marcados
        public static void ExcluirProdutosLista(DataTable table)
        {
            SqlConnection con = Util.AbreBD();
            
            foreach (DataRow row in table.Rows)
            {
                int bit = ((bool)row["Include"]) ? 1 : 0;
                int id = Convert.ToInt32(row["Id"]);
                string query = "update " + Util.TabelaProduto + " set Include = @bit where Id = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@bit", bit);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            con.Close();
        }

        //Todos os produtos em que o total da carga para liga em encomendas é inferior ao minimo são marcados como excluídos
        public static void ExcluiProdutosBaixaCarga(Fusao fusao)
        {
            String query = "update " + Util.TabelaProduto + " " +
            "set Include = 0 " +
            "output inserted.NoEnc as Enc,inserted.NoProd as Prod " +
            "where Id in " +
            "(select Id " +
            "from dbo.[PlanCMW$Produtos] Prod " +
            "inner join " +
            "(select dbo.GetFabrica(Local) as Fabrica,Liga,sum(PesoPeca * QtdPendente) as PesoTotal " +
            "from dbo.[PlanCMW$Produtos] " +
            "group by dbo.GetFabrica(Local),Liga " +
            "having sum(PesoPeca * QtdPendente) < (CASE WHEN dbo.GetFabrica(Local) = 1 THEN @MinimoCMW1 ELSE @MinimoCMW2 END)) Soma " +
            "on dbo.GetFabrica(Prod.Local) = Soma.Fabrica and Prod.Liga = Soma.Liga)";

            Console.WriteLine("Produtos ignorados por falta de carga");
            
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@MinimoCMW1", fusao.FusaoMinima(1));
            command.Parameters.AddWithValue("@MinimoCMW2", fusao.FusaoMinima(2));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine ("Encomenda: " + reader["Enc"] + ", Produto: " + reader["Prod"]);
            reader.Close();
            con.Close();
        }
    }
}
