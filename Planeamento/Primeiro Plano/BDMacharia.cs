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
    class BDMacharia
    {
        private DataSet produtosCMW1;
        private DataSet produtosCMW2;
        private DataSet machosCMW1;
        private DataSet machosCMW2;
        private DataTable planMachariaCMW1;
        private DataTable planMachariaCMW2;
        private static String SEM_MACHO = "M002204%";

        public BDMacharia()
        {
            InicializaPlanMachariaBD();
            InicializaTabelas();
            GetProdCMW1();
            GetProdCMW2();
            int linhas = 0;
            linhas += SetPlanMachariaCMW1();
            linhas += SetPlanMachariaCMW2();
            Console.WriteLine(linhas + " linhas inseridas na tabela Produtos Plan");
            //ImprimeP();
        }

        //Apaga todos os registos da tabela Plan Macharia
        private void InicializaPlanMachariaBD ()
        {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[CMW$Plan Macharia]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Plan Macharia");
            BDUtil.FechaBD(connection);
        }

        //Inicializa DataTables planMachariaCMW1 e planMachariaCMW2
        public void InicializaTabelas()
        {
            planMachariaCMW1 = new DataTable("PlanMacharia CMW1");
            planMachariaCMW1.Columns.Add(new DataColumn("codPlanMach", typeof(String)));
            planMachariaCMW1.Columns.Add(new DataColumn("noDoc", typeof(String)));
            planMachariaCMW1.Columns.Add(new DataColumn("noLine", typeof(int)));
            planMachariaCMW1.Columns.Add(new DataColumn("noProd", typeof(String)));
            planMachariaCMW1.Columns.Add(new DataColumn("qtd", typeof(decimal)));
            planMachariaCMW1.Columns.Add(new DataColumn("tempo", typeof(decimal)));
            planMachariaCMW1.Columns.Add(new DataColumn("codMach", typeof(String)));
            planMachariaCMW1.Columns.Add(new DataColumn("noLiga", typeof(String)));
            
            planMachariaCMW2 = new DataTable("PlanMacharia CMW2");
            planMachariaCMW2.Columns.Add(new DataColumn("codPlanMach", typeof(String)));
            planMachariaCMW2.Columns.Add(new DataColumn("noDoc", typeof(String)));
            planMachariaCMW2.Columns.Add(new DataColumn("noLine", typeof(int)));
            planMachariaCMW2.Columns.Add(new DataColumn("noProd", typeof(String)));
            planMachariaCMW2.Columns.Add(new DataColumn("qtd", typeof(decimal)));
            planMachariaCMW2.Columns.Add(new DataColumn("tempo", typeof(decimal)));
            planMachariaCMW2.Columns.Add(new DataColumn("codMach", typeof(String)));
            planMachariaCMW2.Columns.Add(new DataColumn("noLiga", typeof(String)));
        }

        //Retira produtos e quantidade, ordenados por local de produção, urgência e data de entrega planeada
        //da tabela Sales Line para o DataSet produtosCMW1
        private void GetProdCMW1()
        {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;
            produtosCMW1 = new DataSet();

            try
            {
                String SQL = BDUtil.QuerySalesLine("[Local de Producao], [Line No_],[Document No_], No_,[Outstanding Quantity],Urgente,[Planned Delivery Date],Material,[Peso Peça [Kg]]],NumeroMoldes", local: 1);
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(produtosCMW1);
                res.Dispose();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
            }
            finally
            {
                BDUtil.FechaBD(connection);
            }
        }

        //Retira produtos e quantidade, ordenados por local de produção, urgência e data de entrega planeada
        //da tabela Sales Line para o DataSet produtosCMW2
        private void GetProdCMW2()
        {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;
            produtosCMW2 = new DataSet();

            try
            {
                String SQL = BDUtil.QuerySalesLine("[Local de Producao], [Line No_],[Document No_], No_,[Outstanding Quantity],Urgente,[Planned Delivery Date],Material,[Peso Peça [Kg]]],NumeroMoldes", local: 2);
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(produtosCMW2);
                res.Dispose();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
            }
            finally
            {
                BDUtil.FechaBD(connection);
            }

       } 

        //lê tabela SalesLine para o DataSet produtosCMW1
        //insere na tabela "Produtos Plan" a lista dos produtos CMW1 a planear
        //por cada produto a planear, verifica os machos associados e preenche machosCMW1
        //por cada macho, vê o tempo de fabrico, faz os calculos correspondentes em função da quantidade
        //insere na DataTable planMachariaCMW1 que é o plano final
        private int SetPlanMachariaCMW1()
        {
            machosCMW1 = new DataSet();
            int linhas = 0;
            foreach (DataTable table in produtosCMW1.Tables)
                foreach (DataRow row in table.Rows)
                {
                    linhas += InsereProdutosPlan(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString());
                    ExecutaMachoCMW1(row);
                }
            return linhas;
        }

        //igual ao setPlanMachariaCMW1, mas para a CMW2
        private int SetPlanMachariaCMW2()
        {
            machosCMW2 = new DataSet();
            int linhas = 0;
            foreach (DataTable table in produtosCMW2.Tables)
                foreach (DataRow row in table.Rows)
                {
                    linhas += InsereProdutosPlan(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString());
                    ExecutaMachoCMW2(row);
                }
            return linhas;
        }


        //Insere cada linha dos DataSets produtosCMW1 e produtosCMW2 na tabela Produtos Plan
        private int InsereProdutosPlan(String lProd,String noLine,String noDoc, String noProd,String qtd,String urg,String planDev,String noL){

            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return 0;
            SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Produtos Plan]([Local Producao],[Document No_],[Line No_],[Prod No_],[Quantidade Pendente],Urgencia,[Planned Delivery Date],[Liga Metalica]) VALUES(@local,@doc,@noLine, @noProd,@qtd,@urg,@planD,@lMet)", connection);
            
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@local", Convert.ToDecimal(lProd));
            cmd.Parameters.AddWithValue("@doc", noDoc);
            cmd.Parameters.AddWithValue("@noLine", Convert.ToDecimal(noLine));
            cmd.Parameters.AddWithValue("@noProd", noProd);
            cmd.Parameters.AddWithValue("@qtd", Convert.ToDecimal(qtd));
            cmd.Parameters.AddWithValue("@urg", Convert.ToDecimal(urg));
            cmd.Parameters.AddWithValue("@planD", Convert.ToDateTime(planDev));
            cmd.Parameters.AddWithValue("@lMet", noL);
        
            cmd.ExecuteNonQuery();

            BDUtil.FechaBD(connection);

            return 1;
        }

        //Por cada produto, obtem os machos da tabela Production BOM Line e coloca no DataSet machosCMW1
        //Por cada macho, calcula o tempo total e insere no planMachariaCMW1
        private void ExecutaMachoCMW1(DataRow linha)
        {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;
            String SQL = "SELECT [No_],Quantity FROM Planeamento.dbo.[CMW$Production BOM Line] WHERE ([No_] NOT LIKE '" + SEM_MACHO + "') AND ([No_] LIKE 'M%') AND ([Production BOM No_] LIKE '" + linha[3] + "#" + "')";

            try
            {
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(machosCMW1);
                res.Dispose();

                foreach (DataTable table in machosCMW1.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        ExecutaGO(row, linha, 1);
                    }
                    machosCMW1.Clear();
                }

            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
            }
            finally
            {
                BDUtil.FechaBD(connection);
            }
        }

        //Por cada produto, obtem os machos da tabela Production BOM Line e coloca no DataSet machosCMW2
        //Por cada macho, calcula o tempo total e insere no planMachariaCMW1

        private void ExecutaMachoCMW2(DataRow linha)
        {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;
            String SQL = "SELECT [No_],Quantity FROM Planeamento.dbo.[CMW$Production BOM Line] WHERE ([No_] NOT LIKE '" + SEM_MACHO + "') AND ([No_] LIKE 'M%') AND ([Production BOM No_] LIKE '" + linha[3] + "#" + "')";

            try
            {
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(machosCMW2);
                res.Dispose();

                foreach (DataTable table in machosCMW2.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        ExecutaGO(row, linha, 2);
                    }
                    machosCMW2.Clear();
                }

            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
            }
            finally
            {
                BDUtil.FechaBD(connection);
            }
        }

        //Calcula o tempo necessário para o macho (tempo do macho * quantidade necessária para o produto)
        //Insere na DataTable planMacharia

        private void ExecutaGO(DataRow macho,DataRow prod,int local) {

            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return;
            
            SqlCommand cmd = new SqlCommand("SELECT [Tempo Fabrico Machos] FROM Planeamento.dbo.[CMW$Item] WHERE [No_] = '" + macho[0].ToString() + "'", connection);
            cmd.CommandType = CommandType.Text;
            object o = cmd.ExecuteScalar();

            if (o != null)
            {
                String res = o.ToString();
                decimal tExec = (decimal)Convert.ToDecimal(res.ToString());
                decimal totalMac = (decimal) Convert.ToDecimal(prod[4].ToString()) * (decimal) Convert.ToDecimal(macho[1].ToString());
                decimal tempoMac = (tExec * totalMac) /60;

                if (tempoMac == 0) tempoMac = 1; //ATENÇÃO --------- todos os tempos dos machos devem estar preenchidos!!!!!

                //String codPlanMach = prod[0].ToString() + prod[2].ToString() + "-" + prod[1].ToString();
                String codPlanMach = prod[0].ToString();
                String noD = prod[2].ToString();
                int nLine = (int) Convert.ToDecimal(prod[1].ToString());
                String noProd = prod[3].ToString();
                String mach = macho[0].ToString();
                String nLig = prod[7].ToString();

                InserePlanMach(codPlanMach,noD,nLine,noProd,totalMac,tempoMac,mach, nLig,local);
            }

            BDUtil.FechaBD(connection);
        }

        //Insere linha de macharia (macho e quantidade) na DataTable planMachariaCMW1 ou 2
        private void InserePlanMach(String codPlanMach, String noDoc, int noLine, String noProd, decimal qtd, decimal tempo,String codMach,String noLiga,int local) {

            DataRow dr;
            if (local==1)  dr = planMachariaCMW1.NewRow();
            else dr = planMachariaCMW2.NewRow();

                dr["codPlanMach"] = codPlanMach;
                dr["noDoc"] = noDoc;
                dr["noLine"] = noLine;
                dr["noProd"] = noProd;
                dr["qtd"] = Convert.ToInt32(qtd);
                dr["tempo"] = Math.Round(Convert.ToDecimal(tempo), 0);
                dr["codMach"] = codMach;
                dr["noLiga"] = noLiga;

            if (local==1) planMachariaCMW1.Rows.Add(dr);
            else planMachariaCMW2.Rows.Add(dr);
        
        }

        // ****************************************************************************************************************************************************
        // ************************************************* GETs para o PlanMacharia *************************************************************************
        // ****************************************************************************************************************************************************

        public DataTable GetListaMachariaCMW1() 
        {
            return planMachariaCMW1;
        }

        public DataTable GetListaMachariaCMW2()
        {
            return planMachariaCMW2;
        }

        // ****************************************************************************************************************************************************
        // ************************************************ Inserts após PlanMacharia *************************************************************************
        // ****************************************************************************************************************************************************

        public void InserePlanos(DataTable pCMW1, DataTable pCMW2)
        {
            int linhas = 0;

            foreach (DataRow row in pCMW1.Rows) linhas += ImportPlano(row);

            foreach (DataRow row in pCMW2.Rows) linhas += ImportPlano(row);

            Console.WriteLine(linhas + " linhas inseridas na tabela Plan Macharia");
        }

        private int ImportPlano(DataRow row) 
        {
            SqlConnection connection = BDUtil.AbreBD();
            if (connection == null)
                return 0;

            int num = GetLinhaGeral(row,connection);

            SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Plan Macharia] (linha, local, semana, dia, noDoc,noLine, noProd, codMach, noLiga,qtd,tempo,acc,fabrica) VALUES(@linha,@local,@semana, @dia,@noDoc,@noLine,@noProd,@codMach,@noLiga,@qtd,@tempo,@acc,@fabrica)", connection);
            
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@linha", num);
            cmd.Parameters.AddWithValue("@local", row[1]);
            cmd.Parameters.AddWithValue("@semana", row[2]);
            cmd.Parameters.AddWithValue("@dia", row[3]);
            cmd.Parameters.AddWithValue("@noDoc", row[4]);
            cmd.Parameters.AddWithValue("@noLine", row[5]);
            cmd.Parameters.AddWithValue("@noProd", row[6]);
            cmd.Parameters.AddWithValue("@codMach", row[7]);
            cmd.Parameters.AddWithValue("@noLiga", row[8]);
            cmd.Parameters.AddWithValue("@qtd", row[9]);
            cmd.Parameters.AddWithValue("@tempo", row[10]);
            cmd.Parameters.AddWithValue("@acc", row[11]);
            cmd.Parameters.AddWithValue("@fabrica", row[12]);

            cmd.ExecuteNonQuery();

            BDUtil.FechaBD(connection);

            return 1;
        }

        private int GetLinhaGeral(DataRow row,SqlConnection connection)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand("SELECT linha FROM Planeamento.dbo.[CMW$Numeracao] WHERE noEnc LIKE '" + row[4].ToString() + "' AND noLine=" + Convert.ToInt32(row[5].ToString()) + " AND noProd LIKE '" + row[6].ToString() + "'", connection);

            cmd.CommandType = CommandType.Text;

            res = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            return res;
        }

        // ****************************************************************************************************************************************************
        // ************************************************* GETs para o BDMolde ******************************************************************************
        // ****************************************************************************************************************************************************

        public DataTable GetCMW1(){

            DataTable prodCMW1 = new DataTable();
            DataRow dr;


            // [Local de Producao], [Line No_],[Document No_], No_,[Outstanding Quantity],Urgente,[Planned Delivery Date],Material,[Peso Peça [Kg]]],NumeroMoldes

            prodCMW1 = new DataTable("Produtos");
            prodCMW1.Columns.Add(new DataColumn("linha", typeof(int)));
            prodCMW1.Columns.Add(new DataColumn("local", typeof(int)));
            prodCMW1.Columns.Add(new DataColumn("noEnc", typeof(String)));
            prodCMW1.Columns.Add(new DataColumn("nolineEnc", typeof(int)));
            prodCMW1.Columns.Add(new DataColumn("noProd", typeof(String)));
            prodCMW1.Columns.Add(new DataColumn("qtd", typeof(int)));
            prodCMW1.Columns.Add(new DataColumn("liga", typeof(String)));
            prodCMW1.Columns.Add(new DataColumn("pesoPeca", typeof(int)));
            prodCMW1.Columns.Add(new DataColumn("noMolde", typeof(int)));

            int sumLinha =1;

            foreach (DataTable table in produtosCMW1.Tables)
                foreach (DataRow row in table.Rows)
                {

                    dr = prodCMW1.NewRow();
                    dr["linha"] = sumLinha;
                    dr["local"] = Convert.ToInt32(row[0]);
                    dr["noEnc"] = row[2];
                    dr["nolineEnc"] = Convert.ToInt32(row[1]);
                    dr["noProd"] = row[3];
                    dr["qtd"] = Convert.ToInt32(row[4]);
                    dr["liga"] = row[7];
                    dr["pesoPeca"] = Convert.ToInt32(row[8]);
                    dr["noMolde"] = Convert.ToInt32(row[9]);

                    sumLinha += 1;

                    prodCMW1.Rows.Add(dr);
                }

            produtosCMW1.Clear();

            return prodCMW1;
        }

        public DataTable GetCMW2()
        {
            DataTable prodCMW2 = new DataTable();
            DataRow dr;

            // [Local de Producao], [Line No_],[Document No_], No_,[Outstanding Quantity],Urgente,[Planned Delivery Date],Material,[Peso Peça [Kg]]],NumeroMoldes

            prodCMW2 = new DataTable("Produtos");
            prodCMW2.Columns.Add(new DataColumn("linha", typeof(int)));
            prodCMW2.Columns.Add(new DataColumn("local", typeof(int)));
            prodCMW2.Columns.Add(new DataColumn("noEnc", typeof(String)));
            prodCMW2.Columns.Add(new DataColumn("nolineEnc", typeof(int)));
            prodCMW2.Columns.Add(new DataColumn("noProd", typeof(String)));
            prodCMW2.Columns.Add(new DataColumn("qtd", typeof(int)));
            prodCMW2.Columns.Add(new DataColumn("liga", typeof(String)));
            prodCMW2.Columns.Add(new DataColumn("pesoPeca", typeof(int)));
            prodCMW2.Columns.Add(new DataColumn("noMolde", typeof(int)));

            int sumLinha = 1;

            foreach (DataTable table in produtosCMW2.Tables)
                foreach (DataRow row in table.Rows)
                {
                    dr = prodCMW2.NewRow();
                    dr["linha"] = sumLinha;
                    dr["local"] = Convert.ToInt32(row[0]);
                    dr["noEnc"] = row[2];
                    dr["nolineEnc"] = Convert.ToInt32(row[1]);
                    dr["noProd"] = row[3];
                    dr["qtd"] = Convert.ToInt32(row[4]);
                    dr["liga"] = row[7];
                    dr["pesoPeca"] = Convert.ToInt32(row[8]);
                    dr["noMolde"] = Convert.ToInt32(row[9]);

                    sumLinha += 1;

                    prodCMW2.Rows.Add(dr);

                }

            produtosCMW2.Clear();

            return prodCMW2;
        }
        

        // ****************************************************************************************************************************************************
        // **********************************************************  PRINT´s ********************************************************************************
        // ****************************************************************************************************************************************************


        public void ImprimeP()
        {
            Console.WriteLine("----- CMW 1 -----");

            foreach (DataRow row in planMachariaCMW1.Rows)
            {
                foreach (DataColumn column in planMachariaCMW1.Columns)
                    Console.Write(row[column] + "|");
                Console.WriteLine(" ");

            }
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("----- CMW 2 -----");

            foreach (DataRow row in planMachariaCMW2.Rows)
            {
                foreach (DataColumn column in planMachariaCMW2.Columns)
                    Console.Write(row[column] + "|");
                Console.WriteLine(" ");

            }

        }


        private void ImprimeMachos()
        {
            Console.WriteLine("---- CMW1 ----");

            foreach (DataTable table in machosCMW1.Tables)
            {
                foreach (DataRow row in table.Rows)
                    foreach (DataColumn column in table.Columns)
                        Console.WriteLine(row[column]);
                 
            }
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("---- CMW2 ----");

            foreach (DataTable table in machosCMW2.Tables)
                foreach (DataRow row in table.Rows)
                    foreach (DataColumn column in table.Columns)
                        Console.WriteLine(row[column]);
        }


        private void ImprimeProd()
        {
            Console.WriteLine("---- CMW1 ----");

            foreach (DataTable table in produtosCMW1.Tables)
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                        Console.Write(row[column] + "|");
                    Console.WriteLine(" ");
                }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("---- CMW2 ----");

            foreach (DataTable table in produtosCMW2.Tables)
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                        Console.Write(row[column] + "|");
                    Console.WriteLine(" ");
                }
        }

        // ****************************************************************************************************************************************************
        // ********************************************************** OBSOLETO ********************************************************************************
        // ****************************************************************************************************************************************************
        
        //Numeração é gerada automaticamente pelo SQL
        /*

        private void InsereNumeracaoCMW1()
        {

            foreach (DataTable table in produtosCMW1.Tables)
                foreach (DataRow row in table.Rows)
                {
                    SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
                    SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Numeracao] (linha, noEnc, noLine, noProd) VALUES(@linha,@noEnc,@noLine, @noProd)", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.Parameters.AddWithValue("@linha", numeracao);
                    cmd.Parameters.AddWithValue("@noEnc", row[2].ToString());
                    cmd.Parameters.AddWithValue("@noLine", Convert.ToInt32(row[1].ToString()));
                    cmd.Parameters.AddWithValue("@noProd", row[3].ToString());
                    cmd.ExecuteNonQuery();
                    numeracao++;
                    con.Close();
                }

        }


        private void InsereNumeracaoCMW2()
        {

            foreach (DataTable table in produtosCMW2.Tables)
                foreach (DataRow row in table.Rows)
                {
                    SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
                    SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Numeracao] (linha, noEnc, noLine, noProd) VALUES(@linha,@noEnc,@noLine, @noProd)", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.Parameters.AddWithValue("@linha", numeracao);
                    cmd.Parameters.AddWithValue("@noEnc", row[2].ToString());
                    cmd.Parameters.AddWithValue("@noLine", Convert.ToInt32(row[1].ToString()));
                    cmd.Parameters.AddWithValue("@noProd", row[3].ToString());
                    cmd.ExecuteNonQuery();
                    numeracao++;
                    con.Close();
                }

        }*/

    }

}
