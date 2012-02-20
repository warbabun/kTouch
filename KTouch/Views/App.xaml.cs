using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using log4net;

namespace KTouch {

    // A delegate type for hooking up change notifications.
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        /// <summary>
        /// Starts a Windows Presentation Foundation (WPF) application.
        /// </summary>
        /// <param name="e">The System.Int32 application exit code 
        /// that is returned to the operating system 
        /// when the application shuts down. 
        /// By default, the exit code value is 0. </param>
        protected override void OnStartup(StartupEventArgs e) {
            ILog log = LogManager.GetLogger("KTouch");
            if (log.IsInfoEnabled) {
                log.Info("Demarrage de l'application");
            }
            base.OnStartup(e);
            /* TODO : Never to use this because it overflows the system. */
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        /// <summary>
        /// Raises the System.Windows.Application.Exit event.
        /// </summary>
        /// <param name="e">An System.Windows.ExitEventArgs that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e) {
            ILog log = LogManager.GetLogger("KTouch");
            if (log.IsInfoEnabled) {
                log.Info("Fermeture de l'application");
            }
            base.OnExit(e);
        }

        /// <summary>
        /// Raises the System.Windows.Application.Navigated event.
        /// </summary>
        /// <param name="e">A System.Windows.Navigation.NavigationEventArgs that contains the event data.</param>
        protected override void OnLoadCompleted(NavigationEventArgs e) {
            ILog log = LogManager.GetLogger("KTouch");
            if (log.IsDebugEnabled) {
                log.Info("LoadCompleted");
            }
            base.OnLoadCompleted(e);
        }

        /// <summary>
        /// Represents the method that will handle the event raised by an exception that
        /// is not handled by the application domain.
        /// </summary>
        /// <param name="sender">The source of the unhandled exception event.</param>
        /// <param name="e">An UnhandledExceptionEventArgs that contains the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            ILog log = LogManager.GetLogger("KTouch");
            Exception ex = (Exception)e.ExceptionObject;
            log.Fatal("An error occured : " + ex.Message, ex);
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
