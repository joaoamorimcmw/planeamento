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
            pickerFusoesCMW1.Value = (decimal) ParametrosBD.GetParametro(ParametrosBD.FusoesTurnoTotalCMW1);
            pickerFusoesCMW2.Value = (decimal) ParametrosBD.GetParametro(ParametrosBD.FusoesTurnoTotalCMW2);

            pickerForno11.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno1CMW1);
            pickerForno12.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno2CMW1);
            pickerForno13.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno3CMW1);
            pickerForno14.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno4CMW1);

            pickerForno21.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno1CMW2);
            pickerForno22.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno2CMW2);
            pickerForno23.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno3CMW2);
            pickerForno24.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Forno4CMW2);

            pickerRebFuncionarios.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.FuncionariosTurnoRebarbagem);
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
            ParametrosBD.SetParametro(ParametrosBD.FusoesTurnoTotalCMW1, pickerFusoesCMW1.Value);
            ParametrosBD.SetParametro(ParametrosBD.FusoesTurnoTotalCMW2, pickerFusoesCMW2.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno1CMW1, pickerForno11.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno2CMW1, pickerForno12.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno3CMW1, pickerForno13.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno4CMW1, pickerForno14.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno1CMW2, pickerForno21.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno2CMW2, pickerForno22.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno3CMW2, pickerForno23.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno4CMW2, pickerForno24.Value);

            ParametrosBD.SetParametro(ParametrosBD.FuncionariosTurnoRebarbagem, pickerRebFuncionarios.Value);

            this.Dispose();
        }
    }
}
