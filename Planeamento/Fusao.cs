using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    class Fusao
    {
        private DataTable FusaoCMW1;
        private DataTable FusaoCMW2;
        private DataTable PlanoCMW1;
        private DataTable PlanoCMW2;
        private DataTable Produtos;

        public Fusao () {
            FusaoCMW1 = new DataTable("Fusao CMW1");
            FusaoCMW1.Columns.Add(new DataColumn("Id", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("Liga", typeof(string)));
            FusaoCMW1.Columns.Add(new DataColumn("Qtd", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("PesoPeca", typeof(decimal)));
            FusaoCMW1.Columns.Add(new DataColumn("SemanaMoldacao", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("DiaMoldacao", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("TurnoMoldacao", typeof(int)));

            FusaoCMW2 = new DataTable("Fusao CMW2");
            FusaoCMW2.Columns.Add(new DataColumn("Id", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("Liga", typeof(string)));
            FusaoCMW2.Columns.Add(new DataColumn("Qtd", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("PesoPeca", typeof(decimal)));
            FusaoCMW2.Columns.Add(new DataColumn("SemanaMoldacao", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("DiaMoldacao", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("TurnoMoldacao", typeof(int)));

            PlanoCMW1 = new DataTable("Plano CMW1");
            PlanoCMW1.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Fabrica", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Forno", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("NoFusao", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("PesoTotal", typeof(decimal)));

            PlanoCMW2 = new DataTable("Plano CMW2"); 
            PlanoCMW2.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Fabrica", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Forno", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("NoFusao", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("PesoTotal", typeof(decimal)));

            Produtos = new DataTable("Produtos");
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Semana", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Dia", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Turno", typeof(int)));

        }

        public void Executa (){
            LimpaBDFusao();

            LeituraBD(1);
            LeituraBD(2);

            TotalPesos();
        }

        private void LimpaBDFusao()
        {
            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Fusao]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Fusao");
            connection.Close();
        }

        private void LeituraBD(int Fabrica)
        {
            String query = "select Id,Liga,QtdPendente,PesoPeca,SemanaMoldacao,DiaMoldacao,TurnoMoldacao from dbo.PlanCMW$Produtos where dbo.GetFabrica(Local) = " + Fabrica + " order by Id asc";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DataRow row;

                if (Fabrica == 1)
                    row = FusaoCMW1.NewRow();
                else
                    row = FusaoCMW2.NewRow();

                row["Id"] = Convert.ToInt32(reader["Id"]);
                row["Liga"] = reader["Liga"].ToString();
                row["Qtd"] = Convert.ToInt32(reader["QtdPendente"]);
                row["PesoPeca"] =  Convert.ToDecimal(reader["PesoPeca"]);
                row["SemanaMoldacao"] = Convert.ToInt32(reader["SemanaMoldacao"]);
                row["DiaMoldacao"] = Convert.ToInt32(reader["DiaMoldacao"]);
                row["TurnoMoldacao"] = Convert.ToInt32(reader["TurnoMoldacao"]);

                if (Fabrica == 1)
                    FusaoCMW1.Rows.Add(row);
                else
                    FusaoCMW2.Rows.Add(row);
            }

            reader.Close();
            connection.Close();
        }

        private void TotalPesos()
        {
            Dictionary<String, Decimal> PesosCMW1 = new Dictionary<String, Decimal>();
            Dictionary<String, Decimal> PesosCMW2 = new Dictionary<String, Decimal>();
            
            foreach (DataRow row in FusaoCMW1.Rows) {
                String liga = row["Liga"].ToString();
                decimal peso = Convert.ToDecimal(row["PesoPeca"]) * Convert.ToInt32(row["Qtd"]);

                if (PesosCMW1.ContainsKey(liga))
                    PesosCMW1[liga] += peso;
                else
                    PesosCMW1.Add(liga, peso);
            }

            foreach (DataRow row in FusaoCMW2.Rows)
            {
                String liga = row["Liga"].ToString();
                decimal peso = Convert.ToDecimal(row["PesoPeca"]) * Convert.ToInt32(row["Qtd"]);

                if (PesosCMW2.ContainsKey(liga))
                    PesosCMW2[liga] += peso;
                else
                    PesosCMW2.Add(liga, peso);
            }
            
            Console.WriteLine("Pesos CMW1");
            Console.WriteLine("----------------");

            foreach (String liga in PesosCMW1.Keys)
                Console.WriteLine(liga + ": " + PesosCMW1[liga] + " kgs");

            Console.WriteLine("\n\nPesos CMW2");
            Console.WriteLine("----------------");

            foreach (String liga in PesosCMW2.Keys)
                Console.WriteLine(liga + ": " + PesosCMW2[liga] + " kgs");
        } 
    }
}
