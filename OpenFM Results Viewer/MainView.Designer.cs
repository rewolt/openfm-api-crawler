namespace OpenFM_Results_Viewer
{
    partial class MainView
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView = new System.Windows.Forms.TreeView();
            this.bt_refresh = new System.Windows.Forms.Button();
            this.bt_colapse = new System.Windows.Forms.Button();
            this.bt_expand = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Location = new System.Drawing.Point(13, 41);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(412, 463);
            this.treeView.TabIndex = 0;
            // 
            // bt_refresh
            // 
            this.bt_refresh.Location = new System.Drawing.Point(13, 12);
            this.bt_refresh.Name = "bt_refresh";
            this.bt_refresh.Size = new System.Drawing.Size(75, 23);
            this.bt_refresh.TabIndex = 1;
            this.bt_refresh.Text = "Refresh";
            this.bt_refresh.UseVisualStyleBackColor = true;
            this.bt_refresh.Click += new System.EventHandler(this.bt_refresh_Click);
            // 
            // bt_colapse
            // 
            this.bt_colapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_colapse.Location = new System.Drawing.Point(407, 12);
            this.bt_colapse.Name = "bt_colapse";
            this.bt_colapse.Size = new System.Drawing.Size(20, 23);
            this.bt_colapse.TabIndex = 2;
            this.bt_colapse.Text = "+";
            this.bt_colapse.UseVisualStyleBackColor = true;
            this.bt_colapse.Click += new System.EventHandler(this.bt_colapse_Click);
            // 
            // bt_expand
            // 
            this.bt_expand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_expand.Location = new System.Drawing.Point(381, 12);
            this.bt_expand.Name = "bt_expand";
            this.bt_expand.Size = new System.Drawing.Size(20, 23);
            this.bt_expand.TabIndex = 3;
            this.bt_expand.Text = "-";
            this.bt_expand.UseVisualStyleBackColor = true;
            this.bt_expand.Click += new System.EventHandler(this.bt_expand_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 516);
            this.Controls.Add(this.bt_expand);
            this.Controls.Add(this.bt_colapse);
            this.Controls.Add(this.bt_refresh);
            this.Controls.Add(this.treeView);
            this.Name = "MainView";
            this.Text = "OpenFM Songs";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button bt_refresh;
        private System.Windows.Forms.Button bt_colapse;
        private System.Windows.Forms.Button bt_expand;
    }
}

