
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Clock555
{
    public partial class MainWindow : Window
    {
        private const int WM_NCLBUTTONDOWN = 0x00A1;

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void Resize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var element = sender as FrameworkElement;
            if (element == null || element.Tag == null)
                return;

            int htCode = element.Tag.ToString() switch
            {
                "Left" => HTLEFT,
                "Right" => HTRIGHT,
                "Top" => HTTOP,
                "TopLeft" => HTTOPLEFT,
                "TopRight" => HTTOPRIGHT,
                "Bottom" => HTBOTTOM,
                "BottomLeft" => HTBOTTOMLEFT,
                "BottomRight" => HTBOTTOMRIGHT,
                _ => 0
            };

            if (htCode == 0)
                return;

            var hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            if (hwndSource != null)
            {
                ReleaseCapture();
                SendMessage(hwndSource.Handle, WM_NCLBUTTONDOWN, (IntPtr)htCode, IntPtr.Zero);
            }
        }
    }
}
