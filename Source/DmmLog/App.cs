using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace DmmLog {
    internal static class App {

        [STAThread]
        private static void Main() {
            bool createdNew;
            var mutexSecurity = new MutexSecurity();
            mutexSecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow));
            using (var setupMutex = new Mutex(false, @"Global\JosipMedved_DmmLog", out createdNew, mutexSecurity)) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Medo.Application.UnhandledCatch.ThreadException += new EventHandler<ThreadExceptionEventArgs>(UnhandledCatch_ThreadException);
                Medo.Application.UnhandledCatch.Attach();

                Drivers.Initialize();

                Application.Run(new MainForm());
            }
        }


        private static void UnhandledCatch_ThreadException(object sender, ThreadExceptionEventArgs e) {
#if !DEBUG
            Medo.Diagnostics.ErrorReport.ShowDialog(null, e.Exception, new Uri("http://jmedved.com/feedback/"));
            Medo.Services.Upgrade.ShowDialog(null, new Uri("http://jmedved.com/upgrade/"));
#else
            throw e.Exception;
#endif
        }

    }
}
