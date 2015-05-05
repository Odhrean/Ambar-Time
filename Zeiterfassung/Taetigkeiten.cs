using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zeiterfassung
{
    public partial class Taetigkeiten : Form
    {
        public Taetigkeiten()
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
                        t.ID,
		                t.Taetigkeit,
                       k.[Kategorie],
					   m.start,m.ende,	datediff(mi,m.start,ISNULL(m.ende,GETDATE())) Mi		  
                       from [zeiterfassung].[Taetigkeit] t WITH(NOLOCK)
                       join [zeiterfassung].[Anwender] a WITH(NOLOCK) on t.ID_Anwender=a.ID 
                       left outer join [zeiterfassung].[Kategorie] k on k.ID=t.[ID_Kategorie] 
					   join zeiterfassung.zeitmessung m on m.ID_Taetigkeit=t.ID
                       where a.Anwender='" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "' "
                       +"order by isnull(m.ende,getdate()) desc";

            clsDatabase db = new clsDatabase(sql, "Taetigkeiten->initGrid", clsDatabase.SqlDataAdapter);
            DataSet ds = new DataSet();
            db.Fill(ds, "dummy_table");
            grid_Taetigkeiten.DataSource = ds;
            grid_Taetigkeiten.DataMember = "dummy_table";

            //db.FillDataGridViewThread(grid_Taetigkeiten);

            grid_Taetigkeiten.CellEndEdit += new DataGridViewCellEventHandler(grid_Taetigkeiten_CellEndEdit);

            grid_Taetigkeiten.Columns[0].ReadOnly = true;
            grid_Taetigkeiten.Columns[3].ReadOnly = true;
            grid_Taetigkeiten.Columns[4].ReadOnly = true;
            grid_Taetigkeiten.Columns[5].ReadOnly = true;

        }

        void grid_Taetigkeiten_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Taetigkeit save = new Taetigkeit();
            save.setId((int)grid_Taetigkeiten.Rows[e.RowIndex].Cells[0].Value);
            save.setTitel((string)grid_Taetigkeiten.Rows[e.RowIndex].Cells[1].Value);
            save.setKategorie((string)grid_Taetigkeiten.Rows[e.RowIndex].Cells[2].Value);

            new Buchungen().saveTaetigkeit(save);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
