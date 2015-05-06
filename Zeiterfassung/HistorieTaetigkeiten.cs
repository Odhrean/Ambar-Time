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
                        t.status Status,
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
            DataTable tbl_taetigkeiten = new DataTable();
            db.getDataAdapter().Fill(tbl_taetigkeiten);

            db.close();

            grid_Taetigkeiten.DataSource = tbl_taetigkeiten;

            sql = @"select Kategorie "
                + "from [zeiterfassung].[Kategorie] k "
                + "join [zeiterfassung].[Anwender] a on a.id=k.ID_Anwender "
                + "where a.Anwender='" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "' ";

            db = new clsDatabase(sql, "Taetigkeiten->initGrid[Kategorie]", clsDatabase.SqlDataAdapter);
            DataTable kategorie = new DataTable();

            //tbl_taetigkeiten.TableName = "Zeitverlauf";
            //tbl_taetigkeiten.WriteXml(@"c:\tmp\out.xml");
            db.getDataAdapter().Fill(kategorie);

            db.close();

            tbl_taetigkeiten.RowDeleting += tbl_taetigkeiten_RowDeleting;

            grid_Taetigkeiten.CellEndEdit += new DataGridViewCellEventHandler(grid_Taetigkeiten_CellEndEdit);
            DataGridViewComboBoxColumn kategorieColumn = new DataGridViewComboBoxColumn();
            kategorieColumn.Name = "Kategorie";
            kategorieColumn.DataSource = kategorie;
            kategorieColumn.HeaderText = "Kategorie";
            kategorieColumn.DataPropertyName = "Kategorie";
            kategorieColumn.ValueMember = "Kategorie";
            kategorieColumn.DisplayMember = kategorieColumn.ValueMember;
            
            kategorieColumn.FlatStyle = FlatStyle.Flat;
                        
            grid_Taetigkeiten.Columns["ID_Taetigkeit"].Visible = false;
            grid_Taetigkeiten.Columns["Status"].ReadOnly = true;
            grid_Taetigkeiten.Columns.Remove("Kategorie");
            grid_Taetigkeiten.Columns.Insert(3, kategorieColumn);
            grid_Taetigkeiten.Columns["Start"].ReadOnly = false;
            grid_Taetigkeiten.Columns["Ende"].ReadOnly = false;
            grid_Taetigkeiten.Columns["Mi"].ReadOnly = true;
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
            /*
            DataGridViewCheckBoxColumn aktiv = new DataGridViewCheckBoxColumn();
            {
                aktiv.HeaderText = "aktiv";
                //aktiv.AutoSizeMode =  DataGridViewAutoSizeColumnMode.DisplayedCells;
                //aktiv.FlatStyle = FlatStyle.Standard;
                //aktiv.ThreeState = true;
                aktiv.TrueValue = 1;
                aktiv.FalseValue = 2;
                aktiv.DataPropertyName = "Status";
                aktiv.ValueType = typeof(byte);
                aktiv.ReadOnly = true;
                //aktiv.CellTemplate = new DataGridViewCheckBoxCell();
                //aktiv.CellTemplate.Style.BackColor = Color.Beige;
                //aktiv.DisplayIndex = 1;
            }
            //grid_Taetigkeiten.Columns.Remove("Status");
            grid_Taetigkeiten.Columns.Insert(1, aktiv);
            */
            grid_Taetigkeiten.CellClick += grid_Taetigkeiten_CellClick;
            Debug.Print("{0} : {1}", "Status", grid_Taetigkeiten.Columns["Status"].Width);
            Debug.Print("{0} : {1}", "Taetigkeit", grid_Taetigkeiten.Columns["Taetigkeit"].Width);
            Debug.Print("{0} : {1}", "Kategorie", grid_Taetigkeiten.Columns["Kategorie"].Width);
            Debug.Print("{0} : {1}", "Start", grid_Taetigkeiten.Columns["Start"].Width);
            Debug.Print("{0} : {1}", "Ende", grid_Taetigkeiten.Columns["Ende"].Width);
            Debug.Print("{0} : {1}", "btnOeffnen", grid_Taetigkeiten.Columns["btnOeffnen"].Width);
            
        }

        private void tbl_taetigkeiten_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            Debug.Print("Delete Zeitmessung-ID: {0}", e.Row["ID_zeitmessung"]);
            String sql = "delete from zeiterfassung.zeitmessung where ID=" + e.Row["ID_zeitmessung"];
            //clsDatabase db = new clsDatabase(sql, "tbl_taetigkeiten_RowDeleting->DELETE");
            //db.close();

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
