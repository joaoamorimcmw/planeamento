namespace Planeamento
{
    partial class Resultados
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.boxSemana = new System.Windows.Forms.ComboBox();
            this.resultadosView = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.boxLocal = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ligasView = new System.Windows.Forms.DataGridView();
            this.lblCaixas1 = new System.Windows.Forms.Label();
            this.lblCaixas2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCaixas1 = new System.Windows.Forms.TextBox();
            this.txtCaixas2 = new System.Windows.Forms.TextBox();
            this.txtCaixas1dia = new System.Windows.Forms.TextBox();
            this.lblCaixas1dia = new System.Windows.Forms.Label();
            this.txtCaixas2dia = new System.Windows.Forms.TextBox();
            this.lblCaixas2dia = new System.Windows.Forms.Label();
            this.txtMachariaDia = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMacharia = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.resultadosView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ligasView)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Semana:";
            // 
            // boxSemana
            // 
            this.boxSemana.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxSemana.FormattingEnabled = true;
            this.boxSemana.Location = new System.Drawing.Point(214, 11);
            this.boxSemana.Name = "boxSemana";
            this.boxSemana.Size = new System.Drawing.Size(62, 21);
            this.boxSemana.TabIndex = 3;
            this.boxSemana.SelectedIndexChanged += new System.EventHandler(this.boxSemana_SelectedIndexChanged);
            // 
            // resultadosView
            // 
            this.resultadosView.AllowUserToAddRows = false;
            this.resultadosView.AllowUserToDeleteRows = false;
            this.resultadosView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultadosView.Location = new System.Drawing.Point(15, 64);
            this.resultadosView.Name = "resultadosView";
            this.resultadosView.ReadOnly = true;
            this.resultadosView.RowHeadersVisible = false;
            this.resultadosView.Size = new System.Drawing.Size(707, 371);
            this.resultadosView.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Local:";
            // 
            // boxLocal
            // 
            this.boxLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxLocal.FormattingEnabled = true;
            this.boxLocal.Location = new System.Drawing.Point(54, 11);
            this.boxLocal.Name = "boxLocal";
            this.boxLocal.Size = new System.Drawing.Size(99, 21);
            this.boxLocal.TabIndex = 10;
            this.boxLocal.SelectedIndexChanged += new System.EventHandler(this.boxLocal_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Produtos";
            // 
            // ligasView
            // 
            this.ligasView.AllowUserToAddRows = false;
            this.ligasView.AllowUserToDeleteRows = false;
            this.ligasView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ligasView.Location = new System.Drawing.Point(731, 64);
            this.ligasView.Name = "ligasView";
            this.ligasView.ReadOnly = true;
            this.ligasView.RowHeadersVisible = false;
            this.ligasView.Size = new System.Drawing.Size(192, 211);
            this.ligasView.TabIndex = 12;
            // 
            // lblCaixas1
            // 
            this.lblCaixas1.AutoSize = true;
            this.lblCaixas1.Location = new System.Drawing.Point(728, 340);
            this.lblCaixas1.Name = "lblCaixas1";
            this.lblCaixas1.Size = new System.Drawing.Size(58, 13);
            this.lblCaixas1.TabIndex = 13;
            this.lblCaixas1.Text = "Caixas GF:";
            // 
            // lblCaixas2
            // 
            this.lblCaixas2.AutoSize = true;
            this.lblCaixas2.Location = new System.Drawing.Point(728, 392);
            this.lblCaixas2.Name = "lblCaixas2";
            this.lblCaixas2.Size = new System.Drawing.Size(79, 13);
            this.lblCaixas2.TabIndex = 14;
            this.lblCaixas2.Text = "Caixas Manual:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(728, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Ligas";
            // 
            // txtCaixas1
            // 
            this.txtCaixas1.Location = new System.Drawing.Point(838, 337);
            this.txtCaixas1.Name = "txtCaixas1";
            this.txtCaixas1.ReadOnly = true;
            this.txtCaixas1.Size = new System.Drawing.Size(85, 20);
            this.txtCaixas1.TabIndex = 16;
            // 
            // txtCaixas2
            // 
            this.txtCaixas2.Location = new System.Drawing.Point(838, 389);
            this.txtCaixas2.Name = "txtCaixas2";
            this.txtCaixas2.ReadOnly = true;
            this.txtCaixas2.Size = new System.Drawing.Size(85, 20);
            this.txtCaixas2.TabIndex = 17;
            // 
            // txtCaixas1dia
            // 
            this.txtCaixas1dia.Location = new System.Drawing.Point(838, 363);
            this.txtCaixas1dia.Name = "txtCaixas1dia";
            this.txtCaixas1dia.ReadOnly = true;
            this.txtCaixas1dia.Size = new System.Drawing.Size(85, 20);
            this.txtCaixas1dia.TabIndex = 19;
            // 
            // lblCaixas1dia
            // 
            this.lblCaixas1dia.AutoSize = true;
            this.lblCaixas1dia.Location = new System.Drawing.Point(728, 366);
            this.lblCaixas1dia.Name = "lblCaixas1dia";
            this.lblCaixas1dia.Size = new System.Drawing.Size(58, 13);
            this.lblCaixas1dia.TabIndex = 18;
            this.lblCaixas1dia.Text = "Caixas GF:";
            // 
            // txtCaixas2dia
            // 
            this.txtCaixas2dia.Location = new System.Drawing.Point(838, 415);
            this.txtCaixas2dia.Name = "txtCaixas2dia";
            this.txtCaixas2dia.ReadOnly = true;
            this.txtCaixas2dia.Size = new System.Drawing.Size(85, 20);
            this.txtCaixas2dia.TabIndex = 21;
            // 
            // lblCaixas2dia
            // 
            this.lblCaixas2dia.AutoSize = true;
            this.lblCaixas2dia.Location = new System.Drawing.Point(728, 418);
            this.lblCaixas2dia.Name = "lblCaixas2dia";
            this.lblCaixas2dia.Size = new System.Drawing.Size(79, 13);
            this.lblCaixas2dia.TabIndex = 20;
            this.lblCaixas2dia.Text = "Caixas Manual:";
            // 
            // txtMachariaDia
            // 
            this.txtMachariaDia.Location = new System.Drawing.Point(838, 311);
            this.txtMachariaDia.Name = "txtMachariaDia";
            this.txtMachariaDia.ReadOnly = true;
            this.txtMachariaDia.Size = new System.Drawing.Size(85, 20);
            this.txtMachariaDia.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(728, 314);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Macharia / dia (min):";
            // 
            // txtMacharia
            // 
            this.txtMacharia.Location = new System.Drawing.Point(838, 285);
            this.txtMacharia.Name = "txtMacharia";
            this.txtMacharia.ReadOnly = true;
            this.txtMacharia.Size = new System.Drawing.Size(85, 20);
            this.txtMacharia.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(728, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Macharia (min):";
            // 
            // Resultados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 451);
            this.Controls.Add(this.txtMachariaDia);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMacharia);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtCaixas2dia);
            this.Controls.Add(this.lblCaixas2dia);
            this.Controls.Add(this.txtCaixas1dia);
            this.Controls.Add(this.lblCaixas1dia);
            this.Controls.Add(this.txtCaixas2);
            this.Controls.Add(this.txtCaixas1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblCaixas2);
            this.Controls.Add(this.lblCaixas1);
            this.Controls.Add(this.ligasView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.boxLocal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.resultadosView);
            this.Controls.Add(this.boxSemana);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Resultados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resultados";
            ((System.ComponentModel.ISupportInitialize)(this.resultadosView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ligasView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox boxSemana;
        private System.Windows.Forms.DataGridView resultadosView;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox boxLocal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ligasView;
        private System.Windows.Forms.Label lblCaixas1;
        private System.Windows.Forms.Label lblCaixas2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCaixas1;
        private System.Windows.Forms.TextBox txtCaixas2;
        private System.Windows.Forms.TextBox txtCaixas1dia;
        private System.Windows.Forms.Label lblCaixas1dia;
        private System.Windows.Forms.TextBox txtCaixas2dia;
        private System.Windows.Forms.Label lblCaixas2dia;
        private System.Windows.Forms.TextBox txtMachariaDia;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMacharia;
        private System.Windows.Forms.Label label8;
    }
}