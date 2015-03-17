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
            LimpaProdutosPlanBD();
            LimpaNumeracaoBD();
            ReseedNumeracaoBD();
            InicializaNumeracaoBD();
        }

        //Elimina todos os registos da tabela Produtos Plan

        private void LimpaProdutosPlanBD ()
        {
            SqlConnection con = BDUtil.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Produtos Plan]", con);
            cmd2.CommandType = CommandType.Text;
            int linhas = cmd2.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Produtos Plan");
            BDUtil.FechaBD(con);
        }

        //Elimina todos os registos existentes na tabela Numeracao

        private void LimpaNumeracaoBD ()
        {
            SqlConnection con = BDUtil.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Numeracao]", con);
            cmd2.CommandType = CommandType.Text;
            int linhas = cmd2.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Numeracao");
            BDUtil.FechaBD(con);
        }

        //Reinicia o ID da tabela Numeracao

        private void ReseedNumeracaoBD ()
        {
            SqlConnection con = BDUtil.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd2 = new SqlCommand("DBCC CHECKIDENT ('Planeamento.dbo.[CMW$Numeracao]', RESEED, 0)", con);
            cmd2.CommandType = CommandType.Text;
            cmd2.ExecuteNonQuery();
            BDUtil.FechaBD(con);
        }

        //Inicializa a tabela Numeracao com os dados das encomendas pendentes

        private void InicializaNumeracaoBD ()
        {
            SqlConnection con = BDUtil.AbreBD();
            if (con == null)
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Numeracao] " + BDUtil.QuerySalesLine("[Document No_],[Line No_], No_"), con);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas inseridas na tabela Numeracao");
            BDUtil.FechaBD(con);
        }
    }
}
