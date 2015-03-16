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

     private SqlConnection connection;
     private String SQL;
     private DataSet produtosCMW1;
     private DataSet produtosCMW2;
     private DataSet machosCMW1;
     private DataSet machosCMW2;
     private DataTable planMachariaCMW1;
     private DataTable planMachariaCMW2;
     private int numeracao;


     public BDMacharia()
     {
          numeracao = 1;
         //inicializa as tabelas "Produtos Plan"; "Plan Macharia"; e as DataTables planMachariaCMW1; planMachariaCMW2
         //insere a numeracao
         inicializaTabelas();
         //vai ler a tabela "Sales Line" e preenche o DataSet produtosCMW1      
         //insere na tabela "Produtos Plan" a lista dos produtos CMW1 a planear
         //por cada produto a planear, verifica os machos associados e preenche machosCMW1
         //por cada macho, vê o tempo de fabrico, faz os calculos correspondentes em função da quantidade
         //insere na DataTable planMachariaCMW1 que é o plano final
         setPlanMachariaCMW1();
         //iguak ao CMW1 só que para a CMW2
         setPlanMachariaCMW2();
         //imprimeP();

         
     }

     private void abreBD(){
        connection = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
  
          try {
             connection.Open();
              }
          catch (Exception ex) {
             System.Console.Write(ex);
             System.Console.WriteLine("Erro na conecção.");
              }
    }

     private void fechaBD() {
         try
         {
             connection.Close();
         }
         catch (Exception ex) {
             System.Console.Write(ex);
             System.Console.WriteLine("Erro ao terminal conecção.");
         }

     }


    //saca produtos + quantidade - ordenadas por local de produção, urgencia,data de entrega planeada
     private void getProdCMW1()
     {
         abreBD();
         produtosCMW1 = new DataSet();
         try
         {
             SQL = "SELECT [Local de Producao], [Line No_],[Document No_], No_,[Outstanding Quantity],Urgente,[Planned Delivery Date],Material,[Peso Peça [Kg]]],NumeroMoldes FROM Planeamento.dbo.[CMW$Sales Line] WHERE ([Outstanding Quantity]>0) AND ([Posting Group]='PROD.ACABA') AND [Planned Delivery Date] >= '01-01-14' AND ([Local de Producao] >0) AND ([Local de Producao] = 1) ORDER BY [Local de Producao] ASC,Urgente DESC,[Planned Delivery Date] ASC";
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
             fechaBD();
         }

         //nao é necessário é feito logo em sql
         //insereNumeracaoCMW1();


     }


     private void insereNumeracaoCMW1() {

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


        private void insereNumeracaoCMW2() {

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
                 
     }

     private void getProdCMW2()
     {
         abreBD();
         produtosCMW2 = new DataSet();
         try
         {
             SQL = "SELECT [Local de Producao], [Line No_],[Document No_], No_,[Outstanding Quantity],Urgente,[Planned Delivery Date],Material,[Peso Peça [Kg]]],NumeroMoldes FROM Planeamento.dbo.[CMW$Sales Line] WHERE ([Outstanding Quantity]>0) AND ([Posting Group]='PROD.ACABA') AND [Planned Delivery Date] >= '01-01-14' AND ([Local de Producao] >0) AND ([Local de Producao] > 1) ORDER BY Urgente DESC,[Planned Delivery Date] ASC";
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
             fechaBD();
         }

         //nao é necessário é feito no sql
         //insereNumeracaoCMW2();

     }


    private void insereProdutosPlan(String lProd,String noLine,String noDoc, String noProd,String qtd,String urg,String planDev,String noL){


        SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
        SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Produtos Plan]([Local Producao],[Document No_],[Line No_],[Prod No_],[Quantidade Pendente],Urgencia,[Planned Delivery Date],[Liga Metalica]) VALUES(@local,@doc,@noLine, @noProd,@qtd,@urg,@planD,@lMet)", con);
            
                cmd.CommandType = CommandType.Text;
                con.Open();


                cmd.Parameters.AddWithValue("@local", Convert.ToDecimal(lProd));
                cmd.Parameters.AddWithValue("@doc", noDoc);
                cmd.Parameters.AddWithValue("@noLine", Convert.ToDecimal(noLine));
                cmd.Parameters.AddWithValue("@noProd", noProd);
                cmd.Parameters.AddWithValue("@qtd", Convert.ToDecimal(qtd));
                cmd.Parameters.AddWithValue("@urg", Convert.ToDecimal(urg));
                cmd.Parameters.AddWithValue("@planD", Convert.ToDateTime(planDev));
                cmd.Parameters.AddWithValue("@lMet", noL);
        
                cmd.ExecuteNonQuery();


                con.Close();
           }


     private void executaGO(DataRow macho,DataRow prod,int local) {

         using (SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;"))
         {
             using (SqlCommand cmd = new SqlCommand("SELECT [Tempo Fabrico Machos] FROM Planeamento.dbo.[CMW$Item] WHERE [No_] = '" + macho[0].ToString() + "'", con))
             {
                 cmd.CommandType = CommandType.Text;
                 con.Open();
                 object o = cmd.ExecuteScalar();
                 if (o != null)
                 {
                     String res = o.ToString();
                     decimal tExec = (decimal)Convert.ToDecimal(res.ToString());
                     decimal totalMac = (decimal) Convert.ToDecimal(prod[4].ToString()) * (decimal) Convert.ToDecimal(macho[1].ToString());
                     decimal tempoMac = (tExec * totalMac) /60;

                     if (tempoMac == 0) tempoMac = 1;                                                                                                   //ATENÇÃO --------- todos os tempos dos machos devem estar preenchidos!!!!!

                     //String codPlanMach = prod[0].ToString() + prod[2].ToString() + "-" + prod[1].ToString();
                     String codPlanMach = prod[0].ToString();
                     String noD = prod[2].ToString();
                     int nLine = (int) Convert.ToDecimal(prod[1].ToString());
                     String noProd = prod[3].ToString();
                     String mach = macho[0].ToString();
                     String nLig = prod[7].ToString();

                     inserePlanMach(codPlanMach,noD,nLine,noProd,totalMac,tempoMac,mach, nLig,local);

                 }
                 con.Close();
             }
         }
     }

     private void inserePlanMach(String codPlanMach, String noDoc, int noLine, String noProd, decimal qtd, decimal tempo,String codMach,String noLiga,int local) {

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



     private void setPlanMachariaCMW1()
     {
         getProdCMW1();


         machosCMW1 = new DataSet(); 
         foreach (DataTable table in produtosCMW1.Tables)
             foreach (DataRow row in table.Rows)
             {
                 insereProdutosPlan(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString());
                 executaMachoCMW1(row);
             }
             
                           
     }


     private void setPlanMachariaCMW2()
     {
         getProdCMW2();
         machosCMW2 = new DataSet();
         foreach (DataTable table in produtosCMW2.Tables)
             foreach (DataRow row in table.Rows)
             {
                 insereProdutosPlan(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString());
                 executaMachoCMW2(row);
             }


     }

     
     //por cada produto, vê por quantos machos é composto
     private void executaMachoCMW1(DataRow linha)
     {
         abreBD();
         SQL = "SELECT [No_],Quantity FROM Planeamento.dbo.[CMW$Production BOM Line] WHERE ([No_] NOT LIKE 'M002204%') AND ([No_] LIKE 'M%') AND ([Production BOM No_] LIKE '" + linha[3] + "#" + "')";

         try
         {
             SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
             res.Fill(machosCMW1);
             res.Dispose();
             
             foreach (DataTable table in machosCMW1.Tables)
             {
                 foreach (DataRow row in table.Rows)
                 {
                     executaGO(row, linha,1);
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
             fechaBD();
         }
     }
     private void executaMachoCMW2(DataRow linha)
     {
         abreBD();
         SQL = "SELECT [No_],Quantity FROM Planeamento.dbo.[CMW$Production BOM Line] WHERE ([No_] NOT LIKE 'M002204%') AND ([No_] LIKE 'M%') AND ([Production BOM No_] LIKE '" + linha[3] + "#" + "')";

         try
         {
             SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
             res.Fill(machosCMW2);
             res.Dispose();

             foreach (DataTable table in machosCMW2.Tables)
             {
                 foreach (DataRow row in table.Rows)
                 {
                     executaGO(row, linha,2);
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
             fechaBD();
         }
     }

     private void initPlanMacharia() {

       //apaga todos os registos
       SqlConnection con2 = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
       SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Plan Macharia]", con2);
       cmd2.CommandType = CommandType.Text;
       con2.Open();
       cmd2.ExecuteNonQuery();
       con2.Close();
               
     }

     private void initProdPlan()
     {

         //apaga todos os registos
         SqlConnection con2 = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
         SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Produtos Plan]", con2);
         cmd2.CommandType = CommandType.Text;
         con2.Open();
         cmd2.ExecuteNonQuery();
      //   con2.Close();

    //     con2 = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
         cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Numeracao]", con2);
         cmd2.CommandType = CommandType.Text;
      //   con2.Open();
         cmd2.ExecuteNonQuery();
      //   con2.Close();

        // con2 = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
         cmd2 = new SqlCommand("DBCC CHECKIDENT ('Planeamento.dbo.[CMW$Numeracao]', RESEED, 0)", con2);
         cmd2.CommandType = CommandType.Text;
        // con2.Open();
         cmd2.ExecuteNonQuery();
         con2.Close();

     }



     public void inicializaTabelas() {

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

         initProdPlan();
         initPlanMacharia();

         initNumeracao();


     }

     private void initNumeracao() {

         SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
         
             //insere os novos registos
         SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Numeracao] SELECT [Document No_],[Line No_], No_ FROM Planeamento.dbo.[CMW$Sales Line] WHERE ([Outstanding Quantity]>0) AND ([Posting Group]='PROD.ACABA') AND [Planned Delivery Date] >= '01-01-14' AND ([Local de Producao] >0) ORDER BY [Local de Producao] ASC,Urgente DESC,[Planned Delivery Date] ASC", con);

                 cmd.CommandType = CommandType.Text;
                 con.Open();
                 cmd.ExecuteNonQuery();
                 con.Close();
             
         
     }

     public DataTable getListaMachariaCMW1() {
        return planMachariaCMW1;
     }
     public DataTable getListaMachariaCMW2()
     {
         return planMachariaCMW2;
     }


     private int getLinhaGeral(DataRow row)
     {

        int res=0;

        SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
        SqlCommand cmd = new SqlCommand("SELECT linha FROM Planeamento.dbo.[CMW$Numeracao] WHERE noEnc LIKE '" + row[4].ToString()+"' AND noLine="+Convert.ToInt32(row[5].ToString())+ " AND noProd LIKE '"+row[6].ToString()+"'", con);
         
                cmd.CommandType = CommandType.Text;
                con.Open();
                //cmd.ExecuteNonQuery();

                res= Convert.ToInt32(cmd.ExecuteScalar().ToString());

                con.Close();

        return res;
    }



     private void importPlano(DataRow row) {


         int num = getLinhaGeral(row);


         using (SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;"))
         {

             //insere os novos registos
             using (SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Plan Macharia] (linha, local, semana, dia, noDoc,noLine, noProd, codMach, noLiga,qtd,tempo,acc,fabrica) VALUES(@linha,@local,@semana, @dia,@noDoc,@noLine,@noProd,@codMach,@noLiga,@qtd,@tempo,@acc,@fabrica)", con))
             {
                 cmd.CommandType = CommandType.Text;
                 con.Open();

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
                 con.Close();
             }
         }
     
     }



     public void inserePlanos(DataTable pCMW1,DataTable pCMW2) {

         foreach (DataRow row in pCMW1.Rows) importPlano(row);

         foreach (DataRow row in pCMW2.Rows) importPlano(row);

     }





    public DataTable getCMW1(){

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


        return prodCMW1;

    }

    public DataTable getCMW2()
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

        return prodCMW2;

    }
        




// ****************************************************************************************************************************************************
// **********************************************************  PRINT´s ********************************************************************************
// ****************************************************************************************************************************************************


     public void imprimeP()
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



     private void imprimeMachos()
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
         {
             foreach (DataRow row in table.Rows)
                 foreach (DataColumn column in table.Columns)
                     Console.WriteLine(row[column]);

         }


     }


     private void imprimeProd()
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








































    }

}
