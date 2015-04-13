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

    class PlanFusao
    {
        DataTable cmw1; // aqui tem o que recebe da B.D
        DataTable cmw2; // aqui tem o que recebe da B.D
        DataTable planoCMW1;
        DataTable planoCMW2;
        int cforno1;
        int cforno2;
        int cforno3;
        int cforno4;
        int accforno1;
        int accforno2;
        int accforno3;
        int accforno4;
        int cm;
        int mudDia;
        int numfus1;
        int numfus2;
        int numfus3;
        int numfus4;
        //int conFus;     //contador diario de fusões



        // ************************* CONSTRUTORES ***********************************

        public PlanFusao(DataTable pCMW1, DataTable pCMW2)
        {
            cmw1 = pCMW1.Copy();
            cmw2 = pCMW2.Copy();

            pCMW1.Clear();
            pCMW2.Clear();

            putCapacidadesCMW1();
            init();

            //imprimePlano();
        
        }

        private void putCapacidadesCMW2() {
            cm = 2;
            cforno1 = 1020;
            cforno2 = 453;
            cforno3 = 590;
            cforno4 = 500;
            accforno1 = 0;
            accforno2 = 0;
            accforno3 = 0;
            accforno4 = 0;


        
        }

        private void putCapacidadesCMW1(){
            //conFus = 1;
            mudDia = 1;
            cm = 1;
            cforno1 = 500;
            cforno2 = 500;
            cforno3 = 1700;
            cforno4 = 1700;
            accforno1 = 0;
            accforno2 = 0;
            accforno3 = 0;
            accforno4 = 0;


            numfus1=1;
            numfus2=1;
            numfus3=1;
            numfus4=1;
 


            planoCMW1 = new DataTable("Plano CMW1");
            planoCMW1.Columns.Add(new DataColumn("semana", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("dia", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("liga", typeof(string)));
            planoCMW1.Columns.Add(new DataColumn("forno", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("cargaACC", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("carga", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("linhaPlanMoldacao", typeof(int)));
            planoCMW1.Columns.Add(new DataColumn("numfus", typeof(int)));




            planoCMW2 = new DataTable("Plano CMW2");
            planoCMW2.Columns.Add(new DataColumn("semana", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("dia", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("liga", typeof(string)));
            planoCMW2.Columns.Add(new DataColumn("forno", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("cargaACC", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("carga", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("linhaPlanMoldacao", typeof(int)));
            planoCMW2.Columns.Add(new DataColumn("numfus", typeof(int)));


        }



        private void init()
        {
            foreach (DataRow row in cmw1.Rows)
                plan(row);

            putCapacidadesCMW2();

            foreach (DataRow row in cmw2.Rows)
                plan(row);

        }

        private int escolheForno() {

            if (accforno1 < cforno1) { return 1; }
            else if (accforno2 < cforno2) { return 2; }
            else if (accforno3 < cforno3) { return 3; }
            else if (accforno4 < cforno4) { return 4; }
            else
            {
                accforno1 = 0;
                accforno2 = 0;
                accforno3 = 0;
                accforno4 = 0;
            }

            return 1;
        }


        
        private void adicionaPlanTotal(DataRow row,int pecas, int forno)
        {
            if (pecas == (int)Convert.ToInt16(row[4]))          //aqui é total
            {
                
                if(cm==1){
                    DataRow dr=planoCMW1.NewRow();
                    dr[0] = row[0];
                    dr[1] = row[1];
                    dr[2] = row[2];
                    dr[3] = forno;
                        if (forno == 1) dr[4] = accforno1;
                        else if (forno == 2) dr[4] = accforno2;
                        else if (forno == 3) dr[4] = accforno3;
                        else if (forno == 4) dr[4] = accforno4;
                    dr[5] = (int)Convert.ToInt16(row[3]);
                    dr[6] = (int)Convert.ToInt16(row[6]);

                    if (forno == 1) dr[7] = numfus1;
                    else if (forno == 2) dr[7] = numfus2;
                    else if (forno == 3) dr[7] = numfus3;
                    else if (forno == 4) dr[7] = numfus4;
                   
                    planoCMW1.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = planoCMW2.NewRow();
                    dr[0] = row[0];
                    dr[1] = row[1];
                    dr[2] = row[2];
                    dr[3] = forno;
                        if (forno == 1) dr[4] = accforno1;
                        else if (forno == 2) dr[4] = accforno2;
                        else if (forno == 3) dr[4] = accforno3;
                        else if (forno == 4) dr[4] = accforno4;
                    dr[5] = (int)Convert.ToInt16(row[3]);
                    dr[6] = (int)Convert.ToInt16(row[6]);

                    if (forno == 1) dr[7] = numfus1;
                    else if (forno == 2) dr[7] = numfus2;
                    else if (forno == 3) dr[7] = numfus3;
                    else if (forno == 4) dr[7] = numfus4;

                    planoCMW2.Rows.Add(dr);

                }
            }
            else {                                              //aqui é parcial e esgota a capacidade do forno

             

                if (cm == 1)
                {
                    DataRow dr = planoCMW1.NewRow();
                    dr[0] = row[0];
                    dr[1] = row[1];
                    dr[2] = row[2];
                    dr[3] = forno;
                    if (forno == 1) dr[4] = accforno1 + pecas * (int)Convert.ToInt16(row[5]);
                    else if (forno == 2) dr[4] = accforno2 + pecas * (int)Convert.ToInt16(row[5]);
                    else if (forno == 3) dr[4] = accforno3 + pecas * (int)Convert.ToInt16(row[5]);
                    else if (forno == 4) dr[4] = accforno4 + pecas * (int)Convert.ToInt16(row[5]);
                    dr[5] = pecas * (int)Convert.ToInt16(row[5]);
                    dr[6] = (int)Convert.ToInt16(row[6]);

                    if (forno == 1) dr[7] = numfus1;
                    else if (forno == 2) dr[7] = numfus2;
                    else if (forno == 3) dr[7] = numfus3;
                    else if (forno == 4) dr[7] = numfus4;

                    planoCMW1.Rows.Add(dr);



                    if (forno == 1) accforno1 = cforno1;
                    else if (forno == 2) accforno2 = cforno2;
                    else if (forno == 3) accforno3 = cforno3;
                    else if (forno == 4) accforno4 = cforno4;

                    if (forno == 1) numfus1+=1;
                    else if (forno == 2) numfus2 += 1;
                    else if (forno == 3) numfus3 += 1;
                    else if (forno == 4) numfus4 += 1;

                }
                else {
                    DataRow dr = planoCMW2.NewRow();
                    dr[0] = row[0];
                    dr[1] = row[1];
                    dr[2] = row[2];
                    dr[3] = forno;
                    if (forno == 1) dr[4] = accforno1 + pecas * (int)Convert.ToInt16(row[5]);
                    else if (forno == 2) dr[4] = accforno2 + pecas * (int)Convert.ToInt16(row[5]);
                    else if (forno == 3) dr[4] = accforno3 + pecas * (int)Convert.ToInt16(row[5]);
                    else if (forno == 4) dr[4] = accforno4 + pecas * (int)Convert.ToInt16(row[5]);
                    dr[5] = pecas * (int)Convert.ToInt16(row[5]);
                    dr[6] = (int)Convert.ToInt16(row[6]);

                    if (forno == 1) dr[7] = numfus1;
                    else if (forno == 2) dr[7] = numfus2;
                    else if (forno == 3) dr[7] = numfus3;
                    else if (forno == 4) dr[7] = numfus4;

                    planoCMW2.Rows.Add(dr);
                    if (forno == 1) accforno1 = cforno1;
                    else if (forno == 2) accforno2 = cforno2;
                    else if (forno == 3) accforno3 = cforno3;
                    else if (forno == 4) accforno4 = cforno4;

                    if (forno == 1) numfus1 += 1;
                    else if (forno == 2) numfus2 += 1;
                    else if (forno == 3) numfus3 += 1;
                    else if (forno == 4) numfus4 += 1;

             
                }
                
            }

           



            row[4] = (int)Convert.ToInt16(row[4]) - pecas;
            row[3] = (int)Convert.ToInt16(row[4]) * (int)Convert.ToInt16(row[5]);

            insereForno(row,escolheForno());

        }





        private int valorPeca(DataRow row, int forno) {
            int res = 0;
            int dif=-1;
            int peso = (int)Convert.ToInt16(row[5]);
            int totalP = (int)Convert.ToInt16(row[4]);

            if (forno == 1)
              dif = cforno1 - accforno1;
            else if (forno == 2)
              dif = cforno2 - accforno2;
            else if (forno == 3)
              dif = cforno3 - accforno3;
            else if (forno == 4)
              dif = cforno4 - accforno4;
               
            while ((dif - peso) >= 0 & res <= totalP)
            {
                dif -= peso;
                res++;
            }


            return res;

        }

        private void insereForno(DataRow row,int forno) {


            

            if ((int)Convert.ToInt16(row[4]) > 0)
            {

                if (forno == 1)
                {
                    if (((int)Convert.ToInt16(row[3].ToString()) + accforno1) <= cforno1)
                    {
                                                                      //tem carga total para a linha
                        accforno1 += (int)Convert.ToInt16(row[3]);
                        adicionaPlanTotal(row, (int)Convert.ToInt16(row[4]), 1);

                    }
                    else if (accforno1 < cforno1)
                    {
                        //tem carga parcial para a linha
                        int npecas = valorPeca(row,1);
                        adicionaPlanTotal(row,npecas,1);
                    }

                }
                else if (forno == 2) {

                    if (((int)Convert.ToInt16(row[3].ToString()) + accforno2) <= cforno2)
                    {

                        //tem carga total para a linha
                        accforno2 += (int)Convert.ToInt16(row[3]);
                        adicionaPlanTotal(row, (int)Convert.ToInt16(row[4]), 2);
                    }
                    else if (accforno2 < cforno2)
                    {
                        //tem carga parcial para a linha
                        int npecas = valorPeca(row, 2);
                        adicionaPlanTotal(row, npecas, 2);
                    }

                }
                else if (forno == 3)
                {

                    if (((int)Convert.ToInt16(row[3].ToString()) + accforno3) <= cforno3)
                    {

                        //tem carga total para a linha
                        accforno3 += (int)Convert.ToInt16(row[3]);
                        adicionaPlanTotal(row, (int)Convert.ToInt16(row[4]), 3);
                    }
                    else if (accforno3 < cforno3)
                    {

                        //tem carga parcial para a linha
                        int npecas = valorPeca(row, 3);
                        adicionaPlanTotal(row, npecas, 3);
                    }
                }
                else if (forno == 4)
                {

                    if (((int)Convert.ToInt16(row[3].ToString()) + accforno4) <= cforno4)
                    {

                        //tem carga total para a linha
                        accforno4 += (int)Convert.ToInt16(row[3]);
                        adicionaPlanTotal(row, (int)Convert.ToInt16(row[4]), 4);
                    }
                    else if (accforno4 < cforno4)
                    {
                        //tem carga parcial para a linha
                        int npecas = valorPeca(row, 4);
                        adicionaPlanTotal(row, npecas, 4);
                    }
                }

            }
        }



        private void verificaDia(DataRow row)
        {

            if (Convert.ToInt32(row[1].ToString()) != mudDia) {

                numfus1=1;
                numfus2=1;
                numfus3=1;
                numfus4=1;
            
            }
        }


        private void plan(DataRow row)
        {
            verificaDia(row);
            insereForno(row,escolheForno());
                
        }



        public DataTable getPlanoCMW1()
        {
            return planoCMW1;
        }


        public DataTable getPlanoCMW2()
        {
            return planoCMW2;
        }



        public void imprimePlano()
        {

            Console.WriteLine("---- CMW1 ----");

            foreach (DataRow row in planoCMW1.Rows)
            {
                foreach (DataColumn column in planoCMW1.Columns)
                    Console.Write(column.ToString() + " " + row[column] + "|");


                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            Console.WriteLine("---- CMW2 ----");

            foreach (DataRow row in planoCMW2.Rows)
            {
                foreach (DataColumn column in planoCMW2.Columns)
                    Console.Write(column.ToString() + " " + row[column] + "|");


                Console.WriteLine(" ");
            }

          

        }
    
    
    
    
    }

}
