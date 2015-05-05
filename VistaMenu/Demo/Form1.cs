using System;
using System.Drawing;
using System.Windows.Forms;

namespace VistaMenuDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Font = SystemFonts.MessageBoxFont;

            InitializeComponent();
            this.ContextMenu = contextMenu;
        }

        private void btnSetOptionsImage_Click(object sender, EventArgs e)
        {
            vistaMenu.SetImage(mnuOptions, vistaMenu.GetImage(mnuAddFiles));
        }

        private void btnClearOptionsImage_Click(object sender, EventArgs e)
        {
            vistaMenu.SetImage(mnuOptions, null);
        }
    }
}
