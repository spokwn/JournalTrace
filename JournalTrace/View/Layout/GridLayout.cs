using JournalTrace.Language;
using JournalTrace.Entry;
using System;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using JournalTrace.View.Util;
using System.Collections.Generic;
using System.Linq;

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
        private void performSearch()
        {
            string filterText = txtSearch.Text.Trim();
            string reasonColumn = "reason";
            string combinedFilter = "";

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                if (filterText.Contains(":") && !IsValidPath(filterText))
                {
                    string[] filterConditions = filterText.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string condition in filterConditions)
                    {
                        string[] parts = condition.Split(new[] { ':' }, 2);
                        if (parts.Length != 2) continue;

                        string columnName = parts[0].Trim();
                        string filterValue = parts[1].Trim();

                        if (!dataSourceEntries.Columns.Contains(columnName) && !IsValidPath(filterValue))
                            continue;

                        string columnFilter = BuildColumnFilter(columnName, filterValue);
                        if (!string.IsNullOrEmpty(columnFilter))
                            combinedFilter = AppendFilter(combinedFilter, columnFilter);
                    }
                }
                else
                {
                    string columnName;
                    string searchValue;

                    if (IsValidPath(filterText))
                    {
                        columnName = "directory";
                        searchValue = filterText;
                    }
                    else if (comboSearch.SelectedIndex > 0)
                    {
                        columnName = dataSourceEntries.Columns[comboSearch.SelectedIndex].ColumnName;
                        searchValue = filterText;
                    }
                    else
                    {
                        dataSourceEntries.DefaultView.RowFilter = "";
                        return;
                    }

                    string columnFilter = BuildColumnFilter(columnName, searchValue);
                    if (!string.IsNullOrEmpty(columnFilter))
                        combinedFilter = AppendFilter(combinedFilter, columnFilter);
                }
            }

            combinedFilter = AppendCheckBoxFilters(combinedFilter, reasonColumn);
            dataSourceEntries.DefaultView.RowFilter = combinedFilter;
        }

        private bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            return path.Contains(":\\") ||
                   path.StartsWith("\\\\") ||
                   path.StartsWith("/") ||
                   path.StartsWith("./") ||
                   path.StartsWith("../");
        }

        private string BuildColumnFilter(string columnName, string filterValue)
        {
            if (string.IsNullOrWhiteSpace(filterValue))
                return string.Empty;

            string columnFilter = "";

            if (filterValue.Contains("!!"))
            {
                string[] parts = filterValue.Split(new[] { "!!" }, StringSplitOptions.None);
                if (!string.IsNullOrWhiteSpace(parts[0]))
                    columnFilter = $"{columnName} LIKE '%{parts[0].Trim()}%'";

                for (int i = 1; i < parts.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(parts[i]))
                        columnFilter = AppendFilter(columnFilter, $"{columnName} NOT LIKE '%{parts[i].Trim()}%'", "AND");
                }
            }
            else if (filterValue.Contains("&&"))
            {
                var parts = filterValue.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                columnFilter = string.Join(" AND ", parts.Select(p => $"{columnName} LIKE '%{p.Trim()}%'"));
            }
            else if (filterValue.Contains("||"))
            {
                var parts = filterValue.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                columnFilter = string.Join(" OR ", parts.Select(p => $"{columnName} LIKE '%{p.Trim()}%'"));
            }
            else
            {
                columnFilter = $"{columnName} LIKE '%{filterValue}%'";
            }

            return !string.IsNullOrEmpty(columnFilter) ? $"({columnFilter})" : string.Empty;
        }

        private string AppendFilter(string existingFilter, string newFilter, string conjunction = "AND")
        {
            if (string.IsNullOrEmpty(existingFilter))
                return newFilter;
            if (string.IsNullOrEmpty(newFilter))
                return existingFilter;
            return $"{existingFilter} {conjunction} {newFilter}";
        }

        private string AppendCheckBoxFilters(string currentFilter, string reasonColumn)
        {
            var checkBoxFilters = new Dictionary<CheckBox, string>
    {
        { CBdeleted, "File delete" },
        { CBrenamednew, "Rename: new name" },
        { CBrenamedold, "Rename: old name" },
        { CBstreamchange, "Stream change" },
        { CBbasicinfochange, "Basic info change" }
    };

            foreach (var checkBox in checkBoxFilters)
            {
                if (checkBox.Key.Checked)
                {
                    string newFilter = $"({reasonColumn} LIKE '%{checkBox.Value}%')";
                    currentFilter = AppendFilter(currentFilter, newFilter);
                }
            }

            return currentFilter;
        }
        private void btSearch_Click(object sender, EventArgs e)
        {
            performSearch();
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

        private void CBdataextend_CheckedChanged(object sender, EventArgs e)
        {
            performSearch();
        }

        private void CBrenamed_CheckedChanged(object sender, EventArgs e)
        {
            performSearch();
        }

        private void CBdeleted_CheckedChanged(object sender, EventArgs e)
        {
            performSearch();
        }

        private void CBrenamedold_CheckedChanged(object sender, EventArgs e)
        {
            performSearch();
        }

        private void CBbasicinfochange_CheckedChanged(object sender, EventArgs e)
        {
            performSearch();
        }
    }
}
