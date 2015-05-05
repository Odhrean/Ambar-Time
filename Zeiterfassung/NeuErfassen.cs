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
    public partial class NeuErfassen : Form
    {
        static Zeiterfassung.Properties.Settings userSettings
        {
            get
            {
                return new Zeiterfassung.Properties.Settings();
            }
        }

        Buchungen buch;
        public NeuErfassen()
        {
            InitializeComponent();
            this.Text = userSettings.Titel+"     Version: " + userSettings.Version;
            buch = new Buchungen();
            //Setup data binding
            if (ZeiterfassungNotifyApp.cm.getAktiveTaetigkeit() == null)
            {
                btn_OK_und_beenden.Enabled = false;
            }
            else
            {
                btn_OK_und_beenden.Text = "Start-Neu und \"" + ZeiterfassungNotifyApp.cm.getAktiveTaetigkeit().getTitelKurz(35) +"\" beenden";
            }
            this.cb_Kategorie.DataSource = buch.GetKategorien();
            this.cb_Kategorie.DisplayMember = "Name";
            this.cb_Kategorie.ValueMember = "Name";

            cb_Kategorie.Focus();
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

        private void btn_OK_und_beenden_Click(object sender, EventArgs e)
        {
            if (txt_Taetigkeit != null && txt_Taetigkeit.Text.Length < 2)
            {
                lbl_status.Text = "Tätigkeit ist zu kurz (mind 1 Zeichen sollten es schon sein)";
                lbl_status.Visible=true;

                txt_Taetigkeit.Focus();
            }
            else{
                ZeiterfassungNotifyApp.cm.stop();
                book();
           }
        }

        private void book()
        {
            int id = buch.start(
                txt_Taetigkeit.Text,
                cb_Kategorie.Text,
                txt_Link.Text);

            Taetigkeit neu = new Taetigkeit(id, txt_Taetigkeit.Text, txt_Link.Text);

            ZeiterfassungNotifyApp.cm.addNewTaetigkeit(neu);
            this.Close();
        }

  
    }
}
