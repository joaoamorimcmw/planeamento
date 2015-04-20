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
            peso += Convert.ToDecimal(row["Peso"]);
            return peso;
        }

        public int RemoveLinha(ref decimal pesoDisponivel, ref decimal pesoVazado)
        {
            DataRow next = lista.First.Value;
            decimal pesoNext = Convert.ToDecimal(next["Peso"]);
            int id = Convert.ToInt32(next["Id"]);
            if (pesoDisponivel >= pesoNext)
            {
                lista.RemoveFirst();
                this.peso -= pesoNext;
                pesoDisponivel -= pesoNext;
                pesoVazado = pesoNext;
            }
            else
            {
                next["Peso"] = Convert.ToDecimal(next["Peso"]) - pesoDisponivel;
                this.peso -= pesoDisponivel;
                pesoVazado = pesoDisponivel;
                pesoDisponivel = 0;
            }

            return id;
        }

        public int IdMaisRecente()
        {
            if (lista.Count == 0)
                return Int32.MaxValue;
            return Convert.ToInt32(lista.First.Value["Id"]);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            LigaFusao outro = obj as LigaFusao;
            if (outro != null){
                int compById = this.IdMaisRecente().CompareTo(outro.IdMaisRecente());
                return compById == 0 ? this.Peso.CompareTo(outro.Peso) : compById;
            }
                
            else
                throw new ArgumentException("Objeto não é uma LigaFusao");
        }
    }
}
