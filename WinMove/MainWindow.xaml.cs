using System.Windows;

namespace WinMove
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SecondaryWindow? secondaryWindow { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            OpenSecondary();
        }

        private void OpenSecondary()
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen.Primary) { continue; }
                else { secondaryWindow = new SecondaryWindow(screen.WorkingArea); }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = secondaryWindow?.MoveTo().ConfigureAwait(false);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            secondaryWindow?.Close();
        }
    }
}
