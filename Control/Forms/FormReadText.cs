using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control
{
    public partial class FormReadText : Form
    {
        public FormReadText()
        {
            InitializeComponent();
        }

        public string Send(string Title)
        {
            return Send(Title, false);
        }
        public string Send(string Title, bool Bord)
        {
            LabelTitle.Text = Title;
            if (!Bord)
            {
                Size = new Size(Size.Width, Size.Height - 80);
            }
            BoxLine.Visible = !Bord;
            BoxBord.Visible = Bord;
            ShowDialog();
            if (Bord)
            {
                return BoxBord.Text;
            }
            else
            {
                return BoxLine.Text;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
