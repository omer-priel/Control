using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace myClass
{
    class myData
    {
        //out
        static private string save = @"C:\Data";
        static public string Sava
        {
            get
            {
                return save;
            }
        }
        static public bool Exists(string name)
        {
            return (File.Exists(Sava + @"\" + name + ".data"));
        }

        //in
        private string objecT;
        public string Namespace
        {
            get
            {
                return objecT.Split(new char[] { '.' })[0];
            }
        }
        private RichTextBox text = new RichTextBox();

        public myData(string name)
        {
            this.objecT = name + ".data";
            if (File.Exists(url()))
                text.Text = File.ReadAllText(url());
        }
        ~myData()
        {
            if (File.Exists(url()))
                File.AppendAllText(url(), text.Text);
            else
                File.WriteAllText(url(), text.Text);
        }

        public string url()
        {
            return save + @"\" +  Namespace + ".data";
        }
        public bool ExistsVar(string name)
        {
            foreach (string item in text.Lines)
            {
                if (item == name)
                    return true;
            }
            return false;
        }
        
        public string getVar(string vars)
        {
            foreach (string item in text.Lines)
            {
                string[] str = item.Split(new char[] { ':' });
                if (str[0] == vars)
                    return str[1];
            }
            return "";
        }
        public int setVar(string vars, string value)
        {
            for (int i = 0; i < text.Lines.Length; i++)
            {
                string[] str = text.Lines[i].Split(new char[] { ':' });
                if (str[0] == vars)
                {
                    text.Lines[i] = str[0] + ":" + value;
                    return i;
                }
            }
            return -1;
        }
        public void add(string vars, string value = "")
        {
            if (!ExistsVar(vars))
                text.Text += vars + ":" + value + Environment.NewLine;
        }
    }
}