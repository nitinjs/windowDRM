using WindowDRM.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowDRM
{
    public partial class frmMain : Form
    {
        bool runInBg = true;
        public frmMain()
        {
            InitializeComponent();
            mnExit.Click += MnExit_Click;
            mnSettings.Click += MnSettings_Click;
            ntfTray.DoubleClick += MnSettings_Click; ;
        }

        private void MnSettings_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void MnExit_Click(object sender, EventArgs e)
        {
            runInBg = false;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            runInBg = true;
            this.Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        #region pInvoke

        [DllImport("user32.dll")]
        static extern uint SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //https://stackoverflow.com/questions/56712964/how-to-use-setwindowdisplayaffinity
        //https://stackoverflow.com/questions/1363167/how-can-i-get-the-child-windows-of-a-window-given-its-hwnd/28055461#28055461
        //http://pinvoke.net/default.aspx/user32.EnumChildWindows
        //https://devblogs.microsoft.com/oldnewthing/20130603-00/?p=4193
        void ProtectApp(string processName)
        {
            Process[] anotherApps = Process.GetProcessesByName(processName);
            if (anotherApps.Length == 0) return;
            if (anotherApps[0] != null)
            {
                var allChildWindows = new WindowHandleInfo(anotherApps[0].MainWindowHandle).GetAllChildHandles();

                foreach (IntPtr window in allChildWindows)
                {
                    IntPtr handle = window;
                }
            }
        }
        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = runInBg;
        }
    }
}
