using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class ProducaoCMW2
    {
        private DataTable Produtos;
        private DataTable Plano;

        private readonly int CapacidadeMacharia;
        private readonly int CapacidadeCaixasIMF;
        private readonly int CapacidadeCaixasManual;
        private readonly int CapacidadeFusoes;

        private readonly decimal CapacidadeForno1;
        private readonly decimal CapacidadeForno2;
        private readonly decimal CapacidadeForno3;
        private readonly decimal CapacidadeForno4;

        public ProducaoCMW2()
        {
            Produtos = new DataTable();
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Equipamento",typeof(string)));
            Produtos.Columns.Add(new DataColumn("Liga", typeof(string)));
            Produtos.Columns.Add(new DataColumn("PesoGitos", typeof(decimal)));
            Produtos.Columns.Add(new DataColumn("TempoMachos", typeof(decimal)));
            Produtos.Columns.Add(new DataColumn("Caixas", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Local", typeof(int)));

            Plano = new DataTable();
            Plano.Columns.Add(new DataColumn("Local", typeof(int)));
            Plano.Columns.Add(new DataColumn("Semana", typeof(int)));
            Plano.Columns.Add(new DataColumn("Id", typeof(int)));
            Plano.Columns.Add(new DataColumn("Caixas", typeof(int)));

            int minutosTrabalho = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Horario));
            int homensMacharia = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MachariaCMW2));

            CapacidadeMacharia = minutosTrabalho * homensMacharia * 5;
            CapacidadeCaixasIMF = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.CaixasIMF)) * 5;
            CapacidadeCaixasManual = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.CaixasManual)) * 5;
            CapacidadeFusoes = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.FusoesCMW2)) * 5;

            CapacidadeForno1 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno1CMW2);
            CapacidadeForno2 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno2CMW2);
            CapacidadeForno3 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno3CMW2);
            CapacidadeForno4 = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno4CMW2);
        }

        public void LeituraProdutos()
        {
            String query = "select Id,Equipamento,Liga,[Peso Gitos] as Gitos,TempoMachos,CaixasPendente,Local from " + Util.TabelaProduto + " where Local > 1";
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DataRow row = Produtos.NewRow();

                row["Id"] = Convert.ToInt32(reader["Id"]);
                row["Equipamento"] = (string)reader["Equipamento"];
                row["Liga"] = (string)reader["Liga"];
                row["PesoGitos"] = (decimal)reader["Gitos"];
                row["TempoMachos"] = Convert.ToInt32(reader["TempoMachos"]);
                row["Caixas"] = Convert.ToInt32(reader["CaixasPendente"]);
                row["Local"] = Convert.ToInt32(reader["Local"]);

                Produtos.Rows.Add(row);
            }

            reader.Close();
            con.Close();
        }

        public void ExecutaPlaneamento()
        {
            ExecutaPlaneamentoLocal(2);
            ExecutaPlaneamentoLocal(3);
        }

        private void ExecutaPlaneamentoLocal(int local)
        {
            int semana = 1;
            int caixasAcc = 0;
            int index = 0;
            int CapacidadeCaixas = local == 2 ? CapacidadeCaixasIMF : CapacidadeCaixasManual;

            while (index < Produtos.Rows.Count)
            {
                if (Convert.ToInt32(Produtos.Rows[index]["Local"]) != local)
                {
                    index++;
                    continue;
                }

                int caixasLinha = Convert.ToInt32(Produtos.Rows[index]["Caixas"]);
                int id = Convert.ToInt32(Produtos.Rows[index]["Id"]);

                if (caixasAcc + caixasLinha < CapacidadeCaixas)
                {
                    InsereLinha(id, caixasLinha, semana,local);
                    caixasAcc += caixasLinha;
                    index++;
                }

                else
                {
                    if (caixasAcc < CapacidadeCaixas)
                    {
                        int caixasLivres = CapacidadeCaixas - caixasAcc;
                        InsereLinha(id, caixasLivres, semana,local);
                        Produtos.Rows[index]["Caixas"] = caixasLinha - caixasLivres;
                    }

                    semana++;
                    caixasAcc = 0;
                }
            }
        }

        private void InsereLinha(int id, int caixas, int semana, int local)
        {
            DataRow planoRow = Plano.NewRow();

            planoRow["Id"] = id;
            planoRow["Caixas"] = caixas;
            planoRow["Local"] = local;
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

                cmd = new SqlCommand("UPDATE " + Util.TabelaProduto + " SET CaixasProduzidas = CaixasProduzidas + @Caixas WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Caixas", row["Caixas"]);
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.ExecuteNonQuery();
                linhas++;
            }

            con.Close();

            Console.WriteLine("CMW2: " + linhas + " linhas inseridas na tabela PlanoProducao");

            Plano.Clear();
            Produtos.Clear();
        }
    }
}
