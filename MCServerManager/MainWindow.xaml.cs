using Medallion.Shell;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MCServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Command command;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (command != null)
                {
                    MessageBox.Show("Server already started");
                    return;
                }

                var cmd = File.ReadAllText("cmd.txt");

                if ( cmd == null)
                {
                    MessageBox.Show("Command cannot be null");
                    return;
                }
                command = Command.Run("cmd.exe", "/c", cmd);

                string line;
                string l2;
                LogBox.Text = "";
                _ = Task.Run(() =>
                  {
                      while ((l2 = command.StandardError.ReadLine()) != null && command != null)
                      {
                          if ( l2 != "^C")
                          {
                              MessageBox.Show(l2);
                              BtnStop_Click(null, null);
                          }
                      }
                  });
                while((line = await command.StandardOutput.ReadLineAsync()) != null && command != null) {
                    if ( line.Length != 0) LogBox.Text = $"{LogBox.Text} \n {line}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oops...");
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = command.TrySignalAsync(CommandSignal.ControlC).Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oops...");
            }
            finally
            {
                command = null;
            }
        }
    }
}
