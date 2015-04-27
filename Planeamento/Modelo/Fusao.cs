using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class Fusao
    {
        private DataTable FusaoCMW1;
        private DataTable FusaoCMW2;
        private DataTable PlanoCMW1;
        private DataTable PlanoCMW2;
        private DataTable Produtos;

        private int semana;
        private int dia;
        private int turno;

        /**** Parametros ****/

        private int turnosCMW1 = 3;

        public int TurnosCMW1
        {
            get { return turnosCMW1; }
            set { turnosCMW1 = value; }
        }

        private int turnosCMW2 = 3;

        public int TurnosCMW2
        {
            get { return turnosCMW2; }
            set { turnosCMW2 = value; }
        }

        private int fusoesTurno = 2;

        public int FusoesTurno
        {
            get { return fusoesTurno; }
            set { fusoesTurno = value; }
        }

        private decimal minimo = 0.66M;

        public decimal Minimo
        {
            get { return minimo; }
            set { minimo = value; }
        }

        private Dictionary<Int32, Decimal> capacidadesCMW1;

        public Dictionary<Int32, Decimal> CapacidadesCMW1
        {
            get { return capacidadesCMW1; }
        }

        private Dictionary<Int32, Decimal> capacidadesCMW2;

        public Dictionary<Int32, Decimal> CapacidadesCMW2
        {
            get { return capacidadesCMW2; }
        }


        /*********************/

        public Fusao()
        {
            FusaoCMW1 = new DataTable("Fusao CMW1");
            FusaoCMW1.Columns.Add(new DataColumn("Id", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("Liga", typeof(string)));
            FusaoCMW1.Columns.Add(new DataColumn("Classe", typeof(string)));
            FusaoCMW1.Columns.Add(new DataColumn("Peso", typeof(decimal)));
            FusaoCMW1.Columns.Add(new DataColumn("SemanaMoldacao", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("DiaMoldacao", typeof(int)));
            FusaoCMW1.Columns.Add(new DataColumn("TurnoMoldacao", typeof(int)));

            FusaoCMW2 = new DataTable("Fusao CMW2");
            FusaoCMW2.Columns.Add(new DataColumn("Id", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("Liga", typeof(string)));
            FusaoCMW2.Columns.Add(new DataColumn("Classe", typeof(string)));
            FusaoCMW2.Columns.Add(new DataColumn("Peso", typeof(decimal)));
            FusaoCMW2.Columns.Add(new DataColumn("SemanaMoldacao", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("DiaMoldacao", typeof(int)));
            FusaoCMW2.Columns.Add(new DataColumn("TurnoMoldacao", typeof(int)));

            PlanoCMW1 = new DataTable("Plano CMW1");
            PlanoCMW1.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Fabrica", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Forno", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("NoFusao", typeof(int)));
            PlanoCMW1.Columns.Add(new DataColumn("Liga", typeof(string)));
            PlanoCMW1.Columns.Add(new DataColumn("PesoTotal", typeof(decimal)));

            PlanoCMW2 = new DataTable("Plano CMW2");
            PlanoCMW2.Columns.Add(new DataColumn("Semana", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Dia", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Turno", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Fabrica", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Forno", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("NoFusao", typeof(int)));
            PlanoCMW2.Columns.Add(new DataColumn("Liga", typeof(string)));
            PlanoCMW2.Columns.Add(new DataColumn("PesoTotal", typeof(decimal)));

            Produtos = new DataTable("Produtos");
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Semana", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Dia", typeof(int)));
            Produtos.Columns.Add(new DataColumn("Turno", typeof(int)));
            Produtos.Columns.Add(new DataColumn("PesoVazadas", typeof(decimal)));

            capacidadesCMW1 = new Dictionary<Int32, Decimal>();
            capacidadesCMW2 = new Dictionary<Int32, Decimal>();

            SetCapacidadesCMW1(3000, 3000, 1000, 1000);
            SetCapacidadesCMW2(1750, 1100, 1100, 800);

        }

        public void SetCapacidadesCMW1(decimal forno1, decimal forno2, decimal forno3, decimal forno4)
        {
            capacidadesCMW1.Clear();
            capacidadesCMW1.Add(1, forno1);
            capacidadesCMW1.Add(2, forno2);
            capacidadesCMW1.Add(3, forno3);
            capacidadesCMW1.Add(4, forno4);
            capacidadesCMW1 = capacidadesCMW1.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public void SetCapacidadesCMW2(decimal forno1, decimal forno2, decimal forno3, decimal forno4)
        {
            capacidadesCMW2.Clear();
            capacidadesCMW2.Add(1, forno1);
            capacidadesCMW2.Add(2, forno2);
            capacidadesCMW2.Add(3, forno3);
            capacidadesCMW2.Add(4, forno4);
            capacidadesCMW2 = capacidadesCMW2.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public void LimpaBDFusao()
        {
            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;
            SqlCommand cmd = new SqlCommand("DELETE Planeamento.dbo.[PlanCMW$Fusao]", connection);
            cmd.CommandType = CommandType.Text;
            int linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas removidas da tabela Fusao");

            cmd = new SqlCommand("UPDATE dbo.[PlanCMW$Produtos] set PesoVazadas = 0", connection);
            linhas = cmd.ExecuteNonQuery();
            Console.WriteLine(linhas + " linhas actualizadas na tabela Produtos (PesoVazadas = 0)");

            connection.Close();
        }

        public void Executa(int Fabrica)
        {
            LeituraBD(Fabrica);
            Planeamento(Fabrica);
        }

        private void LeituraBD(int Fabrica)
        {
            String query = "select Prod.Id as Id,Prod.Liga as Liga,Ligas.[Codigo Classe] as Classe,QtdPendente,PesoPeca,SemanaMoldacao,DiaMoldacao,TurnoMoldacao " + 
            "from dbo.PlanCMW$Produtos Prod " + 
            "inner join dbo.PlanCMW$Ligas Ligas " +
            "on Prod.Liga = Ligas.Liga " +
            "where Include = 1 and dbo.GetFabrica(Local) = " + Fabrica + " order by Prod.Id asc";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DataRow row;

                if (Fabrica == 1)
                    row = FusaoCMW1.NewRow();
                else
                    row = FusaoCMW2.NewRow();

                row["Id"] = Convert.ToInt32(reader["Id"]);
                row["Liga"] = reader["Liga"].ToString();
                row["Classe"] = reader["Classe"].ToString();
                row["Peso"] = Convert.ToDecimal(reader["PesoPeca"]) * Convert.ToInt32(reader["QtdPendente"]);
                row["SemanaMoldacao"] = Convert.ToInt32(reader["SemanaMoldacao"]);
                row["DiaMoldacao"] = Convert.ToInt32(reader["DiaMoldacao"]);
                row["TurnoMoldacao"] = Convert.ToInt32(reader["TurnoMoldacao"]);

                if (Fabrica == 1)
                    FusaoCMW1.Rows.Add(row);
                else
                    FusaoCMW2.Rows.Add(row);
            }

            reader.Close();
            connection.Close();
        }

        private void Planeamento(int Local)
        {
            ResetGlobais();
            if (Local == 1)
                PlaneamentoLocal (Local,FusaoCMW1,TurnosCMW1,new Dictionary<string,LigaFusao>(),0);
            else
                PlaneamentoLocal (Local,FusaoCMW2,TurnosCMW2,new Dictionary<string,LigaFusao>(),0);
        }

        private void PlaneamentoLocal(int Local,DataTable Table, int nTurnos, Dictionary<String, LigaFusao> Cargas, int Index)
        {
            while (Index < Table.Rows.Count && RespeitaPrecendencia(Table.Rows[Index]))
            {
                DataRow row = Table.Rows[Index];
                string liga = row["Liga"].ToString();
                if (Cargas.ContainsKey(liga))
                    Cargas[liga].AdicionaLinha(row);
                else
                {
                    LigaFusao lFusao = new LigaFusao(liga, row["Classe"].ToString());
                    lFusao.AdicionaLinha(row);
                    Cargas.Add(liga, lFusao);
                }
                Index++;
            }

            /****** Para debug ******
            Console.WriteLine("Semana " + semana + " Dia " + dia + " Turno " + turno +"\n---------------------");
            foreach (String liga in Cargas.Keys)
            {
                Console.WriteLine(Cargas[liga].Liga + ": " + Cargas[liga].Peso + " kgs");
                foreach (DataRow row in Cargas[liga].Lista)
                    Console.WriteLine("\t" + row["Id"].ToString() + ": " + row["Peso"] + " kgs");
            }
                
            Console.WriteLine("---------------------"); */

            PlaneamentoTurno(Local,Cargas);
            bool continua = false;

            if (Index < Table.Rows.Count)
                continua = true;
            else {
                foreach (LigaFusao lFusao in Cargas.Values)
                    if (lFusao.Peso > FusaoMinima(Local))
                        continua = true;
            }

            if (continua)
            {
                Util.ProximoTurno(ref turno, ref dia, ref semana, nTurnos);
                PlaneamentoLocal(Local, Table, nTurnos, Cargas, Index);
            }
                
        }

        private void PlaneamentoTurno(int Local,Dictionary<String,LigaFusao> Cargas){
            Cargas = Cargas.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            
            decimal maiorCap, menorCap;
            if (Local == 1)
                maiorCap = capacidadesCMW1.Values.ToArray().Max() * Minimo;
            else
                maiorCap = capacidadesCMW2.Values.ToArray().Max() * Minimo;
            menorCap = FusaoMinima(Local);

            int possiveisMenor = 0, possiveisMaior = 0;

            List<LigaFusao> lista = new List<LigaFusao> ();
            foreach (LigaFusao liga in Cargas.Values) {
                if (liga.Peso >= maiorCap)
                    possiveisMaior++;
                if (liga.Peso >= menorCap)
                {
                    possiveisMenor++;
                    lista.Add(liga);
                }
                    
                if (possiveisMaior >= FusoesTurno && possiveisMenor >= FusoesTurno * 4)
                    break;
            }

            DistribuiFusoes(Local,lista);
        }

        private void DistribuiFusoes(int Local, List<LigaFusao> lista)
        {
            int [] fusoesFornos = { 0, 0, 0, 0 };
            int totalFusoes = 0;
            bool existeCarga = true;
            Dictionary<Int32, Decimal> caps;

            if (Local == 1)
                caps = capacidadesCMW1;

            else
                caps = capacidadesCMW2;

            while (totalFusoes < FusoesTurno * 4 && existeCarga)
            {
                lista = lista.OrderByDescending(x => x.Peso).ToList();
                existeCarga = false;
                foreach (LigaFusao liga in lista)
                {
                    bool stop = false;
                    foreach (int forno in caps.Keys)
                    {
                        if (!stop && liga.Peso >= caps[forno] * minimo && fusoesFornos[forno-1] < FusoesTurno)
                        {
                            fusoesFornos[forno - 1]++;
                            RegistaFusao(Local, liga, 1, fusoesFornos[forno - 1], Math.Min(liga.Peso, caps[forno]));
                            totalFusoes++;
                            existeCarga = true;
                            stop = true;
                        }
                    }
                }
            }
        }

        private bool RespeitaPrecendencia(DataRow row)
        {
            int semanaMoldacao = Convert.ToInt32(row["SemanaMoldacao"]);
            int diaMoldacao = Convert.ToInt32(row["DiaMoldacao"]);
            int turnoMoldacao = Convert.ToInt32(row["TurnoMoldacao"]);

            if (semana < semanaMoldacao)
                return false;

            if (semana == semanaMoldacao)
            {
                if (dia < diaMoldacao)
                    return false;
                if (dia == diaMoldacao)
                    return turno > turnoMoldacao;
            }

            return true;
        }

        private void RegistaFusao(int Local, LigaFusao liga, int Forno, int Fusao, decimal Peso)
        {
            //Console.WriteLine("Forno " + Forno + ": " + liga.Liga + " " + Peso + " kgs"); 
            decimal iPeso = Peso;
            while (iPeso > 0)
            {
                decimal pesoProduto = 0;
                int id = liga.RemoveLinha(ref iPeso, ref pesoProduto);
                RegistaProduto(id,pesoProduto);
            }

            DataRow row;
            if (Local == 1)
                row = PlanoCMW1.NewRow();
            else
                row = PlanoCMW2.NewRow();

            row["Semana"] = semana;
            row["Dia"] = dia;
            row["Turno"] = turno;
            row["Fabrica"] = Local;
            row["Forno"] = Forno;
            row["NoFusao"] = Fusao;
            row["Liga"] = liga.Liga;
            row["PesoTotal"] = Peso;

            if (Local == 1)
                PlanoCMW1.Rows.Add(row);
            else
                PlanoCMW2.Rows.Add(row);
        }

        private void RegistaProduto(int Id, decimal Peso)
        {
            DataRow row = Produtos.NewRow();
            row["Id"] = Id;
            row["Semana"] = semana;
            row["Dia"] = dia;
            row["Turno"] = turno;
            row["PesoVazadas"] = Peso;
            Produtos.Rows.Add(row);
        }

        private void ResetGlobais()
        {
            semana = 1;
            dia = 1;
            turno = 1;
        }

        private decimal PesoProdutos(LinkedList<DataRow> Lista)
        {
            decimal total = 0;

            foreach (DataRow row in Lista)
                total += Convert.ToDecimal(row["Peso"]);

            return total;
        }

        public void EscreveBD()
        {
            SqlConnection connection = Util.AbreBD();
            int linhas = 0;

            foreach (DataRow row in Produtos.Rows)
            {
                SqlCommand cmd = new SqlCommand("UPDATE Planeamento.dbo.[PlanCMW$Produtos] SET DiaFusao = @Dia, SemanaFusao = @Semana, TurnoFusao = @Turno, PesoVazadas = PesoVazadas + @PesoVazadas WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", row["Id"]);
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.Parameters.AddWithValue("@PesoVazadas", row["PesoVazadas"]);
                cmd.ExecuteNonQuery();

                linhas++;
            }

            Console.WriteLine(linhas + " produtos com fusão actualizados");

            SqlCommand command = new SqlCommand("UPDATE Planeamento.dbo.[PlanCMW$Produtos] SET QtdVazadas = floor(PesoVazadas / PesoPeca), PesoEmFalta = (QtdPendente * PesoPeca) - PesoVazadas", connection);
            command.ExecuteNonQuery();

            EscrevePlanoBD(1, connection);
            EscrevePlanoBD(2, connection);
        }

        private void EscrevePlanoBD(int Fabrica, SqlConnection connection)
        {
            DataTable Plano;

            if (Fabrica == 1)
                Plano = PlanoCMW1;
            else
                Plano = PlanoCMW2;

            int linhas = 0;
            foreach (DataRow row in Plano.Rows)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Planeamento.dbo.[PlanCMW$Fusao] VALUES(@Semana,@Dia,@Turno,@Fabrica,@Forno,@NoFusao,@Liga,@PesoTotal)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Semana", row["Semana"]);
                cmd.Parameters.AddWithValue("@Dia", row["Dia"]);
                cmd.Parameters.AddWithValue("@Turno", row["Turno"]);
                cmd.Parameters.AddWithValue("@Fabrica", row["Fabrica"]);
                cmd.Parameters.AddWithValue("@Forno", row["Forno"]);
                cmd.Parameters.AddWithValue("@NoFusao", row["NoFusao"]);
                cmd.Parameters.AddWithValue("@Liga", row["Liga"]);
                cmd.Parameters.AddWithValue("@PesoTotal", row["PesoTotal"]);
                cmd.ExecuteNonQuery();
                linhas++;
            }

            Console.WriteLine("CMW" + Fabrica + " : " + linhas + " inseridas na tabela Fusao");
        }

        public void ListaProdutosEmFalta()
        {
            String query = "select NoEnc,NoProd,floor(PesoEmFalta/PesoPeca) as QtdEmFalta from dbo.PlanCMW$Produtos Prod where PesoEmFalta > 0";

            SqlConnection connection = Util.AbreBD();
            if (connection == null)
                return;

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Produtos em falta (pouca carga)");
            Console.WriteLine("Encomenda\tProduto\tQtd em falta\n------------------------------------");
            while (reader.Read())
                Console.WriteLine(reader["NoEnc"] + "\t" + reader["NoProd"] + "\t" + reader["QtdEmFalta"]);

            reader.Close();
            connection.Close();
        }

        public void LimpaTabelas()
        {
            FusaoCMW1.Clear();
            FusaoCMW2.Clear();
            PlanoCMW1.Clear();
            PlanoCMW2.Clear();
            Produtos.Clear();            
        }

        public decimal FusaoMinima (int Fabrica){
            if (Fabrica == 1)
                return capacidadesCMW1.Values.ToArray().Min() * Minimo;
            else
                return capacidadesCMW2.Values.ToArray().Min() * Minimo;
        }
    }
}