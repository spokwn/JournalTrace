namespace JournalTrace.View.Layout
{
    partial class GridLayout
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.datagJournalEntries = new System.Windows.Forms.DataGridView();
            this.comboSearch = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.btSearchClear = new System.Windows.Forms.Button();
            this.CBdeleted = new System.Windows.Forms.CheckBox();
            this.CBrenamednew = new System.Windows.Forms.CheckBox();
            this.CBstreamchange = new System.Windows.Forms.CheckBox();
            this.CBbasicinfochange = new System.Windows.Forms.CheckBox();
            this.CBrenamedold = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.datagJournalEntries)).BeginInit();
            this.SuspendLayout();
            // 
            // datagJournalEntries
            // 
            this.datagJournalEntries.AllowUserToDeleteRows = false;
            this.datagJournalEntries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datagJournalEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagJournalEntries.Location = new System.Drawing.Point(3, 41);
            this.datagJournalEntries.Name = "datagJournalEntries";
            this.datagJournalEntries.ReadOnly = true;
            this.datagJournalEntries.RowHeadersVisible = false;
            this.datagJournalEntries.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.datagJournalEntries.Size = new System.Drawing.Size(704, 324);
            this.datagJournalEntries.TabIndex = 10;
            this.datagJournalEntries.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datagJournalEntries_CellMouseDown);
            // 
            // comboSearch
            // 
            this.comboSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSearch.FormattingEnabled = true;
            this.comboSearch.Items.AddRange(new object[] {
            "USN",
            "Nome",
            "Hora",
            "Razão",
            "Diretório"});
            this.comboSearch.Location = new System.Drawing.Point(493, 6);
            this.comboSearch.Name = "comboSearch";
            this.comboSearch.Size = new System.Drawing.Size(112, 21);
            this.comboSearch.TabIndex = 11;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(49, 20);
            this.txtSearch.TabIndex = 12;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btSearch
            // 
            this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSearch.Location = new System.Drawing.Point(611, 6);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(49, 23);
            this.btSearch.TabIndex = 13;
            this.btSearch.Tag = "search";
            this.btSearch.Text = "search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // btSearchClear
            // 
            this.btSearchClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSearchClear.Location = new System.Drawing.Point(666, 6);
            this.btSearchClear.Name = "btSearchClear";
            this.btSearchClear.Size = new System.Drawing.Size(41, 23);
            this.btSearchClear.TabIndex = 14;
            this.btSearchClear.Tag = "clear";
            this.btSearchClear.Text = "clear";
            this.btSearchClear.UseVisualStyleBackColor = true;
            this.btSearchClear.Click += new System.EventHandler(this.btSearchClear_Click);
            // 
            // CBdeleted
            // 
            this.CBdeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBdeleted.AutoSize = true;
            this.CBdeleted.Location = new System.Drawing.Point(62, 8);
            this.CBdeleted.Name = "CBdeleted";
            this.CBdeleted.Size = new System.Drawing.Size(61, 17);
            this.CBdeleted.TabIndex = 15;
            this.CBdeleted.Text = "deleted";
            this.CBdeleted.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CBdeleted.UseVisualStyleBackColor = true;
            this.CBdeleted.CheckedChanged += new System.EventHandler(this.CBdeleted_CheckedChanged);
            // 
            // CBrenamednew
            // 
            this.CBrenamednew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBrenamednew.AutoSize = true;
            this.CBrenamednew.Location = new System.Drawing.Point(118, 8);
            this.CBrenamednew.Name = "CBrenamednew";
            this.CBrenamednew.Size = new System.Drawing.Size(93, 17);
            this.CBrenamednew.TabIndex = 17;
            this.CBrenamednew.Text = "renamed: new";
            this.CBrenamednew.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CBrenamednew.UseVisualStyleBackColor = true;
            this.CBrenamednew.CheckedChanged += new System.EventHandler(this.CBrenamed_CheckedChanged);
            // 
            // CBstreamchange
            // 
            this.CBstreamchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBstreamchange.AutoSize = true;
            this.CBstreamchange.Location = new System.Drawing.Point(402, 8);
            this.CBstreamchange.Margin = new System.Windows.Forms.Padding(0);
            this.CBstreamchange.Name = "CBstreamchange";
            this.CBstreamchange.Size = new System.Drawing.Size(96, 17);
            this.CBstreamchange.TabIndex = 18;
            this.CBstreamchange.Text = "stream change";
            this.CBstreamchange.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CBstreamchange.UseVisualStyleBackColor = true;
            this.CBstreamchange.CheckedChanged += new System.EventHandler(this.CBdataextend_CheckedChanged);
            // 
            // CBbasicinfochange
            // 
            this.CBbasicinfochange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBbasicinfochange.AutoSize = true;
            this.CBbasicinfochange.Location = new System.Drawing.Point(292, 8);
            this.CBbasicinfochange.Margin = new System.Windows.Forms.Padding(0);
            this.CBbasicinfochange.Name = "CBbasicinfochange";
            this.CBbasicinfochange.Size = new System.Drawing.Size(110, 17);
            this.CBbasicinfochange.TabIndex = 19;
            this.CBbasicinfochange.Text = "basic info change";
            this.CBbasicinfochange.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CBbasicinfochange.UseVisualStyleBackColor = true;
            this.CBbasicinfochange.CheckedChanged += new System.EventHandler(this.CBbasicinfochange_CheckedChanged);
            // 
            // CBrenamedold
            // 
            this.CBrenamedold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBrenamedold.AutoSize = true;
            this.CBrenamedold.Location = new System.Drawing.Point(208, 8);
            this.CBrenamedold.Name = "CBrenamedold";
            this.CBrenamedold.Size = new System.Drawing.Size(90, 17);
            this.CBrenamedold.TabIndex = 20;
            this.CBrenamedold.Text = "renamed:  old";
            this.CBrenamedold.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CBrenamedold.UseVisualStyleBackColor = true;
            this.CBrenamedold.CheckedChanged += new System.EventHandler(this.CBrenamedold_CheckedChanged);
            // 
            // GridLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboSearch);
            this.Controls.Add(this.CBstreamchange);
            this.Controls.Add(this.CBbasicinfochange);
            this.Controls.Add(this.CBrenamedold);
            this.Controls.Add(this.CBrenamednew);
            this.Controls.Add(this.CBdeleted);
            this.Controls.Add(this.btSearchClear);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.datagJournalEntries);
            this.Name = "GridLayout";
            this.Size = new System.Drawing.Size(710, 368);
            ((System.ComponentModel.ISupportInitialize)(this.datagJournalEntries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView datagJournalEntries;
        private System.Windows.Forms.ComboBox comboSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Button btSearchClear;
        private System.Windows.Forms.CheckBox CBdeleted;
        private System.Windows.Forms.CheckBox CBrenamednew;
        private System.Windows.Forms.CheckBox CBstreamchange;
        private System.Windows.Forms.CheckBox CBbasicinfochange;
        private System.Windows.Forms.CheckBox CBrenamedold;
    }
}
