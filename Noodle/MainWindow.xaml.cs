using System;
using System.Configuration;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using Noodle.Core;
using Noodle.Managers;

namespace Noodle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        /// <summary>
        /// The function checks whether the current process is run as administrator.
        /// In other words, it dictates whether the primary access token of the 
        /// process belongs to user account that is a member of the local 
        /// Administrators group and it is elevated.
        /// </summary>
        /// <returns>
        /// Returns true if the primary access token of the process belongs to user 
        /// account that is a member of the local Administrators group and it is 
        /// elevated. Returns false if the token does not.
        /// </returns>
        internal bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public MainWindow()
        {
            var isRunElevated = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("isRunElevated"));

            if (isRunElevated)
                ElevatePermissions();

            InitializeComponent();

            if (isRunElevated)
            {
                if (!IsRunAsAdmin())
                    Application.Current.Shutdown();
            }

            //get WordModels
            FileManager.GetWordModels();

            var schTh = new Thread(new Scheduler().SchedulerLoop);          // Kick off a new thread
            schTh.Start();

            var textInputTh = new Thread(new Define().TextInputLoop);          // Kick off a new thread
            textInputTh.Start();
        }

        private void ElevatePermissions()
        {
            // Elevate the process if it is not run as administrator.
            if (!IsRunAsAdmin())
            {
                // Launch itself as administrator
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;  // or System.Windows.Forms.Application.ExecutablePath ;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    // The user refused to allow privileges elevation.;
                    // Do nothing and return directly ...
                    return;
                }

                Application.Current.Shutdown();  // Quit itself
            }
            else
            {
                Console.WriteLine("UAC: The process is running as administrator.");
            }
        }
    }
}
