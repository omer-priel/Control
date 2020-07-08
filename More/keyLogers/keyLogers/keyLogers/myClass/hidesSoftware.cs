using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace myClass
{
    class hidesSoftware
    {
        private static bool look;
        public static bool Look 
        {
            get
            {
                return look;
            }
            set
            {
                IntPtr hWndConsole = GetConsoleWindow();

                if (hWndConsole != IntPtr.Zero)
                {
                    look = value;
                    if (value)
                        ShowWindow(hWndConsole, 1);
                    else
                        ShowWindow(hWndConsole, 0);
                }
            }
        }
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
    }
}
