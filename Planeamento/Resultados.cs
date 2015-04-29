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
        private static string strMacharia = "Macharia";
        private static string strMoldacao = "Moldação";
        private static string strFusao = "Fusão";
        private static string strRebarbagem = "Rebarbagem";

        private static string[] diasSemana = { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira" };

        private bool actualizar;

        public Resultados()
        {
            actualizar = false;
            InitializeComponent();

            boxFase.Items.Add(strMacharia);
            boxFase.Items.Add(strMoldacao);
            boxFase.Items.Add(strFusao);
            boxFase.Items.Add(strRebarbagem);
            boxFase.SelectedIndex = 0;

            foreach (string dia in diasSemana)
                boxDia.Items.Add(dia);
            boxDia.SelectedIndex = 0;

            actualizar = true;
            ActualizaTabela();
        }

        private void ActualizaTabela()
        {
            String fase = boxFase.SelectedItem.ToString();

            if (fase == strMacharia)
                resultadosView.DataSource = ResultadosBD.GetMacharia(boxLocal.SelectedIndex + 1, boxSemana.SelectedIndex + 1, boxDia.SelectedIndex + 1);
            else if (fase == strMoldacao)
                resultadosView.DataSource = ResultadosBD.GetMoldacao(boxLocal.SelectedIndex + 1, boxSemana.SelectedIndex + 1, boxDia.SelectedIndex + 1, boxTurno.SelectedIndex + 1);
            else if (fase == strFusao)
                resultadosView.DataSource = ResultadosBD.GetFusao(boxLocal.SelectedIndex + 1, boxSemana.SelectedIndex + 1, boxDia.SelectedIndex + 1, boxTurno.SelectedIndex + 1);

            foreach (DataGridViewColumn column in resultadosView.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }


        private void boxFase_SelectedIndexChanged(object sender, EventArgs e)
        {
            String fase = boxFase.SelectedItem.ToString();

            bool stateActualizar = actualizar;
            actualizar = false;

            if (fase == strMacharia)
                ChangeBoxes(2, ResultadosBD.MaxSemanaMacharia(), 1);
            else if (fase == strMoldacao)
                ChangeBoxes(3, ResultadosBD.MaxSemanaMoldacao(), 3);
            else if (fase == strFusao)
                ChangeBoxes(2, ResultadosBD.MaxSemanaFusao(), 3);
            else
                ChangeBoxes(1, 10, 3);

            actualizar = stateActualizar;

            if (actualizar)
                ActualizaTabela();
        }

        private void ChangeBoxes(int nLocais, int semanaMax, int nTurnos)
        {
            FillBoxLocal(nLocais);
            FillBoxSemana(semanaMax);
            FillBoxTurno(nTurnos);
        }

        private void FillBoxLocal(int nLocais)
        {
            boxLocal.Items.Clear();

            if (nLocais == 1)
            {
                boxLocal.Enabled = false;
                return;
            }

            boxLocal.Enabled = true;

            if (nLocais == 3)
            {
                boxLocal.Items.Add("GF");
                boxLocal.Items.Add("IMF");
                boxLocal.Items.Add("Manual");
            }

            else
            {
                boxLocal.Items.Add("CMW 1");
                boxLocal.Items.Add("CMW 2");
            }

            boxLocal.SelectedIndex = 0;
        }

        private void FillBoxSemana(int semana)
        {
            boxSemana.Items.Clear();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            int semanaActual = dfi.Calendar.GetWeekOfYear(DateTime.Now,dfi.CalendarWeekRule,dfi.FirstDayOfWeek);

            for (int i = 1; i <= semana; i++)
                boxSemana.Items.Add(i + semanaActual);

            boxSemana.SelectedIndex = 0;
        }

        private void FillBoxTurno(int nTurnos)
        {
            boxTurno.Items.Clear();
            if (nTurnos == 1)
                boxTurno.Enabled = false;
            else
            {
                boxTurno.Enabled = true;
                for (int i = 1; i <= nTurnos; i++)
                    boxTurno.Items.Add(i);

                boxTurno.SelectedIndex = 0;
            }
        }

        private void boxLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabela();
        }

        private void boxSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabela();
        }

        private void boxDia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabela();
        }

        private void boxTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabela();
        }
    }
}
