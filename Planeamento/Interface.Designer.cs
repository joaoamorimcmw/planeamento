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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.produtosView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFuncoes});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(957, 24);
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
            this.produtosView.Size = new System.Drawing.Size(878, 329);
            this.produtosView.TabIndex = 1;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIniciar.Location = new System.Drawing.Point(897, 318);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(48, 38);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 359);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(957, 22);
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
            this.progressBar.Size = new System.Drawing.Size(553, 16);
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 381);
            this.Controls.Add(this.statusStrip1);
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
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem menuBtnParametros;
        private System.Windows.Forms.ToolStripMenuItem resultadosToolStripMenuItem;
    }
}