namespace Planeamento
{
    partial class Interface
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFuncoes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBtnActualizar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBtnParametros = new System.Windows.Forms.ToolStripMenuItem();
            this.resultadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.produtosView = new System.Windows.Forms.DataGridView();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.groupOpcoes = new System.Windows.Forms.GroupBox();
            this.chckExcluirCarga = new System.Windows.Forms.CheckBox();
            this.chckActLigas = new System.Windows.Forms.CheckBox();
            this.groupFases = new System.Windows.Forms.GroupBox();
            this.chckRebarbagem = new System.Windows.Forms.CheckBox();
            this.chckFusao = new System.Windows.Forms.CheckBox();
            this.chckMoldacao = new System.Windows.Forms.CheckBox();
            this.chckMacharia = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.produtosView)).BeginInit();
            this.groupOpcoes.SuspendLayout();
            this.groupFases.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFuncoes});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(798, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFuncoes
            // 
            this.menuFuncoes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBtnActualizar,
            this.menuBtnParametros,
            this.resultadosToolStripMenuItem});
            this.menuFuncoes.Name = "menuFuncoes";
            this.menuFuncoes.Size = new System.Drawing.Size(63, 20);
            this.menuFuncoes.Text = "Funções";
            // 
            // menuBtnActualizar
            // 
            this.menuBtnActualizar.Name = "menuBtnActualizar";
            this.menuBtnActualizar.Size = new System.Drawing.Size(177, 22);
            this.menuBtnActualizar.Text = "Actualizar Produtos";
            this.menuBtnActualizar.Click += new System.EventHandler(this.obterProdutosToolStripMenuItem_Click);
            // 
            // menuBtnParametros
            // 
            this.menuBtnParametros.Name = "menuBtnParametros";
            this.menuBtnParametros.Size = new System.Drawing.Size(177, 22);
            this.menuBtnParametros.Text = "Parametros";
            this.menuBtnParametros.Click += new System.EventHandler(this.menuBtnParametros_Click);
            // 
            // resultadosToolStripMenuItem
            // 
            this.resultadosToolStripMenuItem.Name = "resultadosToolStripMenuItem";
            this.resultadosToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.resultadosToolStripMenuItem.Text = "Resultados";
            this.resultadosToolStripMenuItem.Click += new System.EventHandler(this.resultadosToolStripMenuItem_Click);
            // 
            // produtosView
            // 
            this.produtosView.AllowUserToAddRows = false;
            this.produtosView.AllowUserToDeleteRows = false;
            this.produtosView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.produtosView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.produtosView.Location = new System.Drawing.Point(12, 27);
            this.produtosView.Name = "produtosView";
            this.produtosView.RowHeadersVisible = false;
            this.produtosView.Size = new System.Drawing.Size(567, 329);
            this.produtosView.TabIndex = 1;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIniciar.Location = new System.Drawing.Point(642, 318);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(104, 38);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupOpcoes
            // 
            this.groupOpcoes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOpcoes.Controls.Add(this.chckExcluirCarga);
            this.groupOpcoes.Controls.Add(this.chckActLigas);
            this.groupOpcoes.Location = new System.Drawing.Point(586, 27);
            this.groupOpcoes.Name = "groupOpcoes";
            this.groupOpcoes.Size = new System.Drawing.Size(200, 92);
            this.groupOpcoes.TabIndex = 3;
            this.groupOpcoes.TabStop = false;
            this.groupOpcoes.Text = "Opções";
            // 
            // chckExcluirCarga
            // 
            this.chckExcluirCarga.AutoSize = true;
            this.chckExcluirCarga.Checked = true;
            this.chckExcluirCarga.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chckExcluirCarga.Location = new System.Drawing.Point(7, 44);
            this.chckExcluirCarga.Name = "chckExcluirCarga";
            this.chckExcluirCarga.Size = new System.Drawing.Size(153, 17);
            this.chckExcluirCarga.TabIndex = 1;
            this.chckExcluirCarga.Text = "Excluir produtos sem carga";
            this.chckExcluirCarga.UseVisualStyleBackColor = true;
            // 
            // chckActLigas
            // 
            this.chckActLigas.AutoSize = true;
            this.chckActLigas.Location = new System.Drawing.Point(7, 20);
            this.chckActLigas.Name = "chckActLigas";
            this.chckActLigas.Size = new System.Drawing.Size(100, 17);
            this.chckActLigas.TabIndex = 0;
            this.chckActLigas.Text = "Actualizar Ligas";
            this.chckActLigas.UseVisualStyleBackColor = true;
            // 
            // groupFases
            // 
            this.groupFases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupFases.Controls.Add(this.chckRebarbagem);
            this.groupFases.Controls.Add(this.chckFusao);
            this.groupFases.Controls.Add(this.chckMoldacao);
            this.groupFases.Controls.Add(this.chckMacharia);
            this.groupFases.Location = new System.Drawing.Point(586, 125);
            this.groupFases.Name = "groupFases";
            this.groupFases.Size = new System.Drawing.Size(200, 120);
            this.groupFases.TabIndex = 4;
            this.groupFases.TabStop = false;
            this.groupFases.Text = "Fases";
            // 
            // chckRebarbagem
            // 
            this.chckRebarbagem.AutoSize = true;
            this.chckRebarbagem.Checked = true;
            this.chckRebarbagem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chckRebarbagem.Location = new System.Drawing.Point(7, 92);
            this.chckRebarbagem.Name = "chckRebarbagem";
            this.chckRebarbagem.Size = new System.Drawing.Size(87, 17);
            this.chckRebarbagem.TabIndex = 3;
            this.chckRebarbagem.Text = "Rebarbagem";
            this.chckRebarbagem.UseVisualStyleBackColor = true;
            // 
            // chckFusao
            // 
            this.chckFusao.AutoSize = true;
            this.chckFusao.Checked = true;
            this.chckFusao.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chckFusao.Location = new System.Drawing.Point(7, 68);
            this.chckFusao.Name = "chckFusao";
            this.chckFusao.Size = new System.Drawing.Size(55, 17);
            this.chckFusao.TabIndex = 2;
            this.chckFusao.Text = "Fusão";
            this.chckFusao.UseVisualStyleBackColor = true;
            // 
            // chckMoldacao
            // 
            this.chckMoldacao.AutoSize = true;
            this.chckMoldacao.Checked = true;
            this.chckMoldacao.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chckMoldacao.Location = new System.Drawing.Point(7, 44);
            this.chckMoldacao.Name = "chckMoldacao";
            this.chckMoldacao.Size = new System.Drawing.Size(73, 17);
            this.chckMoldacao.TabIndex = 1;
            this.chckMoldacao.Text = "Moldação";
            this.chckMoldacao.UseVisualStyleBackColor = true;
            // 
            // chckMacharia
            // 
            this.chckMacharia.AutoSize = true;
            this.chckMacharia.Checked = true;
            this.chckMacharia.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chckMacharia.Location = new System.Drawing.Point(7, 20);
            this.chckMacharia.Name = "chckMacharia";
            this.chckMacharia.Size = new System.Drawing.Size(70, 17);
            this.chckMacharia.TabIndex = 0;
            this.chckMacharia.Text = "Macharia";
            this.chckMacharia.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 359);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(798, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(162, 17);
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Margin = new System.Windows.Forms.Padding(5, 3, 1, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(400, 16);
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 381);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupFases);
            this.Controls.Add(this.groupOpcoes);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.produtosView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Interface";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Planeamento";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.produtosView)).EndInit();
            this.groupOpcoes.ResumeLayout(false);
            this.groupOpcoes.PerformLayout();
            this.groupFases.ResumeLayout(false);
            this.groupFases.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFuncoes;
        private System.Windows.Forms.ToolStripMenuItem menuBtnActualizar;
        private System.Windows.Forms.DataGridView produtosView;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.GroupBox groupOpcoes;
        private System.Windows.Forms.CheckBox chckExcluirCarga;
        private System.Windows.Forms.CheckBox chckActLigas;
        private System.Windows.Forms.GroupBox groupFases;
        private System.Windows.Forms.CheckBox chckRebarbagem;
        private System.Windows.Forms.CheckBox chckFusao;
        private System.Windows.Forms.CheckBox chckMoldacao;
        private System.Windows.Forms.CheckBox chckMacharia;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem menuBtnParametros;
        private System.Windows.Forms.ToolStripMenuItem resultadosToolStripMenuItem;
    }
}