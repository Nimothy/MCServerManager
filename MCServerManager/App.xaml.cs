using Medallion.Shell;
using System;
using System.Windows;

namespace MCServerManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private bool _isExit;
        private MainWindow window;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            window = new MainWindow();
            MainWindow = window;
            MainWindow.Closing += MainWindow_Closing;

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            notifyIcon.Icon = MCServerManager.Properties.Resources.minecraft;
            notifyIcon.Visible = true;

            MainWindow.Show();

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Server Manager").Click += (s, e) => ShowMainWindow();
            notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            if (window.command != null)
            {
                _ = window.command.TrySignalAsync(CommandSignal.ControlC).Result;
            }
            _isExit = true;
            MainWindow.Close();
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if( !_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide();
            }
        }
    }
}
