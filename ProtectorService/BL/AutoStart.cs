using Microsoft.Win32;
using System.Windows.Forms;
namespace WindowDRM
{
    public class AutoStart
    {
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\LocalMachine\\Run", true);
        public AutoStart()
        {
            rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\LocalMachine\\Run", true);
        }
        public void RunAtStart(bool val=true)
        {
            if (val)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("PropTaxApp", Application.ExecutablePath.ToString());
                //MessageBox.Show(Application.ExecutablePath.ToString());
            }
            else
            {
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("PropTaxApp", false);
            }
        }
        public bool Check()
        {
            try
            {
                if (rkApp.GetValue("PropTaxApp") == null)
                {
                    // The value doesn't exist, the application is not set to run at startup
                    return false;
                }

                else
                {
                    // The value exists, the application is set to run at startup
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}
