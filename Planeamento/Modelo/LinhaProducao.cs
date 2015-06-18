using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Representa uma linha de produção (produto,liga,peso c/gitos (por caixa),caixas,tempomachos (por caixa) e local)
 * 
 * */
namespace Planeamento
{
    public class LinhaProducao
    {
        private int id; //Id da linha na tabela de produtos

        public int Id
        {
            get { return id; }
        }

        private int local; //Local onde é produzido (1 - GF, 2 - IMF, 3 - Manual)

        public int Local
        {
            get { return local; }
        }

        private string liga; //Código da liga do produto

        public string Liga
        {
            get { return liga; }
        }

        private decimal pesoGitos; //Peso c/ gitos de 1 caixa

        public decimal PesoGitos
        {
            get { return pesoGitos; }
        }

        private int tempoMachos; //Tempo de macharia (em minutos) de 1 caixa

        public int TempoMachos
        {
            get { return tempoMachos; }
        }

        private int caixas; //Número de caixas desta linha (pode não ser igual ao número de caixas na encomenda)

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
