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
        private DataTable Plano;
        private List<GrupoFusao> Grupos;

        private readonly int CapacidadeMacharia;
        private readonly int CapacidadeCaixasIMF;
        private readonly int CapacidadeCaixasManual;
        private readonly int CapacidadeFusoes;

        private readonly decimal CapacidadeForno1;
        private readonly decimal CapacidadeForno2;
        private readonly decimal CapacidadeForno3;
        private readonly decimal CapacidadeForno4;

        private readonly decimal CapacidadeKgsSemanal;

        public ProducaoCMW2()
        {
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

            CapacidadeKgsSemanal = CapacidadeFusoes * (CapacidadeForno1 + CapacidadeForno2 + CapacidadeForno3 + CapacidadeForno4) / 4;
        }

        #region Planeamento

        public void ExecutaPlaneamento()
        {
            CalculaGrupos();

            //foreach (GrupoFusao grupo in Grupos)
            //    Console.WriteLine(grupo.Liga +" ( " + grupo.IdMaisPequeno + "): " + grupo.Peso + "kg // " + grupo.TempoMacharia + " min // " + grupo.Caixas + " caixas");

            int semana = 1;
            while (Grupos.Count > 0)
            {
                PlaneamentoSemana(semana);
                semana++;
            }
        }

        private void PlaneamentoSemana(int semana)
        {
            decimal pesoAcumulado = 0;
            for (int i = Grupos.Count - 1; i >= 0; i--)
            {
                if (pesoAcumulado + Grupos[i].Peso < CapacidadeKgsSemanal)
                {
                    pesoAcumulado += Grupos[i].Peso;
                    AdicionaGrupo(Grupos[i], semana);
                    Grupos.RemoveAt(i);
                }
            }
        }

        private void AdicionaGrupo(GrupoFusao grupo, int semana)
        {
            foreach (LinhaProducao linha in grupo.Lista)
            {
                DataRow planoRow = Plano.NewRow();

                planoRow["Id"] = linha.Id;
                planoRow["Caixas"] = linha.Caixas;
                planoRow["Local"] = linha.Local;
                planoRow["Semana"] = semana;

                Plano.Rows.Add(planoRow);
            }
        }

        #endregion

        #region DivisaoEmGrupos

        private void CalculaGrupos()
        {
            Grupos = new List<GrupoFusao>();

            String query = "select Liga,sum(CaixasPendente*[Peso Gitos])as Peso from " + Util.TabelaProduto + " where Local > 1 and Include = 1 group by Liga";
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string liga = reader["Liga"].ToString();
                decimal peso = (decimal)reader["Peso"];
                DivideEmGrupos(liga, peso);
            }

            reader.Close();
            con.Close();

            Grupos = Grupos.OrderByDescending(grupo => grupo.IdMaisPequeno).ToList(); 
        }

        private void DivideEmGrupos(string liga, decimal peso)
        {
            decimal pesoGrupo;

            if (peso < 2 * ParametrosBD.CalculaMinimoFusao(1))
                pesoGrupo = peso;

            else if (peso < 2 * ParametrosBD.MenorForno(1))
                pesoGrupo = peso / 2;

            else
            {
                int nGrupos = (int)Math.Ceiling(peso / ParametrosBD.MaiorForno(1));
                pesoGrupo = peso / nGrupos;
            }

            decimal pesoAcumulado = 0;
            GrupoFusao grupo = new GrupoFusao(liga);

            String query = "select Id,[Peso Gitos] as Gitos,TempoMachos,CaixasPendente,Local from " + Util.TabelaProduto + " where Local > 1 and Include = 1 and Liga = @Liga order by Id";
            SqlConnection con = Util.AbreBD();
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Liga", liga);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["Id"]);
                decimal pesoGitos = (decimal)reader["Gitos"];
                int tempoMachos = Convert.ToInt32(reader["TempoMachos"]);
                int caixas = Convert.ToInt32(reader["CaixasPendente"]);
                int local = Convert.ToInt32(reader["Local"]);

                while (pesoAcumulado + caixas * pesoGitos > pesoGrupo)
                {
                    int caixasJuntar = (int) Math.Ceiling ((pesoGrupo - pesoAcumulado) / pesoGitos);
                    grupo.AddLinha(id, pesoGitos, tempoMachos, caixasJuntar, local);
                    Grupos.Add(grupo.Clone());
                    caixas -= caixasJuntar;
                    pesoAcumulado = 0;
                    grupo = new GrupoFusao(liga);
                }

                if (caixas > 0)
                {
                    grupo.AddLinha(id, pesoGitos, tempoMachos, caixas, local);
                    pesoAcumulado += caixas * pesoGitos;
                }
            }

            if (grupo.Peso > 0)
                Grupos.Add(grupo);

            reader.Close();
            con.Close();
        }

        #endregion

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
        }
    }
}
