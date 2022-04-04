using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WinMove
{
    /// <summary>
    /// Interaction logic for SecondaryWindow.xaml
    /// </summary>
    public partial class SecondaryWindow : Window
    {
        #region Properties
        
        // Screen location
        private System.Drawing.Rectangle _position;
        private System.Drawing.Rectangle Position
        {
            get { return _position; }
            set { _position = value; MoveW(); }
        }

        // Screen is being moved bool
        private bool isBeingMoved;

        #endregion

        #region Constructor

        public SecondaryWindow(System.Drawing.Rectangle position)
        {
            Position = position;
            InitializeComponent();

            Show();
        }

        #endregion

        #region Move Window

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            MoveW();
        }

        public void MoveW()
        {
            if (!Position.IsEmpty)
            {
                WindowInteropHelper wih = new(this);
                IntPtr hWnd = wih.Handle;
                _ = MoveWindow(hWnd, Position.Left, Position.Top, Position.Width, Position.Height, false);
            }
        }

        #endregion

        #region Move Assist

        public async Task MoveTo()
        {
            if (isBeingMoved)
            {
                _ = MoveBack(false);
            }
            else
            {
                var screen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(App.Current.MainWindow).Handle);
                System.Drawing.Rectangle rect = screen.WorkingArea;
                if (!rect.IsEmpty)
                {
                    isBeingMoved = true;
                    var wih = new WindowInteropHelper(this);
                    IntPtr hWnd = wih.Handle;
                    MoveWindow(hWnd, rect.Left, rect.Top, rect.Width, rect.Height, false);
                    _ = MoveBack(true);
                }
            }


            await Task.CompletedTask;
        }
        private async Task MoveBack(bool value)
        {
            if (value) { await Task.Delay(5000); }
            if (isBeingMoved)
            {
                isBeingMoved = false;
                MoveW();
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}
