using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace Control
{
    public class FileView
    {
        static public FileView SelectMenu;

        private string url;
        public string Url
        {
            get
            {
                return url;
            }
        }
        private bool isFolder;
        public bool IsFolder
        {
            get
            {
                return isFolder;
            }
        }
        private bool select;
        public bool Select
        {
            get
            {
                return select;
            }
            set
            {
                select = value;
                Cell.BackColor = (select) ? Color.Blue : Color.Red;
            }
        }

        //Forms
        private FormControl Project;
        public Panel Cell;
        public Label NameFile;
        public PictureBox ImageFile;

        public FileView(FormControl project, string Url, bool folder)
        {
            url = Url;
            Project = project;
            isFolder = folder;
            Cell = new Panel();
            Cell.Size = new Size(110, 140);
            Cell.ContextMenuStrip = Project.MenuStripFiles;
            Project.ViewFiles.Controls.Add(Cell);
            NameFile = new Label();
            NameFile.Location = new Point(10, 10);
            NameFile.Size = new Size(Cell.Width - 20, 20);
            if (folder)
            {
                NameFile.Text = new DirectoryInfo(Url).Name;
            }
            else
            {
                NameFile.Text = new FileInfo(Url).Name;
            }
            NameFile.TextAlign = ContentAlignment.MiddleCenter;
            Cell.Controls.Add(NameFile);
            Select = false;
            ImageFile = new PictureBox();
            ImageFile.Location = new Point(10, 30);
            ImageFile.Size = new Size(Cell.Width - 20, Cell.Height - 40);
            string ImageUrl = Strings.Images;
            if (folder)
            {
                ImageUrl += @"\Folder.jpg";
                Cell.DoubleClick += folderOpen;
                NameFile.DoubleClick += folderOpen;
                ImageFile.DoubleClick += folderOpen;
            }
            else
            {
                try
                {
                    string ImageUrl2 = Strings.ImagesIcon + "\\" + new FileInfo(url).Extension.Remove(0, 1) + Strings.IconType;
                    if (File.Exists(ImageUrl2))
                    {
                        ImageUrl = ImageUrl2;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    ImageUrl += @"\File.jpg";
                }
            }
            Bitmap image = image = new Bitmap(ImageUrl);
            ImageFile.Image = image;
            Cell.Controls.Add(ImageFile);

            Cell.Click += click;
            NameFile.Click += click;
            ImageFile.Click += click;
            Cell.MouseDown += mouseDown;
            NameFile.MouseDown += mouseDown;
            ImageFile.MouseDown += mouseDown;
        }

        private void click(object sender, EventArgs e)
        {
            Select = !Select;
            if (Click != null)
                Click(this);
        }
        public EventSender Click;
        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && Select)
            {
                SelectMenu = this;
            }
            else
            {
                SelectMenu = null;
            }
        }
        private void folderOpen(object sender, EventArgs e)
        {
            if (FolderOpen != null)
                FolderOpen(this);
        }
        public EventSender FolderOpen;
    }
}
