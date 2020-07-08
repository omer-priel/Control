using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatKey
{
    public partial class FormMouse : Form
    {
        public string ret = "";
        public FormMouse()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ok();   
        }
        private void ok()
        {
            ret = textBox1.Text + ":" + textBox2.Text;
            Dispose();
        }

        private void FormMouse_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = Cursor.Position.X + "";
            textBox2.Text = Cursor.Position.Y + "";
        }

        private void FormMouse_Load(object sender, EventArgs e)
        {
            Size = myClass.ScreenCapture.CaptureDesktop().Size;
            Location = Point.Empty;
        }

        private void FormMouse_KeyDone(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ok();
        }
    }
}
