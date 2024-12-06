using JournalTrace.Language;
using JournalTrace.Entry;
using System;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using JournalTrace.View.Util;

namespace JournalTrace.View.Layout
{
    public partial class GridLayout : UserControl, ILayout
    {
        private EntryManager entryManager;

        public GridLayout(EntryManager mngr)
        {
            this.entryManager = mngr;
            
            InitializeComponent();

            comboSearch.SelectedIndex = 1;
            //campos de tradução
            datagJournalEntries.Tag = new string[] { null, "name", "date", "reason", "directory" };
            comboSearch.Tag = new string[] { null, "name", "date", "reason", "directory" };
        }
        public void Clean()
        {
            datagJournalEntries.DataSource = null;
            dataSourceEntries.Clear();
            dataSourceEntries.Dispose();
            datagJournalEntries.Rows.Clear();
            datagJournalEntries.Dispose();
            GC.Collect();
        }

        public Control GetControl()
        {
            return this;
        }

        public DataTable dataSourceEntries;

        public async void LoadData(FormMain frm)
        {
            dataSourceEntries = new DataTable();

            dataSourceEntries.Columns.Add("USN", typeof(long));
            dataSourceEntries.Columns.Add("name", typeof(string));
            dataSourceEntries.Columns.Add("date", typeof(string));
            dataSourceEntries.Columns.Add("reason", typeof(string));
            dataSourceEntries.Columns.Add("directory", typeof(string));

            await Task.Run(() =>
            {
                foreach (var item in entryManager.USNEntries)
                {
                    USNEntry entry = item.Value;
                    dataSourceEntries.Rows.Add(entry.USN, entry.Name, entry.Time, entry.Reason, entryManager.parentFileReferenceIdentifiers[entry.ParentFileReference].ResolvedID);
                }
            });

            datagJournalEntries.DataSource = dataSourceEntries;

            LanguageManager.INSTANCE.UpdateControl(datagJournalEntries);

            //tamanho das colunas
            int[] widthColumns = new int[] { 88, 200, 115, 300, 500 };

            for (int i = 0; i < widthColumns.Length; i++)
            {
                datagJournalEntries.Columns[i].Width = widthColumns[i];
            }

            frm.ShowLayoutOption(true);
        }

        private void btSearch_Click(object sender, EventArgs e)
{
    string filterText = txtSearch.Text.Trim();
    string combinedFilter = "";

    string[] filterConditions;
    if (filterText.Contains(":"))
    {
        filterConditions = filterText.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
    }
    else
    {
        if (comboSearch.SelectedIndex <= 0)
        {
            dataSourceEntries.DefaultView.RowFilter = "";
            return;
        }
        string columnName = dataSourceEntries.Columns[comboSearch.SelectedIndex].ColumnName;
        filterConditions = new[] { $"{columnName}:{filterText}" };
    }

    foreach (string condition in filterConditions)
    {
        string[] parts = condition.Split(new[] { ':' }, 2);
        if (parts.Length == 2)
        {
            string columnName = parts[0].Trim();
            string filterValue = parts[1].Trim();

            if (dataSourceEntries.Columns.Contains(columnName))
            {
                string columnFilter = "";

                if (filterValue.Contains("!!"))
                {
                    string[] inclusionAndExclusion = filterValue.Split(new[] { "!!" }, StringSplitOptions.None);

                    if (!string.IsNullOrWhiteSpace(inclusionAndExclusion[0]))
                    {
                        columnFilter += $"{columnName} LIKE '%{inclusionAndExclusion[0].Trim()}%' AND ";
                    }

                    for (int i = 1; i < inclusionAndExclusion.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(inclusionAndExclusion[i]))
                        {
                            columnFilter += $"{columnName} NOT LIKE '%{inclusionAndExclusion[i].Trim()}%' AND ";
                        }
                    }

                    if (columnFilter.EndsWith(" AND "))
                    {
                        columnFilter = columnFilter.Substring(0, columnFilter.Length - 5);
                    }
                }
                else if (filterValue.Contains("&&"))
                {
                    string[] inclusionParts = filterValue.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string inclusion in inclusionParts)
                    {
                        columnFilter += $"{columnName} LIKE '%{inclusion.Trim()}%' AND ";
                    }

                    if (columnFilter.EndsWith(" AND "))
                    {
                        columnFilter = columnFilter.Substring(0, columnFilter.Length - 5);
                    }
                }
                else if (filterValue.Contains("||"))
                {
                    string[] orParts = filterValue.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string orPart in orParts)
                    {
                        columnFilter += $"{columnName} LIKE '%{orPart.Trim()}%' OR ";
                    }

                    if (columnFilter.EndsWith(" OR "))
                    {
                        columnFilter = columnFilter.Substring(0, columnFilter.Length - 4);
                    }
                }
                else
                {
                    columnFilter = $"{columnName} LIKE '%{filterValue}%'";
                }

                combinedFilter += "(" + columnFilter + ") AND ";
            }
        }
    }

    if (combinedFilter.EndsWith(" AND "))
    {
        combinedFilter = combinedFilter.Substring(0, combinedFilter.Length - 5);
    }

    dataSourceEntries.DefaultView.RowFilter = combinedFilter;
}
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btSearch_Click(this, null);
            }
        }

        private void btSearchClear_Click(object sender, EventArgs e)
        {
            dataSourceEntries.DefaultView.RowFilter = "";
        }

        private void datagJournalEntries_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            ContextMenuHelper.INSTANCE.ShowContext(datagJournalEntries, e);
        }
    }
}
