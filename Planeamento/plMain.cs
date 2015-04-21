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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());

            /*Init.UpdateProdutos();
            Init.ExcluiProdutosBaixaCarga();
            Init.InicializaLigas();

            Macharia macharia = new Macharia();
            macharia.LimpaBDMacharia();
            macharia.Executa(1);
            macharia.Executa(2);
            macharia.LimpaTabelas();

            Moldacao moldacao = new Moldacao();
            moldacao.LimpaBDMoldacao();
            moldacao.Executa(1);
            moldacao.Executa(2);
            moldacao.Executa(3);
            moldacao.LimpaTabelas();

            Fusao fusao = new Fusao();
            fusao.LimpaBDFusao();
            fusao.Executa(1);
            fusao.Executa(2);
            fusao.EscreveBD();
            fusao.ListaProdutosEmFalta();
            fusao.LimpaTabelas();*/
        }
    }
}
