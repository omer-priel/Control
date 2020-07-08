using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Both;

namespace Control
{
    public class SpyControl
    {
        static public void LoadImages(Bitmap notNetwork, Bitmap volumeOpen, Bitmap volumeClose)
        {
            NotNetwork = notNetwork;
            Size size = new Size(20, 20);
            VolumeOpen = new Bitmap(volumeOpen, size);
            VolumeClose = new Bitmap(volumeClose, size);
        }
        static public Bitmap NotNetwork;
        static public Bitmap VolumeOpen, VolumeClose;

        //Net
        public Client client;
        private DateSpy date;
        public DateSpy Data
        {
            get
            {
                return date;
            }
        }

        //Data
        public int Index = -1;
        private bool select = false;
        public bool Select
        {
            get
            {
                return select;
            }
            set
            {
                select = value;
                Name.Checked = select;
                Bord.BackColor = (select) ? Color.Blue : Color.Red;
                if (ClickSelect != null)
                    ClickSelect(this);
            }
        }
        public EventSender ClickSelect;

        //Form
        public FormControl project;
        public FlowLayoutPanel Out;

        public Panel Bord;
        public CheckBox Name;
        public PictureBox ImageScrean;
        public PictureBox VolumeMute;

        public SpyControl(string IP, FormControl project, out bool Good)
        {
            date = new DateSpy();
            date.IP = IP;
            this.project = project;
            try
            {
                client = new Client(Net.Port, Net.MaxByte);
                client.Read = readServer;
                client.Connect(date.IP);
                Good = true;
            }
            catch (Exception ex)
            {
                project.LabelError.Text = ex.Message;
                Good = false;
            }
        }
        public void Start()
        {
            Out = project.TableSpys;
            Bord = new Panel();
            Bord.Size = new Size(Out.Width - 25, 100);
            Bord.BackColor = Color.Red;
            Name = new CheckBox();
            Name.CheckedChanged += CheckedChanged;
            Name.Location = new Point(10, 10);
            Name.Size = new Size(Bord.Width - 20, 20);
            Name.Text = date.IP;
            Name.TextAlign = ContentAlignment.MiddleCenter;
            ImageScrean = new PictureBox();
            ImageScrean.Location = new Point(5, 30);
            ImageScrean.Size = new Size(Bord.Width - 10, Bord.Height - 35);
            SetImage(NotNetwork);
            VolumeMute = new PictureBox();
            VolumeMute.Location = new Point(5, Bord.Height - 25);
            VolumeMute.Size = new Size(20, 20);
            VolumeMute.BackColor = Color.Pink;
            UpdateVolume();

            Out.Controls.Add(Bord);
            Bord.Controls.Add(Name);
            Bord.Controls.Add(VolumeMute);
            Bord.Controls.Add(ImageScrean);

            Bord.Click += Click;
            Name.Click += Click;
            ImageScrean.Click += Click;
            VolumeMute.Click += VolumeMuteClick;
        }

        public void UpdateIP()
        {
            if (project.NameIP)
            {
                Name.Text = Data.IP;
            }
            else
            {
                Name.Text = Data.Name;
            }
        }
        public void UpdateVolume()
        {
            if (date.VolumeMute)
                VolumeMute.Image = VolumeOpen;
            else
                VolumeMute.Image = VolumeClose;
        }

        public void SetImage(Image image)
        {
            var bitmap = new Bitmap(image, ImageScrean.Size);
            if (bitmap.Size.Height > 0)
                ImageScrean.Image = bitmap;
        }

        public void Click(object sender, EventArgs e)
        {
            Select = !Select;
        }
        public void CheckedChanged(object sender, EventArgs e)
        {
            Name.Checked = Select;
        }
        public void VolumeMuteClick(object sender, EventArgs e)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write('V');
            byteArray.Write('M');
            byteArray.Write(2);
            client.Write(byteArray.Values);
        }

        private void readServer(object sender ,byte[] array)
        {
            if (project.InvokeRequired)
            {
                project.Invoke(new EventServer(readServer),this, array);
                return;
            }
            if (ReadServer != null)
            {
                ReadServer(this, array);
            }
        }
        public EventServer ReadServer;
    }
    public class DateSpy
    {
        public string IP = "";
        public string Name = "";
        public DateTime StartConnect;

        public bool Hide = true;

        public bool VolumeMute = true;
        public int VolumeLevel = 100;
    }
}
