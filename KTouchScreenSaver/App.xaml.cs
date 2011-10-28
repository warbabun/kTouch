using System;
using System.Windows;
using System.Windows.Interop;

namespace KTouchScreenSaver {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        // Used to host WPF content in preview mode, attach HwndSource to parent Win32 window.
        private HwndSource winWPFContent;
        private Window1 winSaver;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hwnd, IntPtr
        hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int BringWindowToTop(IntPtr hwnd);

        public IntPtr HWND_TOPMOST = (IntPtr)(-1);
        public IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
        public int SWP_NOSIZE = 0x1;

        private void Application_Startup(object sender, StartupEventArgs e) {
            // Preview mode--display in little window in Screen Saver dialog
            // (Not invoked with Preview button, which runs Screen Saver in
            // normal /s mode).
            if (e.Args[0].ToLower().StartsWith("/p")) {
                winSaver = new Window1();

                Int32 previewHandle = Convert.ToInt32(e.Args[1]);
                //  WindowInteropHelper interopWin1 = new WindowInteropHelper(win);
                //interopWin1.Owner = new IntPtr(previewHandle);

                IntPtr pPreviewHnd = new IntPtr(previewHandle);

                RECT lpRect = new RECT();
                bool bGetRect = Win32API.GetClientRect(pPreviewHnd, ref lpRect);

                HwndSourceParameters sourceParams = new HwndSourceParameters("sourceParams");

                sourceParams.PositionX = 0;
                sourceParams.PositionY = 0;
                sourceParams.Height = lpRect.Bottom - lpRect.Top;
                sourceParams.Width = lpRect.Right - lpRect.Left;
                sourceParams.ParentWindow = pPreviewHnd;
                sourceParams.WindowStyle = (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN);

                winWPFContent = new HwndSource(sourceParams);
                winWPFContent.Disposed += new EventHandler(winWPFContent_Disposed);
                winWPFContent.RootVisual = winSaver.grid1;
            }

            // Normal screensaver mode.  Either screen saver kicked in normally,
                // or was launched from Preview button
            else if (e.Args[0].ToLower().StartsWith("/s")) {
                Window1 win = new Window1();
                win.WindowState = WindowState.Maximized;
                win.Show();
            }

            // Config mode, launched from Settings button in screen saver dialog
            else if (e.Args[0].ToLower().StartsWith("/c")) {
                ShowSettingWindow();
            }

            // If not running in one of the sanctioned modes, shut down the app
                // immediately (because we don't have a GUI).
            else {
                Application.Current.Shutdown();
            }
        }

        void ShowSettingWindow() {
            SettingsWindow setWind = new SettingsWindow();
            KTouchScreenSaverSettingsModelView setWindViewModel = new KTouchScreenSaverSettingsModelView();
            setWind.DataContext = setWindViewModel;
            EventHandler handler = null;
            handler = delegate {
                setWindViewModel.RequestClose -= handler;
                setWind.Close();
            };
            setWindViewModel.RequestClose += handler;

            setWind.Show();
        }


        /// <summary>
        /// Event that triggers when parent window is disposed--used when doing
        /// screen saver preview, so that we know when to exit.  If we didn't 
        /// do this, Task Manager would get a new .scr instance every time
        /// we opened Screen Saver dialog or switched dropdown to this saver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void winWPFContent_Disposed(object sender, EventArgs e) {
            winSaver.Close();
            //            Application.Current.Shutdown();
        }
    }
}
