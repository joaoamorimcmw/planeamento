using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;


namespace Planeamento
{
    class PlanMacharia
    {
        private DataTable batchCMW1;
        private DataTable batchCMW2;
        private DataTable planoCMW1;
        private DataTable planoCMW2;

        int capacidadeCMW1;
        int capacidadeCMW2;
        int horario = 8 * 60;
        int acc;
        int dia;
        int semana;
        int index;


        public PlanMacharia(DataTable bdMacCMW1, DataTable bdMacCMW2)
        {
            batchCMW1 = bdMacCMW1.Copy();
            batchCMW2 = bdMacCMW2.Copy();
            //bdMacCMW1.Clear(); Provavelmente inútil, só mesmo para libertar espaço
            //bdMacCMW2.Clear();

            Inicializa();
            Processa();
            //ImprimePlano();
        }

        //Inicializa os parametros, variáveis e as DataTables planoCMW1 e 2
        private void Inicializa() {

            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;

            capacidadeCMW1 = GetCapacidadeCMW1(connection);
            capacidadeCMW2 = GetCapacidadeCMW2(connection);

            BDUtil.FechaBD(connection);

            ResetGlobais();

            planoCMW1 = new DataTable("Plano CMW1");
            planoCMW1.Columns.Add(new DataColumn("linha", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("local", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("semana", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("dia", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("noDoc", typeof(String)));
            planoCMW1.Columns.Add(new DataColumn("noLine", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("noProd", typeof(String)));
            planoCMW1.Columns.Add(new DataColumn("codMach", typeof(String)));
            planoCMW1.Columns.Add(new DataColumn("noLiga", typeof(String)));
            planoCMW1.Columns.Add(new DataColumn("qtd", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("tempo", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("acc", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("fabrica", typeof(int)));

            planoCMW2 = new DataTable("Plano CMW2");
            planoCMW2.Columns.Add(new DataColumn("linha", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("local", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("semana", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("dia", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("noDoc", typeof(String)));
            planoCMW2.Columns.Add(new DataColumn("noLine", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("noProd", typeof(String)));
            planoCMW2.Columns.Add(new DataColumn("codMach", typeof(String)));
            planoCMW2.Columns.Add(new DataColumn("noLiga", typeof(String)));
            planoCMW2.Columns.Add(new DataColumn("qtd", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("tempo", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("acc", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("fabrica", typeof(int)));
        }

        private void ResetGlobais()
        {
            acc = 0;
            dia = 1;
            semana = 1;
            index = 0;
        }

        //Retira parametro Capacidade Macharia CMW1 da tabela Parametros
        private int GetCapacidadeCMW1(SqlConnection connection)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Macharia CMW1'", connection);
            cmd.CommandType = CommandType.Text;
            res = (int) cmd.ExecuteScalar();

            return res;
        }

        //Retira parametro Capacidade Macharia CMW2 da tabela Parametros
        private int GetCapacidadeCMW2(SqlConnection connection)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Macharia CMW2'", connection);
            cmd.CommandType = CommandType.Text;
            res = (int) cmd.ExecuteScalar();

            return res;
        }

        private void Processa()
        {
            foreach (DataRow row in batchCMW1.Rows)
                InserePlaneamento(row, 1);

            ResetGlobais();



            foreach (DataRow row in batchCMW2.Rows)
                InserePlaneamento(row, 2);

        }
        
        //Por cada linha, se houver capacidade restante no dia, insere a linha completa, se não separa e insere no dia seguinte
        private void InserePlaneamento(DataRow row, int fabrica)
        {
            int capacidade;
            if (fabrica == 1)
                capacidade = capacidadeCMW1;
            else
                capacidade = capacidadeCMW2;

            if (acc + Convert.ToInt32(row[5]) > horario * capacidade) 
                SeparaLinha(row,fabrica);
            
            else if (Convert.ToInt32(row[4]) > 0 && Convert.ToInt32(row[5]) > 0)
            {
                index += 1;
                acc = acc + Convert.ToInt32(row[5]);
                InsereLinha(fabrica,Convert.ToInt32(row[0]),row[1].ToString(),Convert.ToInt32(row[2]),row[3].ToString(),row[6].ToString(),row[7].ToString(),Convert.ToInt32(row[4]),Convert.ToInt32(row[5]),acc);
            }

        }

        private void SeparaLinha(DataRow row, int fabrica)
        {
            int espaço;
            if (fabrica == 1)
                espaço = (horario * capacidadeCMW1) - acc;
            else
                espaço = (horario * capacidadeCMW2) - acc;

            decimal razao = Convert.ToDecimal(row[5]) / Convert.ToDecimal(row[4]);
            int quantidadeNova = Convert.ToInt32((Convert.ToDecimal(row[4]) * espaço) / Convert.ToDecimal(row[5]));
            int tempoNovo = Convert.ToInt32(quantidadeNova * razao);
            int accNovo = acc + tempoNovo;
            index += 1;

            InsereLinha(fabrica, Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToInt32(row[2]), row[3].ToString(), row[6].ToString(), row[7].ToString(), quantidadeNova, tempoNovo, accNovo, true);

            acc = 0;

            row["qtd"] = Convert.ToInt32(row[4]) - quantidadeNova;
            row["tempo"] = Convert.ToInt32(row[5]) - tempoNovo;

            ProximoDia();

            InserePlaneamento(row, fabrica);
        }

        private void InsereLinha(int fabrica, int local, String noDoc, int noLine, String noProd, String codMach, String noLiga, int qtd, int tempo, int acc, bool insertAtIndex = false)
        {
            DataRow dr;
            if (fabrica == 1)
                dr = planoCMW1.NewRow();
            else
                dr = planoCMW2.NewRow();

            dr["linha"] = index;
            dr["local"] = local;
            dr["semana"] = semana;
            dr["dia"] = dia;
            dr["noDoc"] = noDoc;
            dr["noLine"] = noLine;
            dr["noProd"] = noProd;
            dr["codMach"] = codMach;
            dr["noLiga"] = noLiga;
            dr["qtd"] = qtd;
            dr["tempo"] = tempo;
            dr["acc"] = acc;
            dr["fabrica"] = fabrica;

            if (!insertAtIndex)
            {
                if (fabrica == 1)
                    planoCMW1.Rows.Add(dr);
                else
                    planoCMW2.Rows.Add(dr);
            }
            else
            {
                if (fabrica == 1)
                    planoCMW1.Rows.InsertAt(dr, index);
                else
                    planoCMW2.Rows.InsertAt(dr, index);
            }

        }

        private void ProximoDia()
        {
            if (dia == 5)
            {
                dia = 1;
                semana = semana + 1;
            }
            else
                dia++;
        }


        public DataTable GetPlanoCMW1() { 
            return planoCMW1;
        }

        public DataTable GetPlanoCMW2()
        {
            return planoCMW2;
        }
        
        private void ImprimePlano() {

            Console.WriteLine("---- Planeamento CMW1 ----");

            foreach (DataRow row in planoCMW1.Rows)
            {
                foreach (DataColumn column in planoCMW1.Columns)
                    Console.Write(column.ToString() + " "+ row[column] + "|");
                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("---- Planeamento CMW2 ----");

            foreach (DataRow row in planoCMW2.Rows)
            {
                foreach (DataColumn column in planoCMW2.Columns)
                    Console.Write(column.ToString() + " " + row[column] + "|");
                Console.WriteLine(" ");

            }
        }
    }


}
