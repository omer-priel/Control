using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;

namespace myClass
{
    class SetCeepBord
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        static public void setUpDown(bool up)
        {
            if (up)
                keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
            else
                keybd_event((byte)Keys.VolumeDown, 0, 0, 0);
        }
        static public void setVolumeMute()
        {
            keybd_event((byte)Keys.VolumeMute, 0, 0, 0);
        }
        static public void setTv(string tv)
        {
            keybd_event((byte)(Keys)Enum.Parse(typeof(Keys), tv), 0, 0, 0);
        }
        static public void setKeys(Keys tv)
        {
            keybd_event((byte)tv, 0, 0, 0);
        }
        public static void setMouse(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }
        static public void setText(string text, bool sloly = false)
        {
            int sleap = 1;
            if (sloly)
                sleap = 50;
            string[] tvs = text.Split(',');
            foreach (var item in tvs)
            {
                Thread.Sleep(sleap);
                go(item);
            }
        }
        static private void go(string text)
        {
            Thread th = new Thread(p);
            po = text;
            th.Start();
        }
        static private string po;
        static private void p()
        {
            myClass.SetCeepBord.setTv(po);
        }
        static public void HandPlay()
        {
            SystemSounds.Hand.Play();
        }
    }
}
