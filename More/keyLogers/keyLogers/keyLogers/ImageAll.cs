using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace myClass
{
    public partial class ImageAll : Form
    {
        public bool stop = true;
        public string fileName = null;
        public ImageAll()
        {
            InitializeComponent();
        }

        private void close(object sender, FormClosingEventArgs e)
        {
            e.Cancel = stop;
        }

        private void myLoad(object sender, EventArgs e)
        {
            this.SetDesktopLocation(0, 0);
            this.Size = myClass.ScreenCapture.CaptureDesktop().Size;
            if (fileName != null)
            {
                if (fileName == "")
                    this.BackgroundImage = myClass.ScreenCapture.CaptureDesktop();
                else
                    this.BackgroundImage = new Bitmap(fileName);
            }
        }
        
    }
}
