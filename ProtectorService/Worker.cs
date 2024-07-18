using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowDRM.BL;
using System.Text;

namespace ProtectorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var collection = new Dictionary<nint, string>();
                EnumDesktopWindowsDelegate filter = delegate (IntPtr hWnd, int lParam)
                {
                    StringBuilder strbTitle = new StringBuilder(255);
                    int nLength = Worker.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                    string strTitle = strbTitle.ToString();

                    if (Worker.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
                    {
                        collection.Add(hWnd, strTitle);
                    }
                    return true;
                };

                if (Worker.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
                {
                    //const uint WDA_NONE = 0;
                    const uint WDA_MONITOR = 1;
                    foreach (var item in collection)
                    {
                        SetWindowDisplayAffinity(item.Key, WDA_MONITOR);
                        //Console.WriteLine(item);
                    }
                }

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        #region pInvoke
        //http://pinvoke.net/default.aspx/user32.EnumDesktopWindows
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);
        private delegate bool EnumDesktopWindowsDelegate(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDesktopWindowsDelegate lpfn, IntPtr lParam);

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
    }
}