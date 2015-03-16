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
        //variaveis
        private DataTable batchCMW1;
        private DataTable batchCMW2;
        private DataTable planoCMW1;
        private DataTable planoCMW2;

        int capacidadeCMW1;
        int capacidadeCMW2;
        int horario;
        int acc;
        int dia;
        int semana;
        int index;


        //construtores
        public PlanMacharia() {
            capacidadeCMW1 = getCapacidadeCMW1();
            capacidadeCMW2 = getCapacidadeCMW2(); 
            acc = 0;
            dia = 1;
            horario = 8;
            semana = 1;
            index = 0;
        }

        public PlanMacharia(DataTable planoCMW1, DataTable planoCMW2)
        {

            //apagar os planos da macharia recebidos de BDMacharia
            batchCMW1 = planoCMW1.Copy();
            batchCMW2 = planoCMW2.Copy();
            planoCMW1.Clear();
            planoCMW2.Clear();

            //verifica as capacidades das macharias através da consulta na tabela Parametros
            //inicializada os DataTables planoCMW1 e planoCMW2
            inicializa();
            //por cada linha da lista de machos, insere no planeamento da macharia planoCMW1 e planoCMW2
            processa();
            
            //imprimePlano();

        }

        //metodos


        private void inicializa() {

            capacidadeCMW1 = getCapacidadeCMW1();
            capacidadeCMW2 = getCapacidadeCMW2();

            acc = 0;
            dia = 1;
            horario = 8*60; // minutos
            semana = 1;
            index = 0;


            planoCMW1 = new DataTable("Plano CMW1");
            planoCMW1.Columns.Add(new DataColumn("linha", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("local", typeof(String)));
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
            planoCMW1.Columns.Add(new DataColumn("fabrica", typeof(String)));

            planoCMW2 = new DataTable("Plano CMW2");
            planoCMW2.Columns.Add(new DataColumn("linha", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("local", typeof(String)));
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
            planoCMW2.Columns.Add(new DataColumn("fabrica", typeof(String)));


        }

        private int getCapacidadeCMW1() {
            int res=0;
            SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
            SqlCommand cmd = new SqlCommand("SELECT [Capacidade Macharia CMW1] FROM Planeamento.dbo.[CMW$Parametros]", con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            res = (int) cmd.ExecuteScalar();
            con.Close();
            return res;
        }

        private int getCapacidadeCMW2()
        {
            int res = 0;
            SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
            SqlCommand cmd = new SqlCommand("SELECT [Capacidade Macharia CMW2] FROM Planeamento.dbo.[CMW$Parametros]", con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            res = (int) cmd.ExecuteScalar();
            con.Close();
            return res;
        }


        private void setDay() {

            if (dia == 1)
                dia = 2;
            else if (dia == 2)
                dia = 3;
            else if (dia == 3)
                dia = 4;
            else if (dia == 4)
                dia = 5;
            else if (dia == 5)
            {
                dia = 1;
                semana = semana + 1;
            }
        
        
        }


        private void splitRowCMW1(DataRow row)
        {
            int espaço = (horario * capacidadeCMW1) - acc;
            decimal razao = Convert.ToDecimal(row[5]) / Convert.ToDecimal(row[4]);
         
            DataRow dr = planoCMW1.NewRow();
            index += 1;
            dr["linha"] = index;
            dr["local"] = Convert.ToInt32(row[0]);
            dr["semana"] = semana;
            dr["dia"] = dia;
            dr["noDoc"] = row[1].ToString();
            dr["noLine"] = row[2];
            dr["noProd"] = row[3];
            int quantidadeNova = Convert.ToInt32((Convert.ToDecimal(row[4]) * espaço) / Convert.ToDecimal(row[5]));
            dr["qtd"] = quantidadeNova;
            int tempoNovo = Convert.ToInt32(quantidadeNova * razao);
            dr["tempo"] = tempoNovo;
            dr["acc"] = acc + tempoNovo;
            dr["codMach"] = row[6].ToString();
            dr["noLiga"] = row[7].ToString();
            dr["fabrica"] = 1;
            planoCMW1.Rows.InsertAt(dr, index);
            acc = 0;
            row["qtd"] = Convert.ToInt32(row[4]) - quantidadeNova;
            row["tempo"] = Convert.ToInt32(row[5]) - tempoNovo;

            setDay();

            inserePlaneamento(row,1);

        }

        private void splitRowCMW2(DataRow row)
        {
            int espaço = (horario * capacidadeCMW2) - acc;
            decimal razao = Convert.ToDecimal(row[5]) / Convert.ToDecimal(row[4]);

            DataRow dr = planoCMW2.NewRow();
            index += 1;
            dr["linha"] = index;
            dr["local"] = Convert.ToInt32(row[0]);
            dr["semana"] = semana;
            dr["dia"] = dia;
            dr["noDoc"] = row[1].ToString();
            dr["noLine"] = row[2];
            dr["noProd"] = row[3];
            int quantidadeNova = Convert.ToInt32((Convert.ToDecimal(row[4]) * espaço) / Convert.ToDecimal(row[5]));
            dr["qtd"] = quantidadeNova;
            int tempoNovo = Convert.ToInt32(quantidadeNova * razao);
            dr["tempo"] = tempoNovo;
            dr["acc"] = acc + tempoNovo;
            dr["codMach"] = row[6].ToString();
            dr["noLiga"] = row[7].ToString();
            dr["fabrica"] = 2;
            planoCMW2.Rows.InsertAt(dr, index);
            acc = 0;
            row["qtd"] = Convert.ToInt32(row[4]) - quantidadeNova;
            row["tempo"] = Convert.ToInt32(row[5]) - tempoNovo;
            setDay();

            inserePlaneamento(row,2);

        }

        



        private void inserePlaneamento(DataRow row, int local)
        {
            if (local == 1)
            {

                if (acc + Convert.ToInt32(row[5]) > horario * capacidadeCMW1) splitRowCMW1(row);
                else if (Convert.ToInt32(row[4]) > 0 && Convert.ToInt32(row[5]) > 0)
                {
                    index = index + 1;
                    acc = acc + Convert.ToInt32(row[5]);
                    DataRow dr = planoCMW1.NewRow();
                    dr["linha"] = index;
                    dr["semana"] = semana;
                    dr["dia"] = dia;
                    dr["noDoc"] = row[1].ToString();
                    dr["noLine"] = row[2];
                    dr["noProd"] = row[3];
                    dr["qtd"] = Convert.ToInt32(row[4]);
                    dr["tempo"] = Convert.ToInt32(row[5]);
                    dr["codMach"] = row[6].ToString();
                    dr["local"] = Convert.ToInt32(row[0]);
                    dr["noLiga"] = row[7].ToString();
                    dr["acc"] = acc;
                    dr["fabrica"] = 1;

                    planoCMW1.Rows.Add(dr);


                }

            }
            else
            {

                if (acc + Convert.ToInt32(row[5]) > horario * capacidadeCMW2) splitRowCMW2(row);
                else if (Convert.ToInt32(row[4]) > 0 && Convert.ToInt32(row[5]) > 0)
                {
                    index = index + 1;
                    acc = acc + Convert.ToInt32(row[5]);
                    DataRow dr = planoCMW2.NewRow();
                    dr["linha"] = index;
                    dr["semana"] = semana;
                    dr["dia"] = dia;
                    dr["noDoc"] = row[1].ToString();
                    dr["noLine"] = row[2];
                    dr["noProd"] = row[3];
                    dr["qtd"] = Convert.ToInt32(row[4]);
                    dr["tempo"] = Convert.ToInt32(row[5]);
                    dr["codMach"] = row[6].ToString();
                    dr["local"] = Convert.ToInt32(row[0]);
                    dr["noLiga"] = row[7].ToString();
                    dr["acc"] = acc;
                    dr["fabrica"] = 2;
                    planoCMW2.Rows.Add(dr);

                }
            }
        }




        private void processa() {

            foreach (DataRow row in batchCMW1.Rows)
                inserePlaneamento(row,1);
            //actualizar globais
            acc = 0;
            dia = 1;
            semana = 1;
            index = 0;

            foreach (DataRow row in batchCMW2.Rows)
                inserePlaneamento(row,2);


        }


        public DataTable getPlanoCMW1() { 
            return planoCMW1;
        }

        public DataTable getPlanoCMW2()
        {
            return planoCMW2;
        }




        
        private void imprimePlano() {


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
