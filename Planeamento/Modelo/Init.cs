using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    class Init
    {
        //Carrega a tabela Produtos da BD do Planeamento para um DataTable
        public static DataTable GetProdutos()
        {
            String query = "select Include,Id,NoEnc as Encomenda,NoProd as Produto,Ligas.Descricao as Liga,QtdPendente,DataPrevista " +
            "from Planeamento.dbo.[PlanCMW$Produtos] Produtos " +
            "inner join Planeamento.dbo.[PlanCMW$Ligas] Ligas " +
            "on Produtos.Liga = Ligas.Liga " +
            "order by Id";
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
            con.Close();
        }

        //Limpa a tabela de Produtos
        private static void LimpaProdutos(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Produtos]", con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Produtos");
        }

        //Reinicia o ID da tabela Produtos
        private static void ReseedProdutos(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT ('Planeamento.dbo.[PlanCMW$Produtos]', RESEED, 0)", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        //Inicializa Produtos e Plano
        private static void InicializaProdutos(SqlConnection con)
        {
            String query = "INSERT INTO dbo.[PlanCMW$Produtos] (NoEnc,NoProd,NoMolde,Liga,PesoPeca,[Peso Gitos],NoMoldes,Local,QtdPendente,DataPrevista,Urgente)" +
            "SELECT A.[Document No_],A.[No_],B.[No_ Molde],B.[Liga Metalica],A.[Peso Peça [Kg]]],B.[Peso com Gitos [Kg]]],A.[NumeroMoldes],A.[Local de Producao],A.[Outstanding Quantity],A.[Planned Delivery Date],A.[Urgente] " +
            "FROM Navision.dbo.[CMW$Sales Line] as A " +
            "INNER JOIN Navision.dbo.[CMW$Item] as B " +
            "on A.[No_] = B.[No_] " +
            "WHERE ([Document No_] LIKE 'VE1%') AND ([Outstanding Quantity]>0) AND ([Posting Group]='PROD.ACABA') AND [Planned Delivery Date] >= '01-01-15' AND ([Local de Producao] >0) " +
            "ORDER BY Urgente DESC,[Planned Delivery Date] ASC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas inseridas na tabela Produtos");
        }

        public static void ExcluirProdutosLista(DataTable table)
        {
            SqlConnection con = Util.AbreBD();
            
            foreach (DataRow row in table.Rows)
            {
                int bit = ((bool)row["Include"]) ? 1 : 0;
                int id = Convert.ToInt32(row["Id"]);
                string query = "update dbo.[PlanCMW$Produtos] set Include = @bit where Id = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@bit", bit);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            con.Close();
        }

        public static void ExcluiProdutosBaixaCarga(Fusao fusao)
        {
            String query = "update dbo.[PlanCMW$Produtos] " +
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

        public static void InicializaLigas()
        {
            String query = "delete from dbo.PlanCMW$Ligas";
            SqlConnection con = Util.AbreBD();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Ligas");

            query = "insert into dbo.PlanCMW$Ligas " +
            "select Ligas.No_,Ligas.Description,Classes.Code,Classes.Descricao as Liga " +
            "from Navision.dbo.CMW$Item Ligas " +
            "left join Navision.dbo.CMW$Item Item " +
            "on Ligas.[Liga Metalica] = Item.No_ " +
            "inner join " +
            "(select Code, Descricao from Navision.dbo.[CMW$Parametrizações Extra] where Tabela = 10) Classes " +
            "on Ligas.[Code Classe Metais] = Classes.Code " +
            "where Ligas.No_ like 'LIG%' ";
            cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas inseridas na tabela Ligas");
        }
    }
}
