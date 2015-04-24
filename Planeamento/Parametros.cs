using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planeamento
{
    public partial class Parametros : Form
    {
        private Macharia macharia;
        private Moldacao moldacao;
        private Fusao fusao;

        public Parametros(Macharia macharia, Moldacao moldacao, Fusao fusao)
        {
            InitializeComponent();
            this.macharia = macharia;
            this.moldacao = moldacao;
            this.fusao = fusao;

            boxTurnosGeral.Items.Add(1);
            boxTurnosGeral.Items.Add(2);
            boxTurnosGeral.Items.Add(3);

            pickerHoras.Value = (decimal) macharia.Horario / 60;
            boxTurnosGeral.SelectedIndex = moldacao.NTurnosGF - 1;

            pickerMacharia1.Value = macharia.CapacidadeCMW1;
            pickerMacharia2.Value = macharia.CapacidadeCMW2;

            pickerMoldacaoGF.Value = moldacao.CapacidadeGF;
            pickerMoldacaoIMF.Value = moldacao.CapacidadeIMF;
            pickerMoldacaoManual.Value = moldacao.CapacidadeManual;
        }
    }
}
