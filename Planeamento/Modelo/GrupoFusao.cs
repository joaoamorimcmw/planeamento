using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeamento
{
    public class GrupoFusao
    {
        private string liga;

        public string Liga
        {
            get { return liga; }
            set { liga = value; }
        }

        private List<LinhaProducao> lista;

        public List<LinhaProducao> Lista
        {
            get { return lista; }
        }

        private int idMaisPequeno;

        public int IdMaisPequeno
        {
            get { return idMaisPequeno; }
            set { idMaisPequeno = value; }
        }

        public decimal Peso
        {
            get
            {
                decimal peso = 0;

                foreach (LinhaProducao linha in lista)
                    peso += linha.Caixas * linha.PesoGitos;

                return peso;
            }
        }

        public int TempoMacharia
        {
            get
            {
                int tempo = 0;

                foreach (LinhaProducao linha in lista)
                    tempo += linha.Caixas * linha.TempoMachos;

                return tempo;
            }
        }

        public int Caixas
        {
            get
            {
                int caixas = 0;

                foreach (LinhaProducao linha in lista)
                    caixas += linha.Caixas;

                return caixas;
            }
        }

        public int CaixasIMF
        {
            get
            {
                int caixas = 0;

                foreach (LinhaProducao linha in lista)
                    if (linha.Local == 2)
                        caixas += linha.Caixas;

                return caixas;
            }
        }

        public int CaixasManual
        {
            get
            {
                int caixas = 0;

                foreach (LinhaProducao linha in lista)
                    if (linha.Local == 3)
                        caixas += linha.Caixas;

                return caixas;
            }
        }

        public GrupoFusao(string liga)
        {
            this.liga = liga;
            idMaisPequeno = Int32.MaxValue;
            lista = new List<LinhaProducao>();
        }

        public void AddLinha (LinhaProducao linha) {
            lista.Add(linha);
        }

        public void AddLinha (int id, decimal pesoGitos, int tempoMachos, int caixas) {
            LinhaProducao linha = new LinhaProducao(id, 1, liga, pesoGitos, tempoMachos, caixas);
            lista.Add(linha);

            if (idMaisPequeno == -1 || id < idMaisPequeno)
                idMaisPequeno = id;
        }

        public GrupoFusao Clone()
        {
            GrupoFusao grupo = new GrupoFusao(liga);
            grupo.IdMaisPequeno = this.idMaisPequeno;
            foreach (LinhaProducao linha in Lista)
                grupo.AddLinha(linha);
            return grupo;
        }
    }
}
