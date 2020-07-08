using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using myClass;

namespace viewImg
{
    public partial class Form1 : Form
    {
        static string save = "";
        static myData data;
        public Bitmap[] image;
        public int index = 0, tab;
        public bool run = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Thread f = new Thread(tik);
            f.Start();
            timer1.Start();
        }
        private void tik()
        {
            run = true;
            while (run && index < image.Length)
            {
                Thread.Sleep(tab);
                vidioBox.Image = image[index];
                index++;
            }
            index %= image.Length;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            run = false;
        }
        private void Start(object sender, EventArgs e)
        {
            buttonMain.Dispose();
            string write = textBox1.Text;
            textBox1.Dispose();
            if (!myData.Exists("data"))
            {
                MessageBox.Show("אין נתונים");
                Dispose();
                return;
            }
            myData data = new myData("data");
            string saveS = data.getVar("name");
            if (saveS == "")
            {
                MessageBox.Show("אין נתונים");
                Dispose();
                return;
            }
            save = myData.Sava + @"\" + saveS;
            if (!Directory.Exists(save))
            {
                MessageBox.Show("אין נתונים");
                Dispose();
                return;
            }
            FileInfo[] s = new FileInfo[(new DirectoryInfo(save)).GetFiles().Length];
            if (s.Length == 0)
            {
                MessageBox.Show("אין תמונות");
                Dispose();
                return;
            }
            tab = int.Parse(write) * 1000;
            timer1.Interval = tab;
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = new FileInfo(save + @"\img" + i + ".jpg");
            }
            image = new Bitmap[s.Length];
            for (int i = s.Length - 1; i >= 0; i--)
            {
                image[i] = new Bitmap(s[i].FullName);
            }
            vidioBox.Size = image[0].Size;
            vidioBox.Image = image[0];
            this.Size = vidioBox.Size;
            vidioBox.Location = new Point(0, 0);
            buttonStart.Location = new Point(vidioBox.Size.Width / 2 - 115, vidioBox.Size.Height - 125);
            buttonStop.Location = new Point(vidioBox.Size.Width / 2 + 40, vidioBox.Size.Height - 125);
            labelKeepbord.Location = new Point(vidioBox.Size.Width / 2, vidioBox.Size.Height - 200);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string write = "";
            if (File.ReadAllLines(myData.Sava + @"\logout.txt").Length > index)
                write = File.ReadAllLines(myData.Sava + @"\logout.txt")[index];
            labelKeepbord.Location = new Point(vidioBox.Size.Width / 2 - write.Length * 2, labelKeepbord.Location.Y);
            labelKeepbord.Text = write;
        }

        private void labelKeepbord_Click(object sender, EventArgs e)
        {
            if (!labelKeepbord.Font.Bold)
                labelKeepbord.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            else
                labelKeepbord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonStart.Location = new Point(0, -50);
            buttonStop.Location = new Point(0, -50);
        }
    }
}