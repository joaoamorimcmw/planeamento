using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Nesta classe tem os métodos para popular a Base de Dados do Planeamento com base no Navision
 * Calcula o tempo total de machos, peso c/ gitos, produtos sem carga, etc. 
 * 
 * */

namespace Planeamento
{
    public class Init
    {
        private static string CodEncomenda = "VE1%"; //Código para procurar as encomendas na Sales Line
        private static string CodMacho = "M%"; //Código para procurar machos nas listas de materiais
        private static string CodLiga = "LIG%"; //Código para procurar ligas na tabela Item
        private static string CodSemMacho = "M002204"; //Código de produto sem macho na lista de materiais
        private static string CodProduto = "PROD.ACABA"; //Código de tipo de produto associado a produtos acabados
        private static string DataInicio = "01-01-15"; //Apenas são consideradas encomendas feitas depois desta data (evita ter em conta encomendas antigas que ainda estão abertas no Navision)

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

        /* Faz o reset da tabela dos produtos com base nas encomendas abertas do Navision
         * A ordem é:
         * 1 - Limpar todas as linhas da tabela de Produtos
         * 2 - Fazer um reseed da coluna do Id (para começar a 1)
         * 3 - Ler os produtos nas encomendas em aberto e popular a tabela de produtos
         * 4 - Calcular o peso com gitos para os produtos que não têm já calculado no Navision
         * 5 - Calcular o tempo total de macharia para cada caixa de produto
         * */
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
            String query = "DELETE " + Util.TabelaProduto;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Produtos");
        }

        //Reinicia o ID da tabela Produtos
        private static void ReseedProdutos(SqlConnection con)
        {
            String query = "DBCC CHECKIDENT ('" + Util.TabelaProduto + "', RESEED, 0)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        //Inicializa Produtos e Plano
        //Procura na Sales Line encomendas em aberto (outstanding quantity > 0), com código do tipo 'VE1*', com produtos do tipo produto acabado, e com data planeada acima da DataInicio
        //O numero de caixas pendentes é calculado dividindo o numero de peças pendentes pelo numero de moldes e arredondando para cima.
        private static void InicializaProdutos(SqlConnection con)
        {
            String query = "INSERT INTO " + Util.TabelaProduto + " (NoEnc,NoProd,NoMolde,Equipamento,Liga,[Descricao Liga],PesoPeca,[Peso Gitos],NoMoldes,Local,QtdPendente,CaixasPendente,DataPrevista,Urgente)" +
            "SELECT A.[Document No_],A.[No_],B.[No_ Molde],B.[Descricao Equip],B.[Liga Metalica],C.Description,A.[Peso Peça [Kg]]],B.[Peso com Gitos [Kg]]],A.[NumeroMoldes],A.[Local de Producao],A.[Outstanding Quantity],CEILING(A.[Outstanding Quantity]/A.[NumeroMoldes]),A.[Planned Delivery Date],A.[Urgente] " +
            "FROM (select * from " + Util.NavisionSalesLine + " WHERE ([Document No_] LIKE @CodEncomenda) AND ([Outstanding Quantity] > 0) AND ([Posting Group] = @CodProduto) AND ([Planned Delivery Date] >= @Data) AND ([Local de Producao] > 0)) as A " +
            "INNER JOIN " + Util.NavisionItem + " as B " +
            "on A.[No_] = B.[No_] " +
            "INNER JOIN (select No_,Description from " + Util.NavisionItem + " where No_ like @CodLiga) as C " +
            "on B.[Liga Metalica] = C.[No_] " +
            "ORDER BY Urgente DESC,[Planned Delivery Date] ASC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CodEncomenda", CodEncomenda);
            cmd.Parameters.AddWithValue("@CodProduto", CodProduto);
            cmd.Parameters.AddWithValue("@CodLiga",CodLiga);
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

        //Calcula o tempo de macharia para todos os produtos (por caixa)

        private static void CalculaTempoMachos(SqlConnection con)
        {
            String query = "update " + Util.TabelaProduto + " " +
            "set TempoMachos = ceiling(isnull(A.TempoCaixa,0)) " +
            "from " +
            Util.TabelaProduto + " as Prd " +
            "inner join " +
            "(select Prd.Id as Id, sum(Prd.NoMoldes * Bom.Quantity * Itm.[Tempo Fabrico Machos] / 60) as TempoCaixa from " +
            "(select Id,NoProd,NoMoldes from " + Util.TabelaProduto + ") as Prd " +
            "left join (select [Production BOM No_],[No_],Quantity from " + Util.NavisionBOMLine + " where No_ like @CodMacho) as Bom " +
            "on Prd.NoProd + '#' = Bom.[Production BOM No_] " +
            "inner join (select [No_], [Tempo Fabrico Machos] from " + Util.NavisionItem + " where [No_] like @CodMacho) as Itm " +
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
        public static void ExcluiProdutosBaixaCarga()
        {
            String query = "update " + Util.TabelaProduto + " " +
            "set Include = 0 " +
            "output inserted.NoEnc as Enc,inserted.NoProd as Prod " +
            "where Id in " +
            "(select Id " +
            "from " + Util.TabelaProduto + " Prod " +
	        "inner join " +
		    "(select dbo.GetFabrica(Local) as Fabrica,Liga,sum([Peso Gitos] * CaixasPendente) as PesoTotal " +
            "from " + Util.TabelaProduto + " " +
		    "group by dbo.GetFabrica(Local),Liga " +
            "having sum([Peso Gitos] * CaixasPendente) < (CASE WHEN dbo.GetFabrica(Local) = 1 THEN @MinimoCMW1 ELSE @MinimoCMW2 END) " +
	        ") Somas " +
	        "on Prod.Liga = Somas.Liga and dbo.GetFabrica(Prod.Local) = Somas.Fabrica)";

            Console.WriteLine("Produtos ignorados por falta de carga");
            
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@MinimoCMW1", ParametrosBD.CalculaMinimoFusao(1));
            command.Parameters.AddWithValue("@MinimoCMW2", ParametrosBD.CalculaMinimoFusao(2));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine ("Encomenda: " + reader["Enc"] + ", Produto: " + reader["Prod"]);
            reader.Close();
            con.Close();
        }

        //Limpa as linhas existentes na tabela do Plano de Producao
        public static void LimpaBDPlanoProducao()
        {
            String query = "delete from " + Util.TabelaPlano;
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            int linhas = command.ExecuteNonQuery();
            con.Close();
            Console.WriteLine(linhas + " linhas eliminadas da tabela PlanoProducao");
        }
    }
}
