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

    class BDFusao
    {

        public SqlConnection connection;
        public String SQL;
        public DataSet fusaoCMW1;
        public DataSet fusaoCMW2;


        // ************************* CONSTRUTORES ***********************************

        public BDFusao()
        {

            initTab();
            fillCMW1();
            fillCMW2();
        }

        // ************************* Metodos ***********************************



        private void initTab() {

            //apaga todos os registos
            SqlConnection con2 = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
            SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Plan Fus]", con2);
            cmd2.CommandType = CommandType.Text;
            con2.Open();
            cmd2.ExecuteNonQuery();
            con2.Close();
        
        }


        private void fillCMW1()
        {

            abreBD();
            fusaoCMW1 = new DataSet();
            try
            {
                SQL = "SELECT semana,dia,liga, pesoTotal,qtd,pesoPeca,linha FROM Planeamento.dbo.[CMW$Plan Mold] WHERE local = 1 ORDER BY semana ASC,dia ASC";
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(fusaoCMW1);
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


        }


        private void fillCMW2()
        {

            abreBD();
            fusaoCMW2 = new DataSet();
            try
            {
                SQL = "SELECT semana,dia,liga, pesoTotal,qtd,pesoPeca,linha FROM Planeamento.dbo.[CMW$Plan Mold] WHERE local != 1 ORDER BY semana ASC,dia ASC";
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(fusaoCMW2);
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


        }


       

        public void inserePlano(DataTable cmw1, DataTable cmw2)
        {
     
            SqlConnection con = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");
            con.Open();


                foreach (DataRow row in cmw1.Rows)
                {


                    SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Plan Fus] (linha, semana, dia, liga, forno, cargaACC, carga,numfus,fabrica) VALUES(@linha,@semana,@dia, @liga,@forno,@cargaACC,@carga,@numfus,@fabrica)", con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@linha", Convert.ToInt32(row[6].ToString()));
                    cmd.Parameters.AddWithValue("@semana", Convert.ToInt32(row[0].ToString()));
                    cmd.Parameters.AddWithValue("@dia", Convert.ToInt32(row[1].ToString()));
                    cmd.Parameters.AddWithValue("@liga", row[2].ToString());
                    cmd.Parameters.AddWithValue("@forno", Convert.ToInt32(row[3].ToString()));
                    cmd.Parameters.AddWithValue("@cargaACC", Convert.ToInt32(row[4].ToString()));
                    cmd.Parameters.AddWithValue("@carga", Convert.ToInt32(row[5].ToString()));
                    cmd.Parameters.AddWithValue("@numfus", Convert.ToInt32(row[7].ToString()));
                    cmd.Parameters.AddWithValue("@fabrica", 1);

                    cmd.ExecuteNonQuery();
                    
                }

                foreach (DataRow row in cmw2.Rows)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Plan Fus] (linha, semana, dia, liga, forno, cargaACC, carga,numfus,fabrica) VALUES(@linha,@semana,@dia, @liga,@forno,@cargaACC,@carga,@numfus,@fabrica)", con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@linha", Convert.ToInt32(row[6].ToString()));
                    cmd.Parameters.AddWithValue("@semana", Convert.ToInt32(row[0].ToString()));
                    cmd.Parameters.AddWithValue("@dia", Convert.ToInt32(row[1].ToString()));
                    cmd.Parameters.AddWithValue("@liga", row[2].ToString());
                    cmd.Parameters.AddWithValue("@forno", Convert.ToInt32(row[3].ToString()));
                    cmd.Parameters.AddWithValue("@cargaACC", Convert.ToInt32(row[4].ToString()));
                    cmd.Parameters.AddWithValue("@carga", Convert.ToInt32(row[5].ToString()));
                    cmd.Parameters.AddWithValue("@numfus", Convert.ToInt32(row[7].ToString()));
                    cmd.Parameters.AddWithValue("@fabrica", 2);

                    cmd.ExecuteNonQuery();

                }





                con.Close();

        }




        public DataTable getFusCMW1() {
            return fusaoCMW1.Tables[0];
        }

        public DataTable getFusCMW2()
        {
            return fusaoCMW2.Tables[0];
        }



        private void abreBD()
        {
            connection = new SqlConnection("Server=Sibelius;Database=Planeamento;Trusted_Connection=True;");

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
                System.Console.WriteLine("Erro na conecção.");
            }
        }

        private void fechaBD()
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
                System.Console.WriteLine("Erro ao terminal conecção.");
            }

        }


        public void imprime()
        {

            Console.WriteLine("---- CMW1 ----");


            foreach (DataTable tabela in fusaoCMW1.Tables)
            foreach (DataRow row in tabela.Rows)
            {
                foreach (DataColumn column in tabela.Columns)
                    Console.Write(column.ToString() + " -" + row[column] + "|");


                Console.WriteLine(" ");
            }


            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            Console.WriteLine("---- CMW2 ----");


            foreach (DataTable tabela in fusaoCMW2.Tables)
                foreach (DataRow row in tabela.Rows)
            {
                foreach (DataColumn column in tabela.Columns)
                    Console.Write(column.ToString() + " -" + row[column] + "|");


                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");
            
        }
            

    }
}
