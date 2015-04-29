using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class Rebarbagem
    {
        private DataTable Produtos;
        private DataTable Plano;

        private int semana;
        private int dia;
        private int turno;

        private int nTurnos;

        public int NTurnos
        {
            get { return nTurnos; }
            set { nTurnos = value; }
        }

        private int funcionariosTurno;

        public int FuncionariosTurno
        {
            get { return funcionariosTurno; }
            set { funcionariosTurno = value; }
        }

        private Dictionary<String, Int32> nPostos;

        private Dictionary<String, Int32> NPostos
        {
            get { return nPostos; }
            set { nPostos = value; }
        }

        private Dictionary<String, String> postosAssociados;

        public Dictionary<String, String> PostosAssociados
        {
            get { return postosAssociados; }
            set { postosAssociados = value; }
        }

        public Rebarbagem()
        {
            Produtos = new DataTable("Produtos");
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Ordem", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Posto", typeof(string)));
            Produtos.Columns.Add(new DataColumn("TempoExecucao", typeof(int)));

            Plano = new DataTable("Plano");
            Plano.Columns.Add(new DataColumn("Id", typeof(int)));
            Plano.Columns.Add(new DataColumn("Semana",typeof(int)));
            Plano.Columns.Add(new DataColumn("Dia",typeof(int)));
            Plano.Columns.Add(new DataColumn("Turno",typeof(int)));
            Plano.Columns.Add(new DataColumn("Posto", typeof(string)));
            Plano.Columns.Add(new DataColumn("QtdPecas", typeof(int)));
            Plano.Columns.Add(new DataColumn("Tempo", typeof(decimal)));

            nPostos = new Dictionary<string, int>();
            postosAssociados = new Dictionary<string, string>();

            String query = "select CodCentroMaquina,CodPosto,QtdPosto from PlanCMW$PostosRebarbagem";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string centro = reader["CodCentroMaquina"].ToString();
                string posto = reader["CodPosto"].ToString();
                int qtd = Convert.ToInt32(reader["QtdPosto"]);

                postosAssociados.Add(centro, posto);
                if (!nPostos.Keys.Contains(posto))
                    nPostos.Add(posto, qtd);
            }

            reader.Close();
            connection.Close();
        }

        public void LimpaBDRebarbagem()
        {
            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Rebarbagem]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Rebarbagem");

            connection.Close();
        }

        public void LeituraBD()
        {
            string query = "select Prod.Id, Prod.QtdVazadas,Rout.[No_],Rout.[Run Time] from " +
            "PlanCMW$Produtos Prod " +
            "left join " +
            "(select * from Navision.dbo.[CMW$Routing Line] where No_ in (select CodCentroMaquina from PlanCMW$PostosRebarbagem)) Rout " +
            "on Prod.NoProd = Rout.[Routing No_] " +
            "order by Id";

            //Tratar qtdsvazadas = 0, e Rout.No_ = null
        }

        public void Executa(double tsri, double tss, double nom, double ettu, double grats)
        {
            double dpsdds = ettu * (1 - tsri - tss);

            double styey = nom * Math.Max(grats + dpsdds - 505.0, 0);
            double wkm = dpsdds - styey + grats;
            double stnoo = nom * Math.Max(dpsdds - 505.0, 0);
            double subsub = (dpsdds - stnoo) * 5/12;
            Console.WriteLine("maymay: {0}\nsts: {0}\nbeach: {1}\nsanta: {1}\ngib: {2}", wkm, subsub, 2 * wkm + 2 * subsub);
        }

        public void EscreveBD()
        {

        }

        public void LimpaTabelas()
        {
            Produtos.Clear();
            Plano.Clear();
        }
    }
}
