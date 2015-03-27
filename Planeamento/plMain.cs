using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planeamento
{
    static class plMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {
            //***********************************      TAREFAS     ************************************
            // - Falta adicionar a restrição no PlanFusao do número de fusões por dia em cada fabrica
            // - Na PlanModacao adicionar a restrição que só pode ser moldada no mesmo dia ou superior aos machos estarem prontos
            //*****************************************************************************************

            BDInit bd = new BDInit();
            Macharia macharia = new Macharia();
            macharia.Executa();

            //aqui são preenchidos os 3 primeiros planeamentos de macharia, moldação, fusao
            //PrimeiroPlano pl = new PrimeiroPlano();

            //aqui são verificadas as precedências entre macharia,moldacao,fusao

        }
    }
}
