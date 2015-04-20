using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planeamento
{
    static class plMain
    {
        [STAThread]
        static void Main()
        {
            /**
             * Todas as classes na pasta "Primeiro Plano" fazem parte do modelo antigo do Figueiredo.
             * A classe BDInit também tem algum código comentado que fazia parte desse modelo.
             */
            //BDInit bd = new BDInit();

            //Macharia macharia = new Macharia();
            //macharia.Executa();

            //Moldacao moldacao = new Moldacao();
            //moldacao.Executa();

            Fusao fusao = new Fusao();
            fusao.Executa();

            //Método antigo (Figueiredo)
            //PrimeiroPlano pl = new PrimeiroPlano();
        }
    }
}
