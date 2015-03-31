using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    class BDInit
    {
        public BDInit() 
        {
            SqlConnection con = Util.AbreBD();
            LimpaProdutos(con);
            ReseedProdutos(con);
            InicializaProdutos(con);
            con.Close();
            /*LimpaProdutosPlanBD();
            LimpaNumeracaoBD();
            ReseedNumeracaoBD();
            InicializaNumeracaoBD();*/
        }

        //Limpa a tabela de Produtos
        private void LimpaProdutos(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Produtos]", con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Produtos");
        }

        //Reinicia o ID da tabela Produtos
        private void ReseedProdutos(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT ('Planeamento.dbo.[PlanCMW$Produtos]', RESEED, 0)", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        //Inicializa Produtos e Plano
        private void InicializaProdutos(SqlConnection con)
        {
            String query = "INSERT INTO dbo.[PlanCMW$Produtos] (NoEnc,NoLine,NoProd,Liga,PesoPeca,NoMoldes,Local,QtdPendente,DataPrevista,Urgente)" +
            "SELECT A.[Document No_],A.[Line No_],A.[No_],B.[Liga Metalica],A.[Peso Peça [Kg]]],A.[NumeroMoldes],A.[Local de Producao],A.[Outstanding Quantity],A.[Planned Delivery Date],A.[Urgente] " +
            "FROM dbo.[CMW$Sales Line] as A " +
            "INNER JOIN dbo.[CMW$Item] as B " +
            "on A.[No_] = B.[No_] " +
            "WHERE ([Outstanding Quantity]>0) AND ([Posting Group]='PROD.ACABA') AND [Planned Delivery Date] >= '01-01-14' AND ([Local de Producao] >0) " +
            "ORDER BY dbo.GetFabrica([Local de Producao]) ASC,Urgente DESC,[Planned Delivery Date] ASC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas inseridas na tabela Produtos");
        }



        /* Funções de inicialização antigas
         * 
        //Elimina todos os registos da tabela Produtos Plan

        private void LimpaProdutosPlanBD ()
        {
            SqlConnection con = Util.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Produtos Plan]", con);
            cmd2.CommandType = CommandType.Text;
            int linhas = cmd2.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Produtos Plan");
            Util.FechaBD(con);
        }

        //Elimina todos os registos existentes na tabela Numeracao

        private void LimpaNumeracaoBD ()
        {
            SqlConnection con = Util.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Numeracao]", con);
            cmd2.CommandType = CommandType.Text;
            int linhas = cmd2.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Numeracao");
            Util.FechaBD(con);
        }

        //Reinicia o ID da tabela Numeracao

        private void ReseedNumeracaoBD ()
        {
            SqlConnection con = Util.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd2 = new SqlCommand("DBCC CHECKIDENT ('Planeamento.dbo.[CMW$Numeracao]', RESEED, 0)", con);
            cmd2.CommandType = CommandType.Text;
            cmd2.ExecuteNonQuery();
            Util.FechaBD(con);
        }

        //Inicializa a tabela Numeracao com os dados das encomendas pendentes

        private void InicializaNumeracaoBD ()
        {
            SqlConnection con = Util.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Numeracao] " + Util.QuerySalesLine("[Document No_],[Line No_], No_,"), con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas inseridas na tabela Numeracao");
            Util.FechaBD(con);
        }*/
    }
}
