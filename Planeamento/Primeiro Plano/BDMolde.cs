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
    class BDMolde
    {
        SqlConnection con;

        public BDMolde(DataTable pGF, DataTable pIMF, DataTable pMAN)
        {
            Inicializa();

            con = BDUtil.AbreBD();

            int linhas = 0;

            foreach (DataRow row in pGF.Rows)
                linhas += Escreve(row);

            foreach (DataRow row in pIMF.Rows)
                linhas += Escreve(row);

            foreach (DataRow row in pMAN.Rows)
                linhas += Escreve(row);

            BDUtil.FechaBD(con);

            Console.WriteLine(linhas + " linhas inseridas na tabela Plan Mold");
            pGF.Clear();
            pIMF.Clear();
            pMAN.Clear();
        }

        private void Inicializa()
        {
            SqlConnection connection = BDUtil.AbreBD();
            SqlCommand cmd2 = new SqlCommand("DELETE Planeamento.dbo.[CMW$Plan Mold]", connection);
            cmd2.CommandType = CommandType.Text;
            int linhas = cmd2.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Plan Mold");
            BDUtil.FechaBD(connection);
        }

        private int Escreve(DataRow row)
        {
            int res = GetLinhaGeral(row);

            SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[CMW$Plan Mold] (linha, local, noEnc, nolineEnc, noProd, qtd, liga, pesoPeca, noMolde, caixas,pesoTotal, caixasACC, pesoTotalACC,semana,dia) VALUES(@linha,@local,@noEnc,@nolineEnc,@noProd,@qtd,@liga,@pesoPeca,@noMolde,@caixas,@pesoTotal,@caixasACC,@pesoTotalACC,@semana,@dia)", con);

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@linha", res);
            cmd.Parameters.AddWithValue("@local", row[1]);
            cmd.Parameters.AddWithValue("@noEnc", row[2]);
            cmd.Parameters.AddWithValue("@nolineEnc", row[3]);
            cmd.Parameters.AddWithValue("@noProd", row[4]);
            cmd.Parameters.AddWithValue("@qtd", row[5]);
            cmd.Parameters.AddWithValue("@liga", row[6]);
            cmd.Parameters.AddWithValue("@pesoPeca", row[7]);
            cmd.Parameters.AddWithValue("@noMolde", row[8]);
            cmd.Parameters.AddWithValue("@caixas", row[9]);
            cmd.Parameters.AddWithValue("@pesoTotal", row[10]);
            cmd.Parameters.AddWithValue("@caixasACC", row[11]);
            cmd.Parameters.AddWithValue("@pesoTotalACC", row[12]);
            cmd.Parameters.AddWithValue("@semana", row[13]);
            cmd.Parameters.AddWithValue("@dia", row[14]);
            return cmd.ExecuteNonQuery();
        }

        private int GetLinhaGeral(DataRow row)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand("SELECT linha FROM Planeamento.dbo.[CMW$Numeracao] WHERE noEnc LIKE '" + row[2].ToString() + "' AND noLine=" + Convert.ToInt32(row[3].ToString()) + " AND noProd LIKE '" + row[4].ToString() + "'", con);
            cmd.CommandType = CommandType.Text;
            res = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            return res;
        }


        //pega na numeracao (linha) da moldacao e vai coloca-los na macharia para ficarem os 3 planos sincronizados com o mesmo numero
        /*public void adicionaNumPlanMacharia() {
            abreBD();
            numeracao = new DataSet();
            try
            {
                SQL = "SELECT linha,noEnc,nolineEnc,noProd FROM Planeamento.dbo.[CMW$Plan Mold]";
                SqlDataAdapter res = new SqlDataAdapter(SQL, connection);
                res.Fill(numeracao);
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
        }*/
    }

}
