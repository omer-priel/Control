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
    public partial class Formkeys : Form
    {
        public string ret = "";
        public Formkeys()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ret = textBoxBody.Text;
            Dispose();
        }

        private void Formkeys_Load(object sender, EventArgs e)
        {
            string[] arr = Enum.GetNames(typeof(Keys));
            Array.Sort(arr);
            comboBoxTvs.Items.AddRange(arr);
        }

        private void comboBoxTvs_Leave(object sender, EventArgs e)
        {
            textBoxBody.Text += comboBoxTvs.Text + ",";
        }
    }
}
