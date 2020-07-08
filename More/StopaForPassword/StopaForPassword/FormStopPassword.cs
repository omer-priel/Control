using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace StopaForPassword
{
    public partial class FormStopPassword : Form
    {
        private bool IsWork = false;

        public string TheGoodPassword;
        public bool StopCtrlAltDelete = true;
        public bool StopAltTab = true;

        public FormStopPassword()
        {
            InitializeComponent();
            
        }

        public void StartWindow(bool HavePassword)
        {
            if (IsWork)
                return;
            IsWork = true;
            buttonOK.Text = "Log In";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = this.TransparencyKey;

            if (HavePassword)
            {
                BoxPassword.Text = "";
                BoxPassword.PasswordChar = '*';
                panelPassword.Location = new Point((this.Width - panelPassword.Width) / 2, (this.Height - panelPassword.Height) / 2);
            }
            else
            {
                panelPassword.Visible = false;
            }
            if (StopCtrlAltDelete)
                AltClick.CtrlAltDelete(false);
            if (StopAltTab)
                timerSetTab.Start();
            
        }
        public void StopWindow()
        {
            if (!IsWork)
                return;
            IsWork = false;
            if (StopCtrlAltDelete)
                AltClick.CtrlAltDelete(true);
            if (StopAltTab)
                timerSetTab.Stop();
            Close();
            Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (IsWork)
            {
                if (BoxPassword.Text == TheGoodPassword)
                {
                    StopWindow();
                }
            }
            else
            {
                TheGoodPassword = BoxPassword.Text;
                StartWindow(true);
            }
        }
        private void timerSetTab_Tick(object sender, EventArgs e)
        {
            AltClick.SetMost(this);
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsWork;
        }
    }
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
                    regkey.DeleteValue("DisableTaskMgr");
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
