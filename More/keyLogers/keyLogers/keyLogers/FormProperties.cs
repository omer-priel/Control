using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace keyLogers
{
    public partial class FormProperties : Form
    {
        public FormProperties()
        {
            InitializeComponent();
        }
        public int TimeTab
        {
            get
            {
                return (int)(double.Parse(this.textBoxTimeTab.Text) * 1000);
            }
        }

        private void textBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBoxPassword.PasswordChar == '*')
                    textBoxPassword.PasswordChar = '\0';
                else
                    textBoxPassword.PasswordChar = '*';
            }
        }
    }
}
