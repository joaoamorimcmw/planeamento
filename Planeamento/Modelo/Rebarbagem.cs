using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento.Modelo
{
    public class Rebarbagem
    {
        private DataTable Produtos;
        private DataTable Plano;

        private int semana;
        private int dia;
        private int turno;

        private int[] capacidadesPosto1;
        //...

        public Rebarbagem()
        {
            Produtos = new DataTable("Produtos");
            Produtos.Columns.Add(new DataColumn("Id", typeof(int)));
            Produtos.Columns.Add(new DataColumn("TempoPre", typeof(int)));
            Produtos.Columns.Add(new DataColumn("TempoExecucao", typeof(int)));
            Produtos.Columns.Add(new DataColumn("TempoPos", typeof(int)));

            Plano = new DataTable("Plano");
        }
    }
}
