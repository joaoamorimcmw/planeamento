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
            this.label1 = new System.Windows.Forms.Label();
            this.boxFase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.boxSemana = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.boxDia = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.boxTurno = new System.Windows.Forms.ComboBox();
            this.resultadosView = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.boxLocal = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.resultadosView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fase:";
            // 
            // boxFase
            // 
            this.boxFase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxFase.FormattingEnabled = true;
            this.boxFase.Location = new System.Drawing.Point(51, 11);
            this.boxFase.Name = "boxFase";
            this.boxFase.Size = new System.Drawing.Size(110, 21);
            this.boxFase.TabIndex = 1;
            this.boxFase.SelectedIndexChanged += new System.EventHandler(this.boxFase_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(277, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Semana:";
            // 
            // boxSemana
            // 
            this.boxSemana.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxSemana.FormattingEnabled = true;
            this.boxSemana.Location = new System.Drawing.Point(332, 11);
            this.boxSemana.Name = "boxSemana";
            this.boxSemana.Size = new System.Drawing.Size(50, 21);
            this.boxSemana.TabIndex = 3;
            this.boxSemana.SelectedIndexChanged += new System.EventHandler(this.boxSemana_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Dia:";
            // 
            // boxDia
            // 
            this.boxDia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxDia.FormattingEnabled = true;
            this.boxDia.Location = new System.Drawing.Point(420, 11);
            this.boxDia.Name = "boxDia";
            this.boxDia.Size = new System.Drawing.Size(97, 21);
            this.boxDia.TabIndex = 5;
            this.boxDia.SelectedIndexChanged += new System.EventHandler(this.boxDia_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(523, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Turno:";
            // 
            // boxTurno
            // 
            this.boxTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxTurno.FormattingEnabled = true;
            this.boxTurno.Location = new System.Drawing.Point(567, 12);
            this.boxTurno.Name = "boxTurno";
            this.boxTurno.Size = new System.Drawing.Size(38, 21);
            this.boxTurno.TabIndex = 7;
            this.boxTurno.SelectedIndexChanged += new System.EventHandler(this.boxTurno_SelectedIndexChanged);
            // 
            // resultadosView
            // 
            this.resultadosView.AllowUserToAddRows = false;
            this.resultadosView.AllowUserToDeleteRows = false;
            this.resultadosView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultadosView.Location = new System.Drawing.Point(15, 47);
            this.resultadosView.Name = "resultadosView";
            this.resultadosView.ReadOnly = true;
            this.resultadosView.RowHeadersVisible = false;
            this.resultadosView.Size = new System.Drawing.Size(591, 306);
            this.resultadosView.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(167, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Local:";
            // 
            // boxLocal
            // 
            this.boxLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxLocal.FormattingEnabled = true;
            this.boxLocal.Location = new System.Drawing.Point(209, 11);
            this.boxLocal.Name = "boxLocal";
            this.boxLocal.Size = new System.Drawing.Size(62, 21);
            this.boxLocal.TabIndex = 10;
            this.boxLocal.SelectedIndexChanged += new System.EventHandler(this.boxLocal_SelectedIndexChanged);
            // 
            // Resultados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 365);
            this.Controls.Add(this.boxLocal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.resultadosView);
            this.Controls.Add(this.boxTurno);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.boxDia);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.boxSemana);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.boxFase);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Resultados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resultados";
            ((System.ComponentModel.ISupportInitialize)(this.resultadosView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox boxFase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox boxSemana;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox boxDia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox boxTurno;
        private System.Windows.Forms.DataGridView resultadosView;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox boxLocal;
    }
}