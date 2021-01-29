using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace MarkAble2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg.Equals("-debug", StringComparison.OrdinalIgnoreCase))
                    {
                        Global.Options.DebugMode = true;
                    }

                    //allow command-line selection of language, overrides current culture.

                    if (arg.Equals("-de", StringComparison.OrdinalIgnoreCase))
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
                    }
                    if (arg.Equals("-en", StringComparison.OrdinalIgnoreCase))
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    }
                }
            }

            Application.Run(new frmSplash());

            if (!Registration.IsExhausted())
            {
                Application.Run(new frmIntro());
            }
        }
    }
}