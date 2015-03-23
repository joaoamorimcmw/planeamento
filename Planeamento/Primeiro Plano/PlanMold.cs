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
        //Não estão a ser usados
        //private int capFGF; 
        //private int capFIMF;
        //private int capFMAN;

        private int horarioGF;
        private int accCaixasGF;
        private int accPesoGF;
        private int diaGF;
        private int semanaGF;
        private int indexGF;

        private int horarioIMF;
        private int accCaixasIMF;
        private int accPesoIMF;
        private int diaIMF;
        private int semanaIMF;
        private int indexIMF;

        private int horarioMAN;
        private int accCaixasMAN;
        private int accPesoMAN;
        private int diaMAN;
        private int semanaMAN;
        private int indexMAN;


        // ************************* CONSTRUTORES ***********************************

        public PlanMold(DataTable prodCMW1, DataTable prodCMW2)
        {
            produtosCMW1 = prodCMW1.Copy();
            produtosCMW2 = prodCMW2.Copy();
            prodCMW1.Clear();
            prodCMW2.Clear();

            Inicializa();
            CalcPlan();
        }

        // ************************* Metodos ***********************************

        public void Inicializa()
        {
            produtosCMW1.Columns.Add(new DataColumn("caixas", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("pesoTotal", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("caixasACC", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("pesoTotalACC", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("semana", typeof(int)));
            produtosCMW1.Columns.Add(new DataColumn("dia", typeof(int)));

            produtosCMW2.Columns.Add(new DataColumn("caixas", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("pesoTotal", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("caixasACC", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("pesoTotalACC", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("semana", typeof(int)));
            produtosCMW2.Columns.Add(new DataColumn("dia", typeof(int)));

            planeamentoMoldacGF = produtosCMW1.Clone();
            planeamentoMoldacIMF = planeamentoMoldacGF.Clone();
            planeamentoMoldacMAN = planeamentoMoldacGF.Clone();

            GetCapacidades();

            horarioGF = 1;
            indexGF = 0;
            diaGF = 1;
            semanaGF = 1;

            horarioIMF = 1;
            indexIMF = 0;
            diaIMF = 1;
            semanaIMF = 1;

            horarioMAN = 1;
            indexMAN = 0;
            diaMAN = 1;
            semanaMAN = 1;

            CalcParametros();
        }

        private void GetCapacidades() {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;

            SqlCommand cmd;
            
            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Mold GF'", connection);
            cmd.CommandType = CommandType.Text;
            capMGF = (int)cmd.ExecuteScalar(); ///420

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Mold IMF'", connection);
            cmd.CommandType = CommandType.Text;
            capMIMF = (int)cmd.ExecuteScalar(); //95

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Mold Manual'", connection);
            cmd.CommandType = CommandType.Text;
            capMMAN = (int)cmd.ExecuteScalar(); //12

            /* O Figueiredo estava a ir buscar estes valores, mas não eram usados
            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Fusao GF'", connection);
            cmd.CommandType = CommandType.Text;
            capFGF = (int)cmd.ExecuteScalar(); //8092

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Fusao IMF'", connection);
            cmd.CommandType = CommandType.Text;
            capFIMF = (int)cmd.ExecuteScalar(); //5753

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Fusao Manual'", connection);
            cmd.CommandType = CommandType.Text;
            capFMAN = (int)cmd.ExecuteScalar(); //5753 */

            BDUtil.FechaBD(connection);
        }

        private void CalcParametros() {

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

        private void CalcPlan()
        {
            foreach (DataRow row in produtosCMW1.Rows)
                InserePlaneamento(row, 1);

            foreach (DataRow row in produtosCMW2.Rows)
                if (Convert.ToInt32(row["local"]) == 2) InserePlaneamento(row, 2);
                else InserePlaneamento(row, 3);
        }

        private void InserePlaneamento(DataRow row, int local)
        {
            int cx = Convert.ToInt32(row[9]);
            int ps = Convert.ToInt32(row[10]);

            if (local == 1)
            {
                if (accCaixasGF + cx > horarioGF * capMGF) SeparaLinha(row, local);
                else if (cx > 0 && ps > 0)
                {
                    indexGF = indexGF + 1;
                    accCaixasGF = accCaixasGF + cx;
                    accPesoGF = accPesoGF + ps;
                    InsereLinha(1, indexGF, semanaGF, diaGF, row[2].ToString(), Convert.ToInt32(row[3]), Convert.ToInt32(row[7]), row[4].ToString(), Convert.ToInt32(row[5]), row[6].ToString(), accCaixasGF, Convert.ToInt32(row[8]), ps, cx, accPesoGF);
                }
            }

            else if (local == 2)
            {
                if (accCaixasIMF + cx > horarioIMF * capMIMF) SeparaLinha(row, local);
                else if (cx > 0 && ps > 0)
                {
                    indexIMF = indexIMF + 1;
                    accCaixasIMF = accCaixasIMF + cx;
                    accPesoIMF = accPesoIMF + ps;
                    InsereLinha(2, indexIMF, semanaIMF, diaIMF, row[2].ToString(), Convert.ToInt32(row[3]), Convert.ToInt32(row[7]), row[4].ToString(), Convert.ToInt32(row[5]), row[6].ToString(), accCaixasIMF, Convert.ToInt32(row[8]), ps, cx, accPesoIMF);
                }
            }

            else
            {
                if (accCaixasMAN + cx > horarioMAN * capMMAN) SeparaLinha(row, local);
                else if (cx > 0 && ps > 0)
                {
                    indexMAN = indexMAN + 1;
                    accCaixasMAN = accCaixasMAN + cx;
                    accPesoMAN = accPesoMAN + ps;
                    InsereLinha(3, indexMAN, semanaMAN, diaMAN, row[2].ToString(), Convert.ToInt32(row[3]), Convert.ToInt32(row[7]), row[4].ToString(), Convert.ToInt32(row[5]), row[6].ToString(), accCaixasMAN, Convert.ToInt32(row[8]), ps, cx, accPesoMAN);
                }
            }
        }

        private void SeparaLinha(DataRow row, int local)
        {
            int espaço = 0, novasCaixas = 0, caixasAcc, pesoNovo, pesoTotalAcc;
            int index, semana, dia;
            DataRow dr;

            if (local == 1)
            {
                espaço = accCaixasGF + Convert.ToInt32(row[9]) - (horarioGF * capMGF);
                dr = planeamentoMoldacGF.NewRow();
                indexGF += 1;
                index = indexGF;
                semana = semanaGF;
                dia = diaGF;
                novasCaixas = (horarioGF * capMGF) - accCaixasGF;
                caixasAcc = accCaixasGF + novasCaixas;
                pesoNovo = novasCaixas * Convert.ToInt32(row[8]) * Convert.ToInt32(row[7]);
                pesoTotalAcc = accPesoGF + pesoNovo;
                accCaixasGF = 0;
                accPesoGF = 0;
            }

            else if (local == 2)
            {
                espaço = accCaixasIMF + Convert.ToInt32(row[9]) - (horarioIMF * capMIMF);
                dr = planeamentoMoldacIMF.NewRow();
                indexIMF += 1;
                index = indexIMF;
                semana = semanaIMF;
                dia = diaIMF;
                novasCaixas = (horarioIMF * capMIMF) - accCaixasIMF;
                caixasAcc = accCaixasIMF + novasCaixas;
                pesoNovo = novasCaixas * Convert.ToInt32(row[8]) * Convert.ToInt32(row[7]);
                pesoTotalAcc = accPesoIMF + pesoNovo;
                accCaixasIMF = 0;
                accPesoIMF = 0;
            }

            else
            {
                espaço = accCaixasMAN + Convert.ToInt32(row[9]) - (horarioMAN * capMMAN);
                dr = planeamentoMoldacMAN.NewRow();
                indexMAN += 1;
                index = indexMAN;
                semana = semanaMAN;
                dia = diaMAN;
                novasCaixas = (horarioMAN * capMMAN) - accCaixasMAN;
                caixasAcc = accCaixasMAN + novasCaixas;
                pesoNovo = novasCaixas * Convert.ToInt32(row[8]) * Convert.ToInt32(row[7]);
                pesoTotalAcc = accPesoMAN + pesoNovo;
                accCaixasMAN = 0;
                accPesoMAN = 0;
            }
            
            InsereLinha(local, index, semana, dia, row[2].ToString(), Convert.ToInt32(row[3]), Convert.ToInt32(row[7]),row[4].ToString(), novasCaixas * Convert.ToInt32(row[8]), row[6].ToString(), caixasAcc, Convert.ToInt32(row[8]), pesoNovo, novasCaixas, pesoTotalAcc);
            
            row["caixas"] = espaço;
            row["qtd"] = espaço * Convert.ToInt32(row[8]);
            row["pesoTotal"] = espaço * Convert.ToInt32(row[8]) * Convert.ToInt32(row[7]);
            
            if (local == 1)
            {
                row["caixasACC"] = accCaixasGF;
                row["pesoTotalACC"] = accPesoGF;
                accPesoGF = Convert.ToInt32(row["pesoTotalACC"]);
                ProximoDia(1); 
                InserePlaneamento(row, 1);
            }

            else if (local == 2)
            {
                row["caixasACC"] = accCaixasIMF;
                row["pesoTotalACC"] = accPesoIMF;
                accPesoIMF = Convert.ToInt32(row["pesoTotalACC"]);
                ProximoDia(2); 
                InserePlaneamento(row, 2);
            }

            else
            {
                row["caixasACC"] = accCaixasMAN;
                row["pesoTotalACC"] = accPesoMAN;
                accPesoMAN = Convert.ToInt32(row["pesoTotalACC"]);
                ProximoDia(3); 
                InserePlaneamento(row, 3);
            }
        }

        private void InsereLinha(int local, int index, int semana, int dia, String noEnc, int noLineEnc, int pesoPeca, String noProd, int qtd, String liga, int caixasAcc, int noMolde, int pesoTotal, int caixas, int pesoTotalAcc, bool insertAtIndex = false)
        {
            DataRow dr;

            switch (local)
            {
                case 1:
                    dr = planeamentoMoldacGF.NewRow();
                    break;
                case 2:
                    dr = planeamentoMoldacIMF.NewRow();
                    break;
                default:
                    dr = planeamentoMoldacMAN.NewRow();
                    break;
            }

            dr["linha"] = index;
            dr["semana"] = semana;
            dr["dia"] = dia;
            dr["noEnc"] = noEnc;
            dr["nolineEnc"] = noLineEnc;
            dr["pesoPeca"] = pesoPeca;
            dr["noProd"] = noProd;
            dr["qtd"] = qtd;
            dr["local"] = local;
            dr["liga"] = liga;
            dr["caixasACC"] = caixasAcc;
            dr["noMolde"] = noMolde;
            dr["pesoTotal"] = pesoTotal;
            dr["caixas"] = caixas;
            dr["pesoTotalACC"] = pesoTotalAcc;

            switch (local)
            {
                case 1:
                    if (insertAtIndex)
                        planeamentoMoldacGF.Rows.InsertAt(dr, index);
                    else
                        planeamentoMoldacGF.Rows.Add(dr);
                    break;
                case 2:
                    if (insertAtIndex)
                        planeamentoMoldacIMF.Rows.InsertAt(dr, index);
                    else
                        planeamentoMoldacIMF.Rows.Add(dr);
                    break;
                default:
                    if (insertAtIndex)
                        planeamentoMoldacMAN.Rows.InsertAt(dr, index);
                    else
                        planeamentoMoldacMAN.Rows.Add(dr);
                    break;
            }
        }

        private void ProximoDia (int local)
        {
            if (local == 1) {
                /*horarioGF = (horarioGF % numeroTurnos) + 1;
                if (horarioGF == 1) {
                    diaGF = (diaGF % 5) + 1;
                    if (diaGF == 1)
                        semanaGF++;
                }*/
                diaGF = (diaGF % 5) + 1;
                if (diaGF == 1)
                    semanaGF++;
            }
            
            else if (local == 2)
            {
                diaIMF = (diaIMF % 5) + 1;
                if (diaIMF == 1)
                    semanaIMF++;
            }     

            else
            {
                diaMAN = (diaMAN % 5) + 1;
                if (diaMAN == 1)
                    semanaMAN++;
            }
        }

        // ****************************************************************************************************************************************************
        // ************************************************* GETs para o BDMolde *****************************************************************************
        // ****************************************************************************************************************************************************

        public DataTable GetPlanMoldGF()
        {
            return planeamentoMoldacGF;
        }

        public DataTable GetPlanMoldIMF()
        {
            return planeamentoMoldacIMF;
        }

        public DataTable GetPlanMoldMAN()
        {
            return planeamentoMoldacMAN;
        }


        // ****************************************************************************************************************************************************
        // ******************************************************************* Prints *************************************************************************
        // ****************************************************************************************************************************************************

        private void ImprimeProd(){

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

        public void ImprimePlan()
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
