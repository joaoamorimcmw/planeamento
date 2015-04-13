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

        private int semana;
        private int dia;
        private int turno;

        /**** Parametros ****/

        private static int TurnosCMW1 = 3;
        private static int TurnosCMW2 = 3;
        private static int FusoesTurno = 2;
        private static decimal Minimo = 0.66M;
        private static decimal CapacidadeCMW1Forno1 = 1000;
        private static decimal CapacidadeCMW1Forno2 = 1000;
        private static decimal CapacidadeCMW1Forno3 = 3000;
        private static decimal CapacidadeCMW1Forno4 = 3000;
        private static decimal CapacidadeCMW2Forno1 = 1100;
        private static decimal CapacidadeCMW2Forno2 = 1100;
        private static decimal CapacidadeCMW2Forno3 = 800;
        private static decimal CapacidadeCMW2Forno4 = 1750;

        /*********************/

        public Fusao()
        {
            FusaoCMW1 = new DataTable("Fusao CMW1");
            FusaoCMW1.Columns.Add(new DataColumn("Id", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("Liga", typeof(string)));
            FusaoCMW1.Columns.Add(new DataColumn("Classe", typeof(string)));
            FusaoCMW1.Columns.Add(new DataColumn("Qtd", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("PesoPeca", typeof(decimal)));
            FusaoCMW1.Columns.Add(new DataColumn("SemanaMoldacao", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("DiaMoldacao", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("TurnoMoldacao", typeof(int)));

            FusaoCMW2 = new DataTable("Fusao CMW2");
            FusaoCMW2.Columns.Add(new DataColumn("Id", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("Liga", typeof(string)));
            FusaoCMW2.Columns.Add(new DataColumn("Classe", typeof(string)));
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

        public void Executa()
        {
            LimpaBDFusao();

            LeituraBD(1);
            Planeamento(1);

            LeituraBD(2);
            Planeamento(2);
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
            String query = "select Prod.Id as Id,Prod.Liga as Liga,Ligas.[Codigo Classe] as Classe,QtdPendente,PesoPeca,SemanaMoldacao,DiaMoldacao,TurnoMoldacao " + 
            "from dbo.PlanCMW$Produtos Prod " + 
            "inner join dbo.PlanCMW$Ligas Ligas " +
            "on Prod.Liga = Ligas.Liga " +
            "where dbo.GetFabrica(Local) = " + Fabrica + " order by Prod.Id asc";

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
                row["Classe"] = reader["Classe"].ToString();
                row["Qtd"] = Convert.ToInt32(reader["QtdPendente"]);
                row["PesoPeca"] = Convert.ToDecimal(reader["PesoPeca"]);
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

        private void Planeamento(int Local)
        {
            ResetGlobais();
            if (Local == 1)
                PlaneamentoLocal (Local,FusaoCMW1,TurnosCMW1,new Dictionary<string,LigaFusao>(),0);
            else
                PlaneamentoLocal (Local,FusaoCMW2,TurnosCMW2,new Dictionary<string,LigaFusao>(),0);
        }

        private void PlaneamentoLocal(int Local,DataTable Table, int nTurnos, Dictionary<String, LigaFusao> Cargas, int Index)
        {
            while (Index < Table.Rows.Count && RespeitaPrecendencia(Table.Rows[Index]))
            {
                DataRow row = Table.Rows[Index];
                string liga = row["Liga"].ToString();
                if (Cargas.ContainsKey(liga))
                    Cargas[liga].AdicionaLinha(row);
                else
                {
                    LigaFusao lFusao = new LigaFusao(liga, row["Classe"].ToString());
                    lFusao.AdicionaLinha(row);
                    Cargas.Add(liga, lFusao);
                }
                Index++;
            }

            PlaneamentoTurno(Local,Cargas);
            bool continua = false;

            if (Index < Table.Rows.Count)
                continua = true;
            else {
                foreach (LigaFusao lFusao in Cargas.Values)
                    if (lFusao.Peso > Fusao.FusaoMinima(Local))
                        continua = true;
            }

            if (continua)
                Util.ProximoTurno(ref turno, ref dia, ref semana, nTurnos);
            //else 
                //Listar produtos que não têm fusão planeada

        }

        private void PlaneamentoTurno(int Local,Dictionary<String,LigaFusao> Cargas){
            //Aqui é que vai acontecer a magia...
        }

        private bool RespeitaPrecendencia(DataRow row)
        {
            int semanaMoldacao = Convert.ToInt32(row["SemanaMoldacao"]);
            int diaMoldacao = Convert.ToInt32(row["DiaMoldacao"]);

            if (semana < semanaMoldacao)
                return false;

            if (semana == semanaMoldacao)
            {
                if (dia < diaMoldacao)
                    return false;
                if (dia == diaMoldacao)
                    return turno > 1;
            }

            return true;
        }

        private void ResetGlobais()
        {
            semana = 1;
            dia = 1;
            turno = 1;
        }

        private decimal PesoProdutos(LinkedList<DataRow> Lista)
        {
            decimal total = 0;

            foreach (DataRow row in Lista)
                total += Convert.ToDecimal(row["Qtd"]) * Convert.ToDecimal(row["PesoPeca"]);

            return total;
        }

        public static decimal FusaoMinima (int Fabrica){
            if (Fabrica == 1)
                return (new[] { CapacidadeCMW1Forno1, CapacidadeCMW1Forno2, CapacidadeCMW1Forno3, CapacidadeCMW1Forno4 }).Min() * Minimo;
            else
                return (new[] { CapacidadeCMW2Forno1, CapacidadeCMW2Forno2, CapacidadeCMW2Forno3, CapacidadeCMW2Forno4 }).Min() * Minimo;
        }
    }
}