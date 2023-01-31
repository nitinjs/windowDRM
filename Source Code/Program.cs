using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowDRM
{
    static class Program
    {
        public static String ApplicationTitle;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);
                Version ver = new Version(Application.ProductVersion);
                AutoStart auto = new AutoStart();
                if (!auto.Check()) 
                {
                    auto.RunAtStart();
                }
                frmMain Kb = new frmMain();
                ApplicationTitle = String.Format(Kb.Text);
                if (ClassProcessUtils.ThisProcessIsAlreadyRunning())
                {
                    ClassProcessUtils.SetFocusToPreviousInstance(ApplicationTitle);
                    Kb.Dispose();
                }
                else
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Kb.Visible = false;
                    Application.Run(Kb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
