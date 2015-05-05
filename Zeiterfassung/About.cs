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
    public partial class About : Form
    {
        static Zeiterfassung.Properties.Settings userSettings
        {
            get
            {
                return new Zeiterfassung.Properties.Settings();
            }
        }

        public About()
        {
            InitializeComponent();

            lbl_Version.Text += userSettings.Version;
            lbl_build.Text += userSettings.Build;
            lbl_Date.Text += userSettings.BuildDate;
        }


    }
}
