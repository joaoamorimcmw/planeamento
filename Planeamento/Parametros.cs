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
        private Rebarbagem rebarbagem;

        public Parametros(Macharia macharia, Moldacao moldacao, Fusao fusao, Rebarbagem rebargagem)
        {
            InitializeComponent();
            this.macharia = macharia;
            this.moldacao = moldacao;
            this.fusao = fusao;
            this.rebarbagem = rebargagem;

            boxTurnosGeral.Items.Add(1);
            boxTurnosGeral.Items.Add(2);
            boxTurnosGeral.Items.Add(3);

            pickerHoras.Value = (decimal) ParametrosBD.GetParametro(ParametrosBD.Horario) / 60;
            boxTurnosGeral.SelectedIndex = (int) ((decimal) ParametrosBD.GetParametro(ParametrosBD.Turnos)) - 1;

            pickerMacharia1.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Macharia1);
            pickerMacharia2.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Macharia2);

            pickerMoldacaoGF.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.MoldacaoGF);
            pickerMoldacaoIMF.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.MoldacaoIMF);
            pickerMoldacaoManual.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.MoldacaoManual);

            pickerMinimoFusao.Value = 100 * (decimal)ParametrosBD.GetParametro(ParametrosBD.MinimoFusao);
            pickerFusoesTurno.Value = (decimal) ParametrosBD.GetParametro(ParametrosBD.FusoesTurnoForno);

            pickerForno11.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno1CMW1);
            pickerForno12.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno2CMW1);
            pickerForno13.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno3CMW1);
            pickerForno14.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno4CMW1);

            pickerForno21.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno1CMW2);
            pickerForno22.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno2CMW2);
            pickerForno23.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno3CMW2);
            pickerForno24.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno4CMW2);

            pickerRebFuncionarios.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.FuncionariosTurnoRebarbagem);

            /*pickerMacharia1.Value = macharia.CapacidadeCMW1;
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

            pickerRebFuncionarios.Value = rebarbagem.FuncionariosTurno;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParametrosBD.SetParametro(ParametrosBD.Horario, pickerHoras.Value * 60);
            ParametrosBD.SetParametro(ParametrosBD.Turnos, boxTurnosGeral.SelectedItem);

            ParametrosBD.SetParametro(ParametrosBD.Macharia1, pickerMacharia1.Value);
            ParametrosBD.SetParametro(ParametrosBD.Macharia2, pickerMacharia2.Value);

            ParametrosBD.SetParametro(ParametrosBD.MoldacaoGF, pickerMoldacaoGF.Value);
            ParametrosBD.SetParametro(ParametrosBD.MoldacaoIMF, pickerMoldacaoIMF.Value);
            ParametrosBD.SetParametro(ParametrosBD.MoldacaoManual, pickerMoldacaoManual.Value);

            ParametrosBD.SetParametro(ParametrosBD.MinimoFusao, pickerMinimoFusao.Value / 100);
            ParametrosBD.SetParametro(ParametrosBD.FusoesTurnoForno, pickerFusoesTurno.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno1CMW1, pickerForno11.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno2CMW1, pickerForno12.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno3CMW1, pickerForno13.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno4CMW1, pickerForno14.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno1CMW2, pickerForno21.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno2CMW2, pickerForno22.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno3CMW2, pickerForno23.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno4CMW2, pickerForno24.Value);

            ParametrosBD.SetParametro(ParametrosBD.FuncionariosTurnoRebarbagem, pickerRebFuncionarios.Value);

            /*macharia.Horario = (int) pickerHoras.Value * 60;
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

            rebarbagem.FuncionariosTurno = (int) pickerRebFuncionarios.Value;*/

            this.Dispose();
        }
    }
}
