using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WK.Libraries.FontsInstallerNS;

namespace obfuscript
{

    static class Program
    {

        [DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int AddFontResource(string lpszFilename);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Directory.Exists(Application.StartupPath + @"\Fonts"))
            {

                Console.WriteLine("Missing files.");

                ObfuscriptDialog dialog = new ObfuscriptDialog("Missing files", @"Please re-download obfuscript, there are missing files.", ObfuscriptDialog.Types.Close);
                dialog.ShowDialog();

                Application.Exit();

            }
            else
            {

                try
                {

                    var fontsInstaller = new FontsInstaller();
                    fontsInstaller.InstallFonts($@"{Application.StartupPath}\Fonts");

                    Console.WriteLine("Fonts Installed");

                }
                catch
                {

                    Application.Restart();

                }

            }

            WebClient client = new WebClient();

            try
            {

                if (!client.DownloadString("https://pastebin.com/raw/WuXGi8yn").Contains("1.3.2"))
                {

                    ObfuscriptDialog dialog = new ObfuscriptDialog("Update Available", @"Please update to the latest version from our GitHub page.", ObfuscriptDialog.Types.Close);
                    dialog.ShowDialog();

                    Application.Exit();

                }

            }
            catch
            {

                Application.Run(new ObfuscriptDialog("Something went very wrong", @"There was an issue checking if there was an update.
Please refer to our GitHub page and update to the latest version.", ObfuscriptDialog.Types.Close));

            }

            try
            {

                if (obfuscript.Properties.Settings.Default.KeyToRemember == "FREETIER")
                    Application.Run(new MainFormNew());

                else
                    Application.Run(new NewStartup());

            }
            catch
            {

                Application.Run(new ObfuscriptDialog("Something went very wrong", @"Obfuscript has caught an error. We're not sure what happened,
but please report this to us on our GitHub page.
If this has already been fixed, then please refer to our 
GitHub page and update to the latest version.", ObfuscriptDialog.Types.Close));

            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
