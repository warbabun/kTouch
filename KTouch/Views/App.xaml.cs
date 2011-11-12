using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using KTouch.Units;
using log4net;

namespace KTouch {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        /// <summary>
        /// Returns 'true' if the application content is loaded.
        /// </summary>
        public bool IsContentLoaded {
            get;
            private set;
        }


        /// <summary>
        /// Starts a Windows Presentation Foundation (WPF) application.
        /// </summary>
        /// <param name="e">The System.Int32 application exit code 
        /// that is returned to the operating system 
        /// when the application shuts down. 
        /// By default, the exit code value is 0. </param>
        protected override void OnStartup(StartupEventArgs e) {
            Console.Out.WriteLine("Hello ");
            ILog log = LogManager.GetLogger("KTouch");
            if(log.IsInfoEnabled) {

                log.Info("Demarrage de l'application");
            }
            base.OnStartup(e);
            /* TODO : Never to use this because it overflows the system. */
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            ItemsLoader.LoadCollections(ConfigurationManager.AppSettings["InputCollections"]);
            Loader<object>.LoadXDocument(ConfigurationManager.AppSettings["ContentDirectory"]);
            Loader<object>.ItemCollectionCreated += new EventHandler(kLoader_ItemCollectionCreated);

        }

        void kLoader_ItemCollectionCreated(object sender, EventArgs e) {
            this.IsContentLoaded = true;
        }

        /// <summary>
        /// Raises the System.Windows.Application.Exit event.
        /// </summary>
        /// <param name="e">An System.Windows.ExitEventArgs that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e) {
            ILog log = LogManager.GetLogger("KTouch");
            if(log.IsInfoEnabled) {
                log.Info("Fermeture de l'application");
            }
            base.OnExit(e);
        }

        /// <summary>
        /// Raises the System.Windows.Application.Navigated event.
        /// </summary>
        /// <param name="e">A System.Windows.Navigation.NavigationEventArgs that contains the event data.</param>
        protected override void OnLoadCompleted(System.Windows.Navigation.NavigationEventArgs e) {
            ILog log = LogManager.GetLogger("KTouch");
            if(log.IsDebugEnabled) {
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
