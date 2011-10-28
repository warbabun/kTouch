using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using log4net;
using KTouch.Units;

namespace KTouch {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup ( StartupEventArgs e ) {
            Console.Out.WriteLine ( "Hello " );
            ILog log = LogManager.GetLogger ( "KTouch" );
            if ( log.IsInfoEnabled ) {

                log.Info ( "Demarrage de l'application" );
            }
            base.OnStartup ( e );
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler ( CurrentDomain_UnhandledException );
            ItemsLoader.LoadCollections ( ConfigurationManager.AppSettings [ "InputCollections" ] );
        }

        protected override void OnExit ( ExitEventArgs e ) {
            ILog log = LogManager.GetLogger ( "KTouch" );
            if ( log.IsInfoEnabled ) {
                log.Info ( "Fermeture de l'application" );
            }
            base.OnExit ( e );
        }

        protected override void OnLoadCompleted ( System.Windows.Navigation.NavigationEventArgs e ) {
            ILog log = LogManager.GetLogger ( "KTouch" );
            if ( log.IsDebugEnabled ) {
                log.Info ( "LoadCompleted" );
            }
            base.OnLoadCompleted ( e );
        }



        private void CurrentDomain_UnhandledException ( object sender, UnhandledExceptionEventArgs e ) {
            ILog log = LogManager.GetLogger ( "KTouch" );
            Exception ex = ( Exception ) e.ExceptionObject;
            log.Fatal ( "An error occured : " + ex.Message, ex );
            Process.Start ( Application.ResourceAssembly.Location );
            Application.Current.Shutdown ( );
        }
    }
}
