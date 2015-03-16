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

    class PlanMold
    {

        private DataTable produtosCMW1;
        private DataTable produtosCMW2;
        private DataTable planeamentoMoldacGF;
        private DataTable planeamentoMoldacIMF;
        private DataTable planeamentoMoldacMAN;
        private int capMGF;
        private int capMIMF;
        private int capMMAN;
        private int capFGF;
        private int capFIMF;
        private int capFMAN;
        private int horarioGF;
        private int accCaixasGF;
        private int accPesoGF;
        private string diaGF;
        private int semanaGF;
        private int indexGF;
        private int horarioIMF;
        private int accCaixasIMF;
        private int accPesoIMF;
        private string diaIMF;
        private int semanaIMF;
        private int indexIMF;
        private int horarioMAN;
        private int accCaixasMAN;
        private int accPesoMAN;
        private string diaMAN;
        private int semanaMAN;
        private int indexMAN;


        // ************************* CONSTRUTORES ***********************************

        public PlanMold(DataTable prodCMW1, DataTable prodCMW2)
        {

            //apaga os planeamentos origem
            produtosCMW1 = prodCMW1.Copy();
            produtosCMW2 = prodCMW2.Copy();
            prodCMW1.Clear();
            prodCMW2.Clear();

            init();
            calcPlan();
            

        }

     




        // ************************* Metodos ***********************************





        public void init()
        {


            produtosCMW1.Columns.Add(new DataColumn("caixas", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("pesoTotal", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("caixasACC", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("pesoTotalACC", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("semanaGF", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("diaGF", typeof(String)));
            produtosCMW2.Columns.Add(new DataColumn("caixas", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("pesoTotal", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("caixasACC", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("pesoTotalACC", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("semanaGF", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("diaGF", typeof(String)));

            planeamentoMoldacGF = produtosCMW1.Clone();
            planeamentoMoldacIMF = planeamentoMoldacGF.Clone();
            planeamentoMoldacMAN = planeamentoMoldacGF.Clone();

            capMGF = getCapacidadeMold(1);
            capMIMF = getCapacidadeMold(2);
            capMMAN = getCapacidadeMold(3);
            capFGF = getCapacidadeFus(1);
            capFIMF = getCapacidadeFus(2);
            capFMAN = getCapacidadeFus(3);
            horarioGF = 1;
            indexGF = 0;
            diaGF = "Segunda-feira";
            semanaGF = 1;


            horarioIMF = 1;
            indexIMF = 0;
            diaIMF = "Segunda-feira";
            semanaIMF = 1;

            horarioMAN = 1;
            indexMAN = 0;
            diaMAN = "Segunda-feira";
            semanaMAN = 1;


            calcParametros();


        }



        private void calcParametros() {

            foreach (DataRow row in produtosCMW1.Rows)
            {
                row[9] = Convert.ToInt32(row[5]) / Convert.ToInt32(row[8]);
                row[10] = Convert.ToInt32(row[5]) * Convert.ToInt32(row[7]);
                row[11] = 0;
                row[12] = 0 ;

            }

            foreach (DataRow row in produtosCMW2.Rows)
            {

                row[9] = Convert.ToInt32(row[5]) / Convert.ToInt32(row[8]);
                row[10] = Convert.ToInt32(row[5]) * Convert.ToInt32(row[7]);
                row[11] = 0;
                row[12] = 0;


            }

        }

        private int getCapacidadeMold(int local)
        {
            int res = 0;
            
            /*SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");

            
            if (local == 1) {  SqlCommand cmd = new SqlCommand("SELECT [Capacidade Mold GF] FROM Planeamento.dbo.[CMW$Parametros]", con);}
            else if (local == 2) { SqlCommand cmd = new SqlCommand("SELECT [Capacidade Mold IMF] FROM Planeamento.dbo.[CMW$Parametros]", con); }
            else SqlCommand cmd = new SqlCommand("SELECT [Capacidade Mold MAN] FROM Planeamento.dbo.[CMW$Parametros]", con);
            

            
            cmd.CommandType = CommandType.Text;
            con.Open();
            res = (int)cmd.ExecuteScalar();
            con.Close();
            
             * */
             
            if (local==1) {  res=420; }
            else if (local==2) { res=95; }
            else res=12;


             return res;
        }
        private int getCapacidadeFus(int local)
        {
            int res = 0;

            /*SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");

            
            if (local == 1) {  SqlCommand cmd = new SqlCommand("SELECT [Capacidade Mold GF] FROM Planeamento.dbo.[CMW$Parametros]", con);}
            else if (local == 2) { SqlCommand cmd = new SqlCommand("SELECT [Capacidade Mold IMF] FROM Planeamento.dbo.[CMW$Parametros]", con); }
            else SqlCommand cmd = new SqlCommand("SELECT [Capacidade Mold MAN] FROM Planeamento.dbo.[CMW$Parametros]", con);
            

            
            cmd.CommandType = CommandType.Text;
            con.Open();
            res = (int)cmd.ExecuteScalar();
            con.Close();
            
             * */

            if (local==1) {  res=8092; }
            else if (local==2) { res=5753; }
            else res=5753;


            return res;
        }


        private void setDay(int local)
        {


            if (local == 1)
            {
                if (String.Compare("Segunda-feira", diaGF, true) == 0)
                    diaGF = "Terça-feira";
                else if (String.Compare("Terça-feira", diaGF, true) == 0)
                    diaGF = "Quarta-feira";
                else if (String.Compare("Quarta-feira", diaGF, true) == 0)
                    diaGF = "Quinta-feira";
                else if (String.Compare("Quinta-feira", diaGF, true) == 0)
                    diaGF = "Sexta-feira";
                else if (String.Compare("Sexta-feira", diaGF, true) == 0)
                {
                    diaGF = "Segunda-feira";
                    semanaGF = semanaGF + 1;
                }
            }
            
            if (local == 2)
            {
                if (String.Compare("Segunda-feira", diaIMF, true) == 0)
                    diaIMF = "Terça-feira";
                else if (String.Compare("Terça-feira", diaIMF, true) == 0)
                    diaIMF = "Quarta-feira";
                else if (String.Compare("Quarta-feira", diaIMF, true) == 0)
                    diaIMF = "Quinta-feira";
                else if (String.Compare("Quinta-feira", diaIMF, true) == 0)
                    diaIMF = "Sexta-feira";
                else if (String.Compare("Sexta-feira", diaIMF, true) == 0)
                {
                    diaIMF = "Segunda-feira";
                    semanaIMF = semanaIMF + 1;
                }
            }     
            if (local == 3)
            {
                if (String.Compare("Segunda-feira", diaMAN, true) == 0)
                    diaMAN = "Terça-feira";
                else if (String.Compare("Terça-feira", diaMAN, true) == 0)
                    diaMAN = "Quarta-feira";
                else if (String.Compare("Quarta-feira", diaMAN, true) == 0)
                    diaMAN = "Quinta-feira";
                else if (String.Compare("Quinta-feira", diaMAN, true) == 0)
                    diaMAN = "Sexta-feira";
                else if (String.Compare("Sexta-feira", diaMAN, true) == 0)
                {
                    diaMAN = "Segunda-feira";
                    semanaMAN = semanaMAN + 1;
                }

            }
        }


        private void splitRow(DataRow row,int local)
        {
           
            int espaço=0;
            DataRow dr;

            if (local == 1) {
                espaço = accCaixasGF + Convert.ToInt32(row[9]) - (horarioGF * capMGF);
                dr = planeamentoMoldacGF.NewRow();
                indexGF += 1;
                dr["linha"] = indexGF;
            }
            else if (local == 2) { espaço = accCaixasIMF + Convert.ToInt32(row[9]) - (horarioIMF * capMIMF);
                dr = planeamentoMoldacIMF.NewRow();
                indexGF += 1;
                indexIMF += 1;
                dr["linha"] = indexIMF; 
            
            }
            else { espaço = accCaixasMAN + Convert.ToInt32(row[9]) - (horarioMAN * capMMAN);
                dr = planeamentoMoldacMAN.NewRow();
                indexMAN += 1;
                dr["linha"] = indexMAN; 
            
            }
            dr["local"] = Convert.ToInt32(row[1]);
            if (local == 1) { dr["semanaGF"] = semanaGF; dr["diaGF"] = diaGF; }
            else if (local == 2) { dr["semanaGF"] = semanaIMF; dr["diaGF"] = diaIMF; }
            else { dr["semanaGF"] = semanaMAN; dr["diaGF"] = diaMAN; }
            dr["noEnc"] = row[2].ToString();
            dr["nolineEnc"] = row[3];
            dr["noProd"] = row[4];
            dr["noMolde"] = Convert.ToInt32(row[8]);
            dr["pesoPeca"] = Convert.ToInt32(row[7]);
            dr["liga"] = row[6].ToString();
            int novasCaixas=0;
            if (local == 1) { novasCaixas = (horarioGF * capMGF) - accCaixasGF; }
            else if (local == 2) { novasCaixas = (horarioIMF * capMIMF) - accCaixasIMF; }
            else { novasCaixas = (horarioMAN * capMMAN) - accCaixasMAN; }
            dr["caixas"] = novasCaixas;
            dr["qtd"] = novasCaixas * Convert.ToInt32(row[8]);
            int pesoNovo = novasCaixas * Convert.ToInt32(row[8]) * Convert.ToInt32(row[7]);
            if (local == 1) { dr["caixasACC"] = accCaixasGF + novasCaixas; }
            else if (local == 2) { dr["caixasACC"] = accCaixasIMF + novasCaixas; }
            else { dr["caixasACC"] = accCaixasMAN + novasCaixas; }
            dr["pesoTotal"] = pesoNovo;
            if (local == 1)
            {
                dr["pesoTotalACC"] = accPesoGF + pesoNovo;
                accCaixasGF = 0;
                accPesoGF = 0;
                planeamentoMoldacGF.Rows.InsertAt(dr, indexGF);
            }
            else if (local == 2)
            {
                dr["pesoTotalACC"] = accPesoIMF + pesoNovo;
                accCaixasIMF = 0;
                accPesoIMF = 0;
                planeamentoMoldacIMF.Rows.InsertAt(dr, indexIMF);
            }
            else
            {
                dr["pesoTotalACC"] = accPesoMAN + pesoNovo;
                accCaixasMAN = 0;
                accPesoMAN = 0;
                planeamentoMoldacMAN.Rows.InsertAt(dr, indexMAN);
            }
            row["caixas"] = espaço;
            row["qtd"] = espaço * Convert.ToInt32(row[8]);
            row["pesoTotal"] = espaço * Convert.ToInt32(row[8]) * Convert.ToInt32(row[7]);
            if (local == 1)
            {
                row["caixasACC"] = accCaixasGF;
                row["pesoTotalACC"] = accPesoGF;
                accPesoGF = Convert.ToInt32(row["pesoTotalACC"]);
                setDay(1); inserePlaneamento(row, 1);
            }
            else if (local == 2)
            {
                row["caixasACC"] = accCaixasIMF;
                row["pesoTotalACC"] = accPesoIMF;
                accPesoIMF = Convert.ToInt32(row["pesoTotalACC"]);
                setDay(2); inserePlaneamento(row, 2);
            }
            else
            {
                row["caixasACC"] = accCaixasMAN;
                row["pesoTotalACC"] = accPesoMAN;
                accPesoMAN = Convert.ToInt32(row["pesoTotalACC"]);
                setDay(3); inserePlaneamento(row, 3);
            }
        }




        

        private void inserePlaneamento(DataRow row, int local)
        {

            int cx =Convert.ToInt32(row[9]);
            int ps = Convert.ToInt32(row[10]);

            if (local == 1)
            {
                if (accCaixasGF + cx > horarioGF * capMGF) splitRow(row, 1);
                else if (cx > 0 && ps > 0)
                {
                    indexGF = indexGF + 1;
                    accCaixasGF = accCaixasGF + cx;
                    accPesoGF = accPesoGF + ps;
                    DataRow dr = planeamentoMoldacGF.NewRow();
                    dr["linha"] = indexGF;
                    dr["semanaGF"] = semanaGF;
                    dr["diaGF"] = diaGF;
                    dr["noEnc"] = row[2].ToString();
                    dr["nolineEnc"] = row[3];
                    dr["pesoPeca"] = row[7];
                    dr["noProd"] = row[4];
                    dr["qtd"] = Convert.ToInt32(row[5]);
                    dr["local"] = Convert.ToInt32(row[1]);
                    dr["liga"] = row[6].ToString();
                    dr["caixasACC"] = accCaixasGF;
                    dr["noMolde"] = Convert.ToInt32(row[8]);
                    dr["pesoTotal"] = ps;
                    dr["caixas"] = cx;
                    dr["pesoTotalACC"] = accPesoGF;
                    planeamentoMoldacGF.Rows.Add(dr);

                }
            }
            else if (local == 2) {

                if (accCaixasIMF + cx > horarioIMF * capMIMF) splitRow(row, 2);
                else if (cx > 0 && ps > 0)
                {
                    indexIMF = indexIMF + 1;
                    accCaixasIMF = accCaixasIMF + cx;
                    accPesoIMF = accPesoIMF + ps;
                    DataRow dr = planeamentoMoldacIMF.NewRow();
                    dr["linha"] = indexIMF;
                    dr["semanaGF"] = semanaIMF;
                    dr["diaGF"] = diaIMF;
                    dr["noEnc"] = row[2].ToString();
                    dr["nolineEnc"] = row[3];
                    dr["pesoPeca"] = row[7];
                    dr["noProd"] = row[4];
                    dr["qtd"] = Convert.ToInt32(row[5]);
                    dr["local"] = Convert.ToInt32(row[1]);
                    dr["liga"] = row[6].ToString();
                    dr["caixasACC"] = accCaixasIMF;
                    dr["noMolde"] = Convert.ToInt32(row[8]);
                    dr["pesoTotal"] = ps;
                    dr["caixas"] = cx;
                    dr["pesoTotalACC"] = accPesoIMF;
                    planeamentoMoldacIMF.Rows.Add(dr);

                }


            }
            else if (local == 3)
            {

                if (accCaixasMAN + cx > horarioMAN * capMMAN) splitRow(row, 3);
                else if (cx > 0 && ps > 0)
                {
                    indexMAN = indexMAN + 1;
                    accCaixasMAN = accCaixasMAN + cx;
                    accPesoMAN = accPesoMAN + ps;
                    DataRow dr = planeamentoMoldacMAN.NewRow();
                    dr["linha"] = indexMAN;
                    dr["semanaGF"] = semanaMAN;
                    dr["diaGF"] = diaMAN;
                    dr["noEnc"] = row[2].ToString();
                    dr["nolineEnc"] = row[3];
                    dr["pesoPeca"] = row[7];
                    dr["noProd"] = row[4];
                    dr["qtd"] = Convert.ToInt32(row[5]);
                    dr["local"] = Convert.ToInt32(row[1]);
                    dr["liga"] = row[6].ToString();
                    dr["caixasACC"] = accCaixasMAN;
                    dr["noMolde"] = Convert.ToInt32(row[8]);
                    dr["pesoTotal"] = ps;
                    dr["caixas"] = cx;
                    dr["pesoTotalACC"] = accPesoMAN;
                    planeamentoMoldacMAN.Rows.Add(dr);

                }


            }

            
            
        }


        private void calcPlan()
        {  
            foreach (DataRow row in produtosCMW1.Rows)
                inserePlaneamento(row,1);
                        

            foreach (DataRow row in produtosCMW2.Rows)
                if (Convert.ToInt32(row["local"]) == 2) inserePlaneamento(row, 2);
                else inserePlaneamento(row, 3);
                        
        }












// ****************************************** PRINTS ********************************************************



        private void imprimeProd(){

            Console.WriteLine("---- CMW1 ----");

            foreach (DataRow row in produtosCMW1.Rows)
            {
                foreach (DataColumn column in produtosCMW1.Columns)
                    Console.Write(column.ToString() + " -" + row[column] + "|");


                Console.WriteLine(" ");
            }


            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("---- CMW2 ----");

            foreach (DataRow row in produtosCMW2.Rows)
            {
                foreach (DataColumn column in produtosCMW2.Columns)
                    Console.Write(column.ToString() + " -" + row[column] + "|");


                Console.WriteLine(" ");
            }
        
        
        
        }


        public DataTable getPlanMoldGF() {
            return planeamentoMoldacGF;
        }

        public DataTable getPlanMoldIMF()
        {
            return planeamentoMoldacIMF;
        }

        public DataTable getPlanMoldMAN()
        {
            return planeamentoMoldacMAN;
        }


        public void imprimePlan()
        {

            Console.WriteLine("---- GF ----");

            foreach (DataRow row in planeamentoMoldacGF.Rows)
            {
                foreach (DataColumn column in planeamentoMoldacGF.Columns)
                    Console.Write(column.ToString() + " " + row[column] + "|");


                Console.WriteLine(" ");
            }
            
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            Console.WriteLine("---- IMF ----");

            foreach (DataRow row in planeamentoMoldacIMF.Rows)
            {
                foreach (DataColumn column in planeamentoMoldacIMF.Columns)
                    Console.Write(column.ToString() + " " + row[column] + "|");


                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            Console.WriteLine("---- MAN ----");

            foreach (DataRow row in planeamentoMoldacMAN.Rows)
            {
                foreach (DataColumn column in planeamentoMoldacMAN.Columns)
                    Console.Write(column.ToString() + " " + row[column] + "|");


                Console.WriteLine(" ");
            }

            
        }
            
            

    }
              
           

}
