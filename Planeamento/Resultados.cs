﻿using System;
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

        private static string strCaixasGF = "Caixas GF";
        private static string strCaixasIMF = "Caixas IMF";

        private bool actualizar;

        public Resultados()
        {
            actualizar = false;
            InitializeComponent();

            boxLocal.Items.Add(strCMW1);
            boxLocal.Items.Add(strCMW2);
            boxLocal.SelectedIndex = 0;

            FillBoxSemanaProducao();
            FillBoxSemana(boxSemanaRebarbagem, ResultadosBD.MaxSemana());

            ActualizaTabela();
            ActualizaTabelaRebarbagem();

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
                ActualizaEcra(resultados, ligas, false, strCaixasGF, macharia, caixasGF);
            }

            if (local == strCMW2)
            {
                DataTable resultados = ResultadosBD.GetPlanoCMW2(semana);
                DataTable ligas = ResultadosBD.LigasCMW2(semana);
                int caixasIMF = ResultadosBD.CaixasIMF(semana);
                int caixasManual = ResultadosBD.CaixasManual(semana);
                int macharia = ResultadosBD.MachariaCMW2(semana);
                ActualizaEcra(resultados, ligas, true, strCaixasIMF, macharia, caixasIMF, caixasManual);
            }
        }

        private void ActualizaEcra(DataTable resultados, DataTable ligas, bool caixasVisible, string textoCaixas,int tempoMacharia, int caixas1, int caixas2 = 0)
        {
            resultadosView.DataSource = resultados;
            foreach (DataGridViewColumn column in resultadosView.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            ligasView.DataSource = ligas;
            foreach (DataGridViewColumn column in ligasView.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            resultadosView.Columns[6].DefaultCellStyle.Format = "n2";
            ligasView.Columns[1].DefaultCellStyle.Format = "n0";

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

        private void ActualizaTabelaRebarbagem()
        {
            viewRebarbagem.DataSource = ResultadosBD.CargaRebarbagem(boxSemanaRebarbagem.SelectedIndex + 1);
            foreach (DataGridViewColumn column in viewRebarbagem.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            viewRebarbagem.Columns[2].DefaultCellStyle.Format = "n0";
            viewRebarbagem.Columns[3].DefaultCellStyle.Format = "n0";
            viewRebarbagem.Columns[4].DefaultCellStyle.Format = "n1";
        }

        private void FillBoxSemanaProducao()
        {
            actualizar = false;

            int semana;
            String local = boxLocal.SelectedItem.ToString();

            if (local == strCMW1)
                semana = ResultadosBD.MaxSemanaCMW1();

            else
                semana = ResultadosBD.MaxSemanaCMW2();

            FillBoxSemana(boxSemana, semana);

            actualizar = true;
        }

        private void FillBoxSemana(ComboBox box, int semana)
        {
            box.Items.Clear();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            int semanaActual = dfi.Calendar.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            for (int i = 1; i <= semana; i++)
                box.Items.Add(i + semanaActual);

            if (boxSemana.Items.Count > 0)
                box.SelectedIndex = 0;

        }

        private void boxSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabela();
        }

        private void boxLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBoxSemanaProducao();
            ActualizaTabela();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String encomenda = txtEncomenda.Text;
            if (!String.IsNullOrEmpty(encomenda))
            {
                viewEncomenda.DataSource = ResultadosBD.GetEncomenda(encomenda);
                foreach (DataGridViewColumn column in viewEncomenda.Columns)
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                viewEncomenda.Columns[5].DefaultCellStyle.Format = "n2";
            }
                
        }

        private void boxSemanaRebarbagem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizar)
                ActualizaTabelaRebarbagem();
        }

        private void txtEncomenda_Enter(object sender, EventArgs e)
        {
            ActiveForm.AcceptButton = button1;
        }

        private void txtEncomenda_Leave(object sender, EventArgs e)
        {
            ActiveForm.AcceptButton = null;
        }
    }
}
