using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Control
{
    public partial class FormIconOfFile : Form
    {
        public string Folder, UrlDefault;
        public Image Default;
        private IconFile selectIcon = null;

        public FormIconOfFile(string folder, string urlDefault)
        {
            InitializeComponent();
            Folder = folder;
            UrlDefault = urlDefault;
            Default = new Bitmap(urlDefault);
        }

        private void FormIconOfFile_Load(object sender, EventArgs e)
        {
            SetImage();
            UpdateList();
        }

        //Buttons
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image " + Strings.IconType.ToUpper().Remove(0,1) + "|*" + Strings.IconType;
            open.Title = open.Filter;
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(open.FileName);
                if (image.Size != new Size(90, 100))
                {
                    var anser = MessageBox.Show("התמונה לא במידה הנכונה\n90px על 100px\n?אתה בטוח שאתה רוצה", "?אתה בטוח", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (anser != DialogResult.OK)
                    {
                        return;
                    }
                    image.Dispose();
                }
                selectIcon.Image.Dispose();
                File.Delete(selectIcon.Url);
                File.Copy(open.FileName, selectIcon.Url);
                open.Dispose();
                selectIcon.Url = open.FileName;
                selectIcon.Image = new Bitmap(selectIcon.Url);
                SetImage(selectIcon);
            }
        }
        private void BoxListNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectIcon = (IconFile)BoxListNames.SelectedItem;
            SetImage(selectIcon);
        }

        //Tool
        private void Tool_Add(object sender, EventArgs e)
        {
            FormReadText read = new FormReadText();
            IconFile add = new IconFile();
            add.Name = read.Send("Type File");
            add.Url = Folder + "\\" + add.Name + Strings.IconType;
            if (File.Exists(add.Url))
            {
                MessageBox.Show("Is Name In The Icons List!");
                return;
            }
            File.Copy(UrlDefault, add.Url);
            add.Image = new Bitmap(add.Url);
            BoxListNames.Items.Add(add);
        }
        private void Tool_Delet(object sender, EventArgs e)
        {
            if (selectIcon != null)
            {
                string url = selectIcon.Url;
                SetImage();
                selectIcon.Image.Dispose();
                BoxListNames.Items.Remove(selectIcon);
                File.Delete(url);
                selectIcon = null;
            }
        }

        //More
        private void SetImage(IconFile Icon = null)
        {
            buttonOpenFile.Enabled = (Icon != null);
            if (!buttonOpenFile.Enabled)
            {
                BoxIcon.Image = Default;
                labelName.Text = "File";
            }
            else
            {
                BoxIcon.Image = Icon.Image;
                labelName.Text = Icon.Name;
            }
        }
        private void UpdateList()
        {
            BoxListNames.Items.Clear();
            string[] icons = Directory.GetFiles(Folder);
            foreach (var item in icons)
            {
                FileInfo file = new FileInfo(item);
                if (file.Extension == Strings.IconType)
                {
                    IconFile add = new IconFile();
                    add.Name = file.Name.Split('.')[0];
                    add.Url = item;
                    add.Image = new Bitmap(item);
                    BoxListNames.Items.Add(add);
                }
            }
        }

        class IconFile
        {
            public string Name;
            public string Url;
            public Bitmap Image;

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
