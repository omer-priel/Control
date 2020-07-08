using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Device.Location;

namespace chatKey
{
    public partial class FormGoogleMaps : Form
    {
        string uri = @"https://www.google.co.il/maps/place/32%C2%B051'25.1%22N+35%C2%B045'17.9%22E/@";
        public FormGoogleMaps(string location)
        {
            InitializeComponent();
            uri += location + @",17z/data=!3m1!4b1!4m5!3m4!1s0x0:0x0!8m2!3d32.856978!4d35.754968";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", uri);
        }
    }
}
