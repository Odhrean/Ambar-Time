using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zeiterfassung
{
    public partial class HistorieTaetigkeiten : Form
    {
        public HistorieTaetigkeiten()
        {
            InitializeComponent();
            initGrid();
        }
        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
        }
        private void initGrid()
        {
            String sql = @"select	
                        t.ID ID_Taetigkeit,
		                t.Taetigkeit Taetigkeit,
                       k.[Kategorie] Kategorie,
					   m.start Start,m.ende Ende,	datediff(mi,m.start,ISNULL(m.ende,GETDATE())) Mi		  
                       ,m.ID ID_zeitmessung
                       from [zeiterfassung].[Taetigkeit] t WITH(NOLOCK)
                       join [zeiterfassung].[Anwender] a WITH(NOLOCK) on t.ID_Anwender=a.ID 
                       left outer join [zeiterfassung].[Kategorie] k on k.ID=t.[ID_Kategorie] 
					   join zeiterfassung.zeitmessung m on m.ID_Taetigkeit=t.ID
                       where a.Anwender='" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "' "
                       +"order by isnull(m.ende,getdate()) desc";

            clsDatabase db = new clsDatabase(sql, "Taetigkeiten->initGrid", clsDatabase.SqlDataAdapter);
            DataSet ds = new DataSet();
            db.Fill(ds, "dummy_table");
            db.close();

            grid_Taetigkeiten.DataSource = ds;
            grid_Taetigkeiten.DataMember = "dummy_table";

            sql = @"select Kategorie "
                + "from [zeiterfassung].[Kategorie] k "
                + "join [zeiterfassung].[Anwender] a on a.id=k.ID_Anwender "
                + "where a.Anwender='" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "' ";

            db = new clsDatabase(sql, "Taetigkeiten->initGrid[Kategorie]", clsDatabase.SqlDataAdapter);
            DataTable kategorie = new DataTable();
            //db.Fill(kategorie, "dummy_table");
            db.getDataAdapter().Fill(kategorie);
            //db.FillDataGridViewThread(grid_Taetigkeiten);
            db.close();

            grid_Taetigkeiten.CellEndEdit += new DataGridViewCellEventHandler(grid_Taetigkeiten_CellEndEdit);
            DataGridViewComboBoxColumn kategorieColumn = new DataGridViewComboBoxColumn();
            kategorieColumn.Name = "Kategorie";
            kategorieColumn.DataSource = kategorie;
            kategorieColumn.HeaderText = "Kategorie";
            kategorieColumn.DataPropertyName = "Kategorie";
            kategorieColumn.ValueMember = "Kategorie";
            kategorieColumn.DisplayMember = kategorieColumn.ValueMember;
            
            kategorieColumn.FlatStyle = FlatStyle.Flat;

            grid_Taetigkeiten.Columns["ID_Taetigkeit"].ReadOnly = true;
            grid_Taetigkeiten.Columns["ID_Taetigkeit"].Visible = false;
            grid_Taetigkeiten.Columns.Remove("Kategorie");
            grid_Taetigkeiten.Columns.Insert(2, kategorieColumn);
            grid_Taetigkeiten.Columns["Start"].ReadOnly = false;
            grid_Taetigkeiten.Columns["Ende"].ReadOnly = false;
            grid_Taetigkeiten.Columns["Mi"].ReadOnly = true;
            grid_Taetigkeiten.Columns["ID_zeitmessung"].ReadOnly = true;
            grid_Taetigkeiten.Columns["ID_zeitmessung"].Visible = false;

            DataGridViewButtonColumn oeffnen = new DataGridViewButtonColumn();
            {
                oeffnen.HeaderText = "fortsetzen";
                oeffnen.Name = "btnOeffnen";
                oeffnen.Text = " => ";
                oeffnen.UseColumnTextForButtonValue = true;
                oeffnen.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                oeffnen.FlatStyle = FlatStyle.Standard;
                oeffnen.CellTemplate.Style.BackColor = Color.Honeydew;
                oeffnen.DisplayIndex = 0;
                
            }
            grid_Taetigkeiten.Columns.Add(oeffnen);
            grid_Taetigkeiten.CellClick += grid_Taetigkeiten_CellClick;
            
        }

        private void grid_Taetigkeiten_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nur Clicks auf Button bearbeiten
            if (e.ColumnIndex == grid_Taetigkeiten.Columns["btnOeffnen"].Index)
            { 
                Debug.Print("Setze fort: {0} ({1})", grid_Taetigkeiten.Rows[e.RowIndex].Cells["ID_Taetigkeit"].Value, grid_Taetigkeiten.Rows[e.RowIndex].Cells["ID_zeitmessung"].Value);
                new Buchungen().start(Convert.ToInt32(grid_Taetigkeiten.Rows[e.RowIndex].Cells["ID_Taetigkeit"].Value));

                ZeiterfassungNotifyApp.cm.refresh();
                Close();
            }

        }

        void grid_Taetigkeiten_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grid_Taetigkeiten.Columns["Taetigkeit"].Index
                || e.ColumnIndex == grid_Taetigkeiten.Columns["Kategorie"].Index)
            {
                Taetigkeit save = new Taetigkeit();
                save.setId((int)grid_Taetigkeiten.Rows[e.RowIndex].Cells["ID_Taetigkeit"].Value);
                save.setTitel((string)grid_Taetigkeiten.Rows[e.RowIndex].Cells["Taetigkeit"].Value);
                save.setKategorie((string)grid_Taetigkeiten.Rows[e.RowIndex].Cells["Kategorie"].Value);
                new Buchungen().saveTaetigkeit(save);
            }
            if (e.ColumnIndex == grid_Taetigkeiten.Columns["Start"].Index
                || e.ColumnIndex == grid_Taetigkeiten.Columns["Ende"].Index)
            {
                Debug.Print("({0}) Edit Start {1} und Ende {2} ", grid_Taetigkeiten.Rows[e.RowIndex].Cells["ID_zeitmessung"].Value, grid_Taetigkeiten.Rows[e.RowIndex].Cells["Start"].Value, grid_Taetigkeiten.Rows[e.RowIndex].Cells["Ende"].Value);
                String sql = "update zeiterfassung.zeitmessung set start='" + grid_Taetigkeiten.Rows[e.RowIndex].Cells["Start"].Value + "', ende=case when '" + grid_Taetigkeiten.Rows[e.RowIndex].Cells["Ende"].Value + "' != '' then '" + grid_Taetigkeiten.Rows[e.RowIndex].Cells["Ende"].Value + "' else null end where ID=" + grid_Taetigkeiten.Rows[e.RowIndex].Cells["ID_zeitmessung"].Value;

                clsDatabase db = new clsDatabase(sql, "grid_Taetigkeiten_CellEndEdit->Edit");
                db.close();
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
