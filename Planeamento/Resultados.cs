using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planeamento
{
    public partial class Resultados : Form
    {
        private static string strCMW1 = "CMW1";
        private static string strCMW2 = "CMW2";
        private static string strRebarbagem = "Rebarbagem";

        private static string strCaixasGF = "Caixas GF";
        private static string strCaixasIMF = "Caixas IMF";

        private bool actualizar;

        public Resultados()
        {
            actualizar = false;
            InitializeComponent();

            boxLocal.Items.Add(strCMW1);
            boxLocal.Items.Add(strCMW2);
            //boxLocal.Items.Add(strRebarbagem);
            boxLocal.SelectedIndex = 0;

            FillBoxSemana();
            ActualizaTabela();
            actualizar = true;
        }

        private void ActualizaTabela()
        {
            String local = boxLocal.SelectedItem.ToString();
            int semana = boxSemana.SelectedIndex + 1;

            if (local == strCMW1)
            {
                DataTable resultados = ResultadosBD.GetPlanoCMW1(semana);
                DataTable ligas = ResultadosBD.LigasCMW1(semana);
                int caixasGF = ResultadosBD.CaixasGF(semana);
                int macharia = ResultadosBD.MachariaCMW1(semana);
                ActualizaEcrã(resultados, ligas, false, strCaixasGF, macharia, caixasGF);
            }

            if (local == strCMW2)
            {
                DataTable resultados = ResultadosBD.GetPlanoCMW2(semana);
                DataTable ligas = ResultadosBD.LigasCMW2(semana);
                int caixasIMF = ResultadosBD.CaixasIMF(semana);
                int caixasManual = ResultadosBD.CaixasManual(semana);
                int macharia = ResultadosBD.MachariaCMW2(semana);
                ActualizaEcrã(resultados, ligas, true, strCaixasIMF, macharia, caixasIMF, caixasManual);
            }
        }

        private void ActualizaEcrã(DataTable resultados, DataTable ligas, bool caixasVisible, string textoCaixas,int tempoMacharia, int caixas1, int caixas2 = 0)
        {
            resultadosView.DataSource = resultados;
            foreach (DataGridViewColumn column in resultadosView.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            ligasView.DataSource = ligas;
            foreach (DataGridViewColumn column in ligasView.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            lblCaixas2.Visible = caixasVisible;
            txtCaixas2.Visible = caixasVisible;
            lblCaixas2dia.Visible = caixasVisible;
            txtCaixas2dia.Visible = caixasVisible;

            txtMacharia.Text = "" + tempoMacharia;
            txtMachariaDia.Text = "" + (tempoMacharia / 5);

            lblCaixas1.Text = textoCaixas + ":";
            txtCaixas1.Text = "" + caixas1;
            lblCaixas1dia.Text = textoCaixas + " / dia:";
            txtCaixas1dia.Text = "" + (caixas1 / 5);

            txtCaixas2.Text = "" + caixas2;
            txtCaixas2dia.Text = "" + (caixas2 / 5);
        }

        private void FillBoxSemana()
        {
            actualizar = false;

            int semana;
            String local = boxLocal.SelectedItem.ToString();

            if (local == strCMW1)
                semana = ResultadosBD.MaxSemanaCMW1();

            else
                semana = ResultadosBD.MaxSemanaCMW2();

            boxSemana.Items.Clear();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            int semanaActual = dfi.Calendar.GetWeekOfYear(DateTime.Now,dfi.CalendarWeekRule,dfi.FirstDayOfWeek);

            for (int i = 1; i <= semana; i++)
                boxSemana.Items.Add(i + semanaActual);

            if (boxSemana.Items.Count > 0)
                boxSemana.SelectedIndex = 0;

            actualizar = true;
        }

        private void boxSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabela();
        }

        private void boxLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBoxSemana();
            ActualizaTabela();              
        }
    }
}
