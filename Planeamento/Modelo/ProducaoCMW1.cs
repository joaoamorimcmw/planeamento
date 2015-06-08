using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class ProducaoCMW1
    {
        private DataTable Produtos;
        private DataTable Plano;

        private readonly int CapacidadeMacharia;
        private readonly int CapacidadeCaixas;
        private readonly int CapacidadeFusoes;

        private readonly decimal CapacidadeForno1;
        private readonly decimal CapacidadeForno2;
        private readonly decimal CapacidadeForno3;
        private readonly decimal CapacidadeForno4;

        public ProducaoCMW1()
        {
            Produtos = new DataTable();
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Liga", typeof(string)));
            Produtos.Columns.Add(new DataColumn("PesoGitos", typeof(decimal)));
            Produtos.Columns.Add(new DataColumn("TempoMachos", typeof(decimal)));
            Produtos.Columns.Add(new DataColumn("Caixas", typeof(int)));

            Plano = new DataTable();
            Plano.Columns.Add(new DataColumn("Local", typeof(int)));
            Plano.Columns.Add(new DataColumn("Semana", typeof(int)));
            Plano.Columns.Add(new DataColumn("Id", typeof(int)));
            Plano.Columns.Add(new DataColumn("Caixas", typeof(int)));

            int minutosTrabalho = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Horario));
            int homensMacharia = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MachariaCMW1));
            
            CapacidadeMacharia = minutosTrabalho * homensMacharia * 5;
            CapacidadeCaixas = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.CaixasGF)) * 5;
            CapacidadeFusoes = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.FusoesCMW1)) * 5;

            CapacidadeForno1 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno1CMW1);
            CapacidadeForno2 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno2CMW1);
            CapacidadeForno3 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno3CMW1);
            CapacidadeForno4 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno4CMW1);
        }

        public void LeituraProdutos()
        {
            String query = "select Id,Liga,[Peso Gitos] as Gitos,TempoMachos,CaixasPendente from " + Util.TabelaProduto + " where Local = 1";
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DataRow row = Produtos.NewRow();

                row["Id"] = Convert.ToInt32(reader["Id"]);
                row["Liga"] = (string)reader["Liga"];
                row["PesoGitos"] = (decimal)reader["Gitos"];
                row["TempoMachos"] = Convert.ToInt32(reader["TempoMachos"]);
                row["Caixas"] = Convert.ToInt32(reader["CaixasPendente"]);

                Produtos.Rows.Add(row);
            }

            reader.Close();
            con.Close();
        }

        public void ExecutaPlaneamento()
        {
            int semana = 1;
            int caixasAcc = 0;
            int index = 0;

            while (index < Produtos.Rows.Count)
            {
                int caixasLinha = Convert.ToInt32(Produtos.Rows[index]["Caixas"]);
                int id = Convert.ToInt32(Produtos.Rows[index]["Id"]);

                if (caixasAcc + caixasLinha < CapacidadeCaixas)
                {
                    InsereLinha(id, caixasLinha, semana);
                    caixasAcc += caixasLinha;
                    index++;
                }

                else
                {
                    if (caixasAcc < CapacidadeCaixas)
                    {
                        int caixasLivres = CapacidadeCaixas - caixasAcc;
                        InsereLinha(id, caixasLivres, semana);
                        Produtos.Rows[index]["Caixas"] = caixasLinha - caixasLivres;
                    }

                    semana++;
                    caixasAcc = 0;
                }
            }
        }

        private void InsereLinha(int id, int caixas, int semana)
        {
            DataRow planoRow = Plano.NewRow();

            planoRow["Id"] = id;
            planoRow["Caixas"] = caixas;
            planoRow["Local"] = 1;
            planoRow["Semana"] = semana;

            Plano.Rows.Add(planoRow);
        }

        public void EscritaBD()
        {
            int linhas = 0;
            SqlConnection con = Util.AbreBD();
            
            foreach (DataRow row in Plano.Rows)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO " + Util.TabelaPlano + " VALUES(@Local,@Semana,@Id,@Caixas)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Local", row["Local"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Caixas", row["Caixas"]);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE " + Util.TabelaProduto + " SET CaixasPendente = CaixasPendente - @Caixas WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Caixas", row["Caixas"]);
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.ExecuteNonQuery();
                linhas++;
            }

            con.Close();

            Console.WriteLine("CMW1: " + linhas + " linhas inseridas na tabela PlanoProducao");

            Plano.Clear();
            Produtos.Clear();
        }

    }
}
