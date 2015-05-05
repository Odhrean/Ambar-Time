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
    public partial class UpdateLink : Form
    {    
        Taetigkeit edit;
        public UpdateLink(Taetigkeit ed)
        {
            InitializeComponent();

            this.edit = ed;
            this.txt_Link.Text = edit.getLink();
        }
        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
            this.txt_Link.Focus();
        }


        private void btn_OK_Click(object sender, EventArgs e)
        {
            edit.setLink(this.txt_Link.Text);
            new Buchungen().saveTaetigkeit(edit);
            ZeiterfassungNotifyApp.cm.refresh();
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
