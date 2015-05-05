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
    public partial class EditTaetigkeit : Form
    {
        static Zeiterfassung.Properties.Settings userSettings
        {
            get
            {
                return new Zeiterfassung.Properties.Settings();
            }
        }

        Buchungen buch;
        Taetigkeit edit;
        public EditTaetigkeit(Taetigkeit ed)
        {
            InitializeComponent();
            this.Text = userSettings.Titel + "     Version: " + userSettings.Version;
            buch = new Buchungen();

            this.cb_Kategorie.DataSource = buch.GetKategorien();
            this.cb_Kategorie.DisplayMember = "Name";
            this.cb_Kategorie.ValueMember = "Name";
            this.edit = ed;
            if (edit.getKategorie() != "")
            {
                this.cb_Kategorie.Text = edit.getKategorie();
            }
            else
            {               
                this.cb_Kategorie.Text = "";
                this.cb_Kategorie.SelectedItem = "";
            }
            this.txt_Link.Text = edit.getLink();
            this.txt_Taetigkeit.Text = edit.getTitel();

        }
                
        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

            if (txt_Taetigkeit != null && txt_Taetigkeit.Text.Length < 2)
            {
                lbl_status.Text = "Tätigkeit ist zu kurz (mind 1 Zeichen sollten es schon sein)";
                lbl_status.Visible=true;

                txt_Taetigkeit.Focus();
            }
            else{
                book();
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void book()
        {
            edit.setTitel(txt_Taetigkeit.Text);
            edit.setLink(txt_Link.Text);
            edit.setKategorie(cb_Kategorie.Text);

            buch.saveTaetigkeit(edit);


            ZeiterfassungNotifyApp.cm.refresh();
            this.Close();
        }
  
    }
}
