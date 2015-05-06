namespace Zeiterfassung
{
    partial class HistorieTaetigkeiten
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.grid_Taetigkeiten = new System.Windows.Forms.DataGridView();
            this.btn_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Taetigkeiten)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_Taetigkeiten
            // 
            this.grid_Taetigkeiten.AllowUserToAddRows = false;
            this.grid_Taetigkeiten.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grid_Taetigkeiten.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid_Taetigkeiten.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grid_Taetigkeiten.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Taetigkeiten.Location = new System.Drawing.Point(1, 1);
            this.grid_Taetigkeiten.Name = "grid_Taetigkeiten";
            this.grid_Taetigkeiten.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Taetigkeiten.Size = new System.Drawing.Size(959, 601);
            this.grid_Taetigkeiten.TabIndex = 0;
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Location = new System.Drawing.Point(1, 1);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(40, 23);
            this.btn_close.TabIndex = 1;
            this.btn_close.Text = "X";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // HistorieTaetigkeiten
            // 
            this.AcceptButton = this.btn_close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(962, 603);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.grid_Taetigkeiten);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HistorieTaetigkeiten";
            this.Opacity = 0.85D;
            this.Text = "Taetigkeiten";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.grid_Taetigkeiten)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_Taetigkeiten;
        private System.Windows.Forms.Button btn_close;
    }
}