using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Media;
using NAudio.CoreAudioApi;

namespace Both
{
    public class UserAPI
    {

        //All
        public UserAPI()
        {
            readVolume = new ReadVolume();
        }

        //Screenshot
        public static Image ScreenshotDesktop()
        {
            return ScreenshotWindow(User32.GetDesktopWindow());
        }
        public static Bitmap ScreenshotWindow()
        {
            return ScreenshotWindow(User32.GetForegroundWindow());
        }
        public static Bitmap ScreenshotWindow(IntPtr handle)
        {
            var rect = new User32.Rect();
            User32.GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }

        //ReadKey
        private ReadKey readKey;
        public void ReadKeyShange(myAction read)
        {
            readKey = new ReadKey(read);
        }
        public void ReadKeyStart()
        {
            if (readKey != null)
            {
                readKey.Start();
            }
        }
        public static string KeyToText(Keys key, DataKey date)
        {
            string text = key + "";
            switch (key)
            {
                case Keys.RButton:
                    {
                        return "Right Click";
                    }
                case Keys.LButton:
                    {
                        return "Left Click";
                    }
                case Keys.OemMinus:
                    {
                        return "-";
                    }
                case Keys.Oemplus:
                    {
                        return "+";
                    }
            }
            if (text.Length == 2 && text[0] == 'D')
            {
                return text[1] + "";
            }
            if (text.Length == 7 && text.Substring(0, 6) == "NumPad")
            {
                if (date.LockNum)
                {
                    return text[text.Length - 1] + "";
                }
                else
                {
                    string[] NumPads = { "INS", "END", "Down", "PGDN", "Left", "Clear", "Right", "HOME", "Up", "PGUP" };
                    try
                    {
                        int i = int.Parse(text.Length - 1 + "");
                        text = NumPads[i];
                        return text;
                    }
                    catch { }
                }
            }
            return key + "";
        }
        public class DataKey
        {
            public bool LockNum = true;
        }

        //KeepBord
        static public void SetVolumeUpDown(bool up)
        {
            if (up)
                User32.keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
            else
                User32.keybd_event((byte)Keys.VolumeDown, 0, 0, 0);

        }
        static public void SetVolumeMute()
        {
            User32.keybd_event((byte)Keys.VolumeMute, 0, 0, 0);
        }
        static public void SetTv(string tv)
        {
            User32.keybd_event((byte)(Keys)Enum.Parse(typeof(Keys), tv), 0, 0, 0);
        }
        static public void SetKeys(Keys tv)
        {
            User32.keybd_event((byte)tv, 0, 0, 0);
        }
        public static void SetMouse(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }
        static public void HandPlay()
        {
            SystemSounds.Hand.Play();
        }

        //ReadVolume
        public class ReadVolume
        {
            class Device
            {
                public MMDevice[] All;
                public MMDevice Default;
            }
            static private Device GetDrves()
            {
                Device ret = new Device();
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                var all = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.All);
                int i = 0;
                ret.All = new MMDevice[all.Count];
                foreach (var item in all)
                {
                    ret.All[i] = item;
                    i++;
                }
                ret.Default = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
                return ret;
            }

            private Device Devices;

            public ReadVolume()
            {
                Devices = GetDrves();
            }

            public int Level
            {
                get
                {
                    float level = Devices.Default.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
                    return (int)level;
                }
                set
                {
                    float NewValue = value;
                    Devices.Default.AudioEndpointVolume.MasterVolumeLevelScalar = NewValue / 100;
                }
            }
            public int Volume
            {
                get
                {
                    float f = Devices.Default.AudioMeterInformation.MasterPeakValue * 100;
                    int volume = (int)f;
                    return volume;
                }
            }
            public bool Mute
            {
                get
                {
                    return !Devices.Default.AudioEndpointVolume.Mute;
                }
                set
                {
                    Devices.Default.AudioEndpointVolume.Mute = !value;
                }
            }
        }
        public ReadVolume readVolume;
    }
    class User32
    {
        //Screenshot
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        //Read Key
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        //KeepBord
        [DllImport("user32.dll")]
        static public extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    }
    public delegate void myAction(Keys tv);
    class ReadKey
    {

        public static Point mouseLocation()
        {
            User32.POINT lpPoint;
            User32.GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        public int sleep = 5;

        public myAction Read;
        public bool run = false;

        public ReadKey(myAction write = null)
        {
            Read = write;
        }
        public void Start()
        {
            run = true;
            while (run)
            {
                Thread.Sleep(sleep);
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = User32.GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        Read((Keys)i);
                    }
                }
            }
        }
        public void Stop()
        {
            run = false;
        }
    }
}