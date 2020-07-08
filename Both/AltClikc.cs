using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Both
{
    public static class AltClick
    {

        [DllImport("user32.dll")]

        [return: MarshalAs(UnmanagedType.Bool)]

        static extern bool SetForegroundWindow(IntPtr hWnd);

        static public void CtrlAltDelete(bool open)
        {
            RegistryKey regkey = default(RegistryKey);
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
            try
            {
                regkey = Registry.CurrentUser.CreateSubKey(subKey);
                if (open)
                {
                    regkey.SetValue("DisableTaskMgr", "0");
                    //regkey.DeleteValue("DisableTaskMgr");
                }
                else
                {
                    regkey.SetValue("DisableTaskMgr", "1");
                }
                regkey.Close();
            }
            catch (Exception ex)
            {
                try { regkey.Close(); } catch { }
            }
        }

        static public void SetMost(Form Window)
        {
            SetForegroundWindow(Window.Handle);
        }
    }

}
