using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 
 * 
 */

namespace Planeamento
{
    public class Macharia
    {
        private DataTable MachosCMW1;
        private DataTable MachosCMW2;
        private DataTable PlanoCMW1;
        private DataTable PlanoCMW2;
        
        private int acc;
        private int dia;
        private int semana;

        /**** Parametros ****/

        private int CapacidadeCMW1;
        private int CapacidadeCMW2;
        private int Horario; //capacidade diária em minutos

        private static String SEM_MACHO = "M002204";

        /*********************/

        public Macharia()
        {
            MachosCMW1 = new DataTable("Machos CMW1"); //Contem informação sobre os machos a planear
            MachosCMW1.Columns.Add(new DataColumn("Id", typeof(int))); //Linha da tabela Produtos associada
            MachosCMW1.Columns.Add(new DataColumn("CodMach", typeof(String))); //Código do macho a planear
            MachosCMW1.Columns.Add(new DataColumn("Qtd", typeof(decimal))); //Quantidade do macho a produzir
            MachosCMW1.Columns.Add(new DataColumn("Tempo", typeof(decimal))); //Tempo que demora 1 macho a produzir (em minutos)

            MachosCMW2 = new DataTable("Machos CMW2");
            MachosCMW2.Columns.Add(new DataColumn("Id", typeof(int)));
            MachosCMW2.Columns.Add(new DataColumn("CodMach", typeof(String)));
            MachosCMW2.Columns.Add(new DataColumn("Qtd", typeof(decimal)));
            MachosCMW2.Columns.Add(new DataColumn("Tempo", typeof(decimal)));

            PlanoCMW1 = new DataTable("Plano CMW1"); //Plano de produção dos machos
            PlanoCMW1.Columns.Add(new DataColumn("Id", typeof(int))); //Linha da tabela Produtos associada
            PlanoCMW1.Columns.Add(new DataColumn("CodMach", typeof(String))); //Código do macho a produzir
            PlanoCMW1.Columns.Add(new DataColumn("Fabrica", typeof(int))); //Fábrica onde é produzido
            PlanoCMW1.Columns.Add(new DataColumn("Semana", typeof(int))); //Semana em que é produzido
            PlanoCMW1.Columns.Add(new DataColumn("Dia", typeof(int))); //Dia da semana em que é produzido
            PlanoCMW1.Columns.Add(new DataColumn("Qtd", typeof(decimal))); //Quantidade do macho a produzir
            PlanoCMW1.Columns.Add(new DataColumn("Tempo", typeof(int))); //Tempo total que demora a produzir
            PlanoCMW1.Columns.Add(new DataColumn("Acc", typeof(int))); //Tempo acumulado diário no final

            PlanoCMW2 = new DataTable("Plano CMW2");
            PlanoCMW2.Columns.Add(new DataColumn("Id", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("CodMach", typeof(String)));
            PlanoCMW2.Columns.Add(new DataColumn("Fabrica", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Qtd", typeof(decimal)));
            PlanoCMW2.Columns.Add(new DataColumn("Tempo", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Acc", typeof(int)));
        }

        //Lê os parametros da BD
        public void GetParametros()
        {
            CapacidadeCMW1 = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Macharia1));
            CapacidadeCMW2 = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Macharia2));
            Horario = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Horario));
        }

        //Elimina plano antigo da Base de Dados.
        public void LimpaBDMacharia() 
        {
            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Macharia]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Macharia");
            connection.Close();
        }

        public void Executa(int Fabrica)
        {
            LeituraBD(Fabrica);
            Planeamento(Fabrica);
            EscreveBD(Fabrica);
        }

        //Lê da BD os machos associados a cada produto, e qual a quantidade total desse macho a produzir (qtdProduto * qtdMacho p/ produto).
        //Os produtos que nâo têm macho são actualizados directamente na tabela Produtos com DiaMacharia e SemanaMacharia = 0.
        private void LeituraBD(int Fabrica) 
        {
            List<int> produtosSemMacho = new List<int>();
            String query = "select " +
                "Prod.Id as Linha, " +
                "isnull(Bom.[No_],'" + SEM_MACHO + "') as Macho, " + //alguns produtos podem não ter os machos na lista de materiais, assume-se que não têm macho
                "isnull(Prod.QtdPendente*Bom.Quantity,0.0) as Quantidade, " +
                "isnull(Item.[Tempo Fabrico Machos]/60,0.0) as Tempo " + //Tempo está em segundos, dividir por 60
            "from " +
                "(select * from dbo.PlanCMW$Produtos where Include = 1 and dbo.GetFabrica(Local) = " + Fabrica + ") Prod " +
                "left join (select * from Navision.dbo.[CMW$Production BOM Line] where [No_] like 'M%') Bom " +
                    "on Prod.[NoProd] + '#' = Bom.[Production BOM No_] " + //importante o #, pois é na Gama Operatória do sub-produto que estão os machos
                "left join Navision.dbo.[CMW$Item] as Item " +
                    "on Bom.[No_] = Item.[No_] " +
            "order by Linha asc";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                String macho = reader["Macho"].ToString();
                if (macho == SEM_MACHO)
                    produtosSemMacho.Add(reader.GetInt32(0));
                else
                    InsereLinhaMacho (Convert.ToInt32(reader["Linha"]), macho, Convert.ToDecimal(reader["Quantidade"]), Convert.ToDecimal(reader["Tempo"]), Fabrica, connection);
            }

            reader.Close();

            //Tratamento dos produtos sem Macho.

            foreach (int id in produtosSemMacho)
                ActualizaSemMacho(id, connection);

            Console.WriteLine("CMW" + Fabrica +": " + produtosSemMacho.Count + " produtos sem macho actualizados");

            connection.Close();
        }

        //Insere cada macho a produzir na DataTable MachosCMW.
        private void InsereLinhaMacho(int Id, string CodMach, decimal Qtd, decimal Tempo, int Fabrica, SqlConnection connection)
        {
            DataRow dr;
            if (Fabrica == 1)
                dr = MachosCMW1.NewRow();
            else
                dr = MachosCMW2.NewRow();

            dr["Id"] = Id;
            dr["CodMach"] = CodMach;
            dr["Qtd"] = Qtd;
            dr["Tempo"] = Tempo;

            if (Fabrica == 1)
                MachosCMW1.Rows.Add(dr);
            else
                MachosCMW2.Rows.Add(dr);
        }

        //Actualiza a tabela Produtos na BD para os produtos que não têm Macho.
        private void ActualizaSemMacho(int Id, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Planeamento.dbo.[PlanCMW$Produtos] SET DiaMacharia = 0, SemanaMacharia = 0 WHERE Id = " + Id, connection);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        //Vai à BD buscar as capacidades de Macharia de ambas as fábricas.
        private void GetCapacidades()
        {
            SqlConnection connection = Util.AbreBD();
            
            SqlCommand cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Macharia CMW1'", connection);
            cmd.CommandType = CommandType.Text;
            CapacidadeCMW1 = (int)cmd.ExecuteScalar() * Horario;

            cmd = new SqlCommand("SELECT [Valor] FROM Planeamento.dbo.[CMW$Parametros] where [Parametro] = 'Capacidade Macharia CMW2'", connection);
            cmd.CommandType = CommandType.Text;
            CapacidadeCMW2 = (int)cmd.ExecuteScalar() * Horario;

            connection.Close();
        }

        private void Planeamento(int Local)
        {
            ResetGlobais();

            if(Local == 1)
                foreach (DataRow row in MachosCMW1.Rows)
                    LinhaPlaneamento(1, row,CapacidadeCMW1 * Horario);
            else
                foreach (DataRow row in MachosCMW2.Rows)
                    LinhaPlaneamento(2, row,CapacidadeCMW2 * Horario);
        }

        //Reinicia as variáveis globais.
        private void ResetGlobais()
        {
            acc = 0;
            dia = 1;
            semana = 1;
        }

        //Trata cada linha a planear.
        //2 casos: linha cabe toda no dia actual ou não.
        private void LinhaPlaneamento(int Fabrica, DataRow row, int Capacidade)
        {
            decimal qtd = (decimal) row["Qtd"];
            decimal tempo = (decimal) row ["Tempo"];
            int tempoTotal = (int) Math.Round(qtd * tempo);

            if (acc + tempoTotal > Capacidade) //Linha não cabe toda.
            {
                int espaco = Capacidade - acc;
                decimal qtdNova = Math.Floor ((decimal) espaco / tempo); //Verifica qual a quantidade que "cabe" no dia (arredonda para baixo).

                if (qtdNova > 0) //Tem espaço para produzir alguns no dia actual. Separa a linha em 2.
                {
                    int tempoNovo = (int) Math.Round (qtdNova * tempo);
                    acc += tempoNovo;
                    InsereLinhaPlaneamento((int) row["Id"], row["CodMach"].ToString(), Fabrica, qtdNova, tempoNovo);
                    Util.ProximoDia(ref dia, ref semana);
                    acc = 0;
                    row ["Qtd"] = qtd - qtdNova;
                    LinhaPlaneamento(Fabrica, row, Capacidade);
                }

                else //Não tem espaço para produzir no dia actual. Passa tudo para o dia seguinte.
                {
                    Util.ProximoDia (ref dia, ref semana);
                    acc = 0;
                    LinhaPlaneamento(Fabrica, row, Capacidade);
                }
            }

            else //Linha cabe toda no dia actual.
            {
                acc += tempoTotal;
                InsereLinhaPlaneamento((int) row["Id"], row["CodMach"].ToString(), Fabrica, qtd, tempoTotal);
            }
        }

        //Insere linha na tabela PlanoCMW
        private void InsereLinhaPlaneamento(int Id, String CodMach, int Fabrica, decimal Qtd, int Tempo)
        {
            DataRow row;

            if (Fabrica == 1)
                row = PlanoCMW1.NewRow();
            else
                row = PlanoCMW2.NewRow();

            row["Id"] = Id;
            row["CodMach"] = CodMach;
            row["Fabrica"] = Fabrica;
            row["Semana"] = semana;
            row["Dia"] = dia;
            row["Qtd"] = Qtd;
            row["Tempo"] = Tempo;
            row["Acc"] = acc;

            if (Fabrica == 1)
                PlanoCMW1.Rows.Add(row);
            else
                PlanoCMW2.Rows.Add(row);
        }

        //Escreve as linhas de plano de Macharia na tabela Macharia, e actualiza a tabela Produtos com o Dia e Semana da Macharia
        private void EscreveBD(int Fabrica)
        {
            DataTable Plano;
            if (Fabrica == 1)
                Plano = PlanoCMW1;
            else
                Plano = PlanoCMW2;

            SqlConnection connection = Util.AbreBD();
            int linhas = 0;

            foreach (DataRow row in Plano.Rows)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[PlanCMW$Macharia] VALUES(@Id,@CodMach,@Fabrica,@Semana,@Dia,@Qtd,@Tempo,@Acc)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", row["Id"]);
                cmd.Parameters.AddWithValue("@CodMach", row["CodMach"]);
                cmd.Parameters.AddWithValue("@Fabrica", row["Fabrica"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Qtd", row["Qtd"]);
                cmd.Parameters.AddWithValue("@Tempo", row["Tempo"]);
                cmd.Parameters.AddWithValue("@Acc", row["Acc"]);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE Planeamento.dbo.[PlanCMW$Produtos] SET DiaMacharia = @Dia, SemanaMacharia = @Semana WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@ID", row["Id"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.ExecuteNonQuery();
                linhas++;
            }

            Console.WriteLine("CMW" + Fabrica + ": " + linhas + " linhas inseridas na tabela Macharia");
        }

        public void LimpaTabelas()
        {
            MachosCMW1.Clear();
            MachosCMW2.Clear();
            PlanoCMW1.Clear();
            PlanoCMW2.Clear();
        }

    }
}
