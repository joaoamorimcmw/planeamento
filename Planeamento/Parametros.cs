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
        public Parametros()
        {
            InitializeComponent();

            for (int i = 1; i <= 3; i++)
            {
                boxTurnos1.Items.Add(i);
                boxTurnos2.Items.Add(i);
                boxTurnosReb.Items.Add(i);
            }

            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.lblMinFusao, "Percentagem mínima da capacidade do forno necessária para fazer uma fusão");
            toolTip1.SetToolTip(this.lblGitos, "Peso c/ gitos = (peso por peça) * (nº moldes p/ caixa) * (este valor)");

            pickerHoras.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.Horario) / 60;
            pickerGitos.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.PercentagemGitos);
            pickerMinFusao.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.MinimoFusao);

            boxTurnos1.SelectedItem = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.TurnosCMW1));
            boxTurnos2.SelectedItem = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.TurnosCMW2));
            boxTurnosReb.SelectedItem = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.TurnosRebarbagem));

            pickerMacharia1.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MachariaCMW1));
            pickerCaixasGF.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.CaixasGF));
            pickerFusoes1.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.FusoesCMW1));

            pickerForno11.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno1CMW1));
            pickerForno12.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno2CMW1));
            pickerForno13.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno3CMW1));
            pickerForno14.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno4CMW1));

            pickerMacharia2.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.MachariaCMW2));
            pickerCaixasIMF.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.CaixasIMF));
            pickerCaixasManual.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.CaixasManual));
            pickerFusoes2.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.FusoesCMW2));

            pickerForno21.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno1CMW2));
            pickerForno22.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno2CMW2));
            pickerForno23.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno3CMW2));
            pickerForno24.Value = Convert.ToInt32(ParametrosBD.GetParametro(ParametrosBD.Forno4CMW2));


            pickerRebFuncionarios.Value = (decimal)ParametrosBD.GetParametro(ParametrosBD.FuncionariosTurnoRebarbagem);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ParametrosBD.SetParametro(ParametrosBD.Horario, pickerHoras.Value * 60);
            ParametrosBD.SetParametro(ParametrosBD.PercentagemGitos, pickerGitos.Value);
            ParametrosBD.SetParametro(ParametrosBD.MinimoFusao, pickerMinFusao.Value);

            ParametrosBD.SetParametro(ParametrosBD.TurnosCMW1, boxTurnos1.SelectedItem);
            ParametrosBD.SetParametro(ParametrosBD.TurnosCMW2, boxTurnos2.SelectedItem);
            ParametrosBD.SetParametro(ParametrosBD.TurnosRebarbagem, boxTurnosReb.SelectedItem);

            ParametrosBD.SetParametro(ParametrosBD.MachariaCMW1, pickerMacharia1.Value);
            ParametrosBD.SetParametro(ParametrosBD.CaixasGF, pickerCaixasGF.Value);
            ParametrosBD.SetParametro(ParametrosBD.FusoesCMW1, pickerFusoes1.Value);

            ParametrosBD.SetParametro(ParametrosBD.Forno1CMW1, pickerForno11.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno2CMW1, pickerForno12.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno3CMW1, pickerForno13.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno4CMW1, pickerForno14.Value);

            ParametrosBD.SetParametro(ParametrosBD.MachariaCMW2, pickerMacharia2.Value);
            ParametrosBD.SetParametro(ParametrosBD.CaixasIMF, pickerCaixasIMF.Value);
            ParametrosBD.SetParametro(ParametrosBD.CaixasManual, pickerCaixasManual.Value);
            ParametrosBD.SetParametro(ParametrosBD.FusoesCMW2, pickerFusoes2.Value);

            ParametrosBD.SetParametro(ParametrosBD.Forno1CMW2, pickerForno21.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno2CMW2, pickerForno22.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno3CMW2, pickerForno23.Value);
            ParametrosBD.SetParametro(ParametrosBD.Forno4CMW2, pickerForno24.Value);

            //ParametrosBD.SetParametro(ParametrosBD.FuncionariosTurnoRebarbagem, pickerRebFuncionarios.Value);

            this.Dispose();
        }
    }
}
