using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    class LigaFusao : IComparable
    {
        private string liga;

        public string Liga
        {
            get { return liga; }
        }

        private string classe;

        public string Classe
        {
            get { return classe; }
        }

        private decimal peso;

        public decimal Peso
        {
            get { return peso; }
        }

        private LinkedList<DataRow> lista;

        public LinkedList<DataRow> Lista
        {
            get { return lista; }
        }

        public LigaFusao(string liga,string classe)
        {
            this.liga = liga;
            this.classe = classe;
            this.peso = 0;
            this.lista = new LinkedList<DataRow>();
        }

        public decimal AdicionaLinha(DataRow row)
        {
            lista.AddLast(row);
            peso += Convert.ToDecimal(row["Qtd"]) * Convert.ToDecimal(row["Peso"]);
            return peso;
        }

        public int IdMaisRecente()
        {
            return Convert.ToInt32(lista.First.Value["Id"]);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            LigaFusao outro = obj as LigaFusao;
            if (outro != null)
                return this.IdMaisRecente().CompareTo(outro.IdMaisRecente());
            else
                throw new ArgumentException("Objeto não é uma LigaFusao");
        }
    }
}
