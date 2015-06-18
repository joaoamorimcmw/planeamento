using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Esta classe representa um conjunto de produtos que vão ser resultado de uma só fusão
 * Tem associada uma liga, o ID mais pequeno (ou seja, qual o produto mais urgente), o peso total (c/ gitos), o tempo total de macharia, e o numero de caixas
 * 
 * */

namespace Planeamento
{
    public class GrupoFusao
    {

        #region Properties

        private string liga;

        public string Liga //Liga dos produtos do grupo
        {
            get { return liga; }
            set { liga = value; }
        }

        private List<LinhaProducao> lista;

        public List<LinhaProducao> Lista //Lista de produtos
        {
            get { return lista; }
        }

        private int idMaisPequeno; //Id do produto mais urgente

        public int IdMaisPequeno
        {
            get { return idMaisPequeno; }
            set { idMaisPequeno = value; }
        }

        public decimal Peso //Peso total com gitos
        {
            get
            {
                decimal peso = 0;

                foreach (LinhaProducao linha in lista)
                    peso += linha.Caixas * linha.PesoGitos;

                return peso;
            }
        }

        public int TempoMacharia //Tempo total de macharia em minutos
        {
            get
            {
                int tempo = 0;

                foreach (LinhaProducao linha in lista)
                    tempo += linha.Caixas * linha.TempoMachos;

                return tempo;
            }
        }

        public int Caixas //Total de caixas (independentemente de local)
        {
            get
            {
                int caixas = 0;

                foreach (LinhaProducao linha in lista)
                    caixas += linha.Caixas;

                return caixas;
            }
        }

        public int CaixasIMF //Total de caixas para IMF (local = 2)
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

        public int CaixasManual //Total de caixas para Manual (local = 3)
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

        #endregion

        public GrupoFusao(string liga)
        {
            this.liga = liga;
            idMaisPequeno = Int32.MaxValue;
            lista = new List<LinhaProducao>();
        }

        //Adicionar uma linha de produção com base numa linha já criada
        public void AddLinha (LinhaProducao linha) {
            lista.Add(linha);
        }

        //Adicionar uma linha de produção com base nos dados
        public void AddLinha (int id, decimal pesoGitos, int tempoMachos, int caixas, int local) {
            LinhaProducao linha = new LinhaProducao(id, local, liga, pesoGitos, tempoMachos, caixas);
            lista.Add(linha);

            if (idMaisPequeno == -1 || id < idMaisPequeno)
                idMaisPequeno = id;
        }

        //Faz uma cópia do objecto
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
