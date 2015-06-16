using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class LinhaProducao
    {
        private int id;

        public int Id
        {
            get { return id; }
        }

        private int local;

        public int Local
        {
            get { return local; }
        }

        private string liga;

        public string Liga
        {
            get { return liga; }
        }

        private decimal pesoGitos;

        public decimal PesoGitos
        {
            get { return pesoGitos; }
        }

        private int tempoMachos;

        public int TempoMachos
        {
            get { return tempoMachos; }
        }

        private int caixas;

        public int Caixas
        {
            get { return caixas; }
            set { caixas = value; }
        }

        public LinhaProducao(int id, int local, string liga, decimal pesoGitos, int tempoMachos, int caixas)
        {
            this.id = id;
            this.local = local;
            this.liga = liga;
            this.pesoGitos = pesoGitos;
            this.tempoMachos = tempoMachos;
            this.caixas = caixas;
        }
    }
}
