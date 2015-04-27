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

            pickerMinimoFusao.Value = 100 * fusao.Minimo;
            pickerFusoesTurno.Value = fusao.FusoesTurno;

            pickerForno11.Value = fusao.CapacidadesCMW1[1];
            pickerForno12.Value = fusao.CapacidadesCMW1[2];
            pickerForno13.Value = fusao.CapacidadesCMW1[3];
            pickerForno14.Value = fusao.CapacidadesCMW1[4];

            pickerForno21.Value = fusao.CapacidadesCMW2[1];
            pickerForno22.Value = fusao.CapacidadesCMW2[2];
            pickerForno23.Value = fusao.CapacidadesCMW2[3];
            pickerForno24.Value = fusao.CapacidadesCMW2[4];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            macharia.Horario = (int) pickerHoras.Value * 60;
            macharia.CapacidadeCMW1 = (int) pickerMacharia1.Value;
            macharia.CapacidadeCMW2 = (int) pickerMacharia2.Value;

            moldacao.CapacidadeGF = (int) pickerMoldacaoGF.Value;
            moldacao.CapacidadeIMF = (int) pickerMoldacaoIMF.Value;
            moldacao.CapacidadeManual = (int) pickerMoldacaoManual.Value;
            moldacao.NTurnosGF = (int) boxTurnosGeral.SelectedItem;
            moldacao.NTurnosIMF = (int) boxTurnosGeral.SelectedItem;
            moldacao.NTurnosManual = (int) boxTurnosGeral.SelectedItem;

            fusao.Minimo = pickerMinimoFusao.Value / 100;
            fusao.FusoesTurno = (int) pickerFusoesTurno.Value;
            fusao.TurnosCMW1 = (int) boxTurnosGeral.SelectedItem;
            fusao.TurnosCMW2 = (int) boxTurnosGeral.SelectedItem;

            fusao.SetCapacidadesCMW1(pickerForno11.Value, pickerForno12.Value, pickerForno13.Value, pickerForno14.Value);
            fusao.SetCapacidadesCMW2(pickerForno21.Value, pickerForno22.Value, pickerForno23.Value, pickerForno24.Value);

            this.Dispose();
        }
    }
}
