using CubeKit.Flyouts.Transparency;
using Windows.Win32;
using WinRT.Interop;
using WinUIEx;
using WinUIEx.Messaging;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CubeKit.Flyouts
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class BaseWindow : WindowEx
    {
        WindowMessageMonitor m;
        HWND Handle;
        WINDOW_EX_STYLE ExStyle
        {
            get => (WINDOW_EX_STYLE)PInvoke.GetWindowLong(Handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
            set => _ = PInvoke.SetWindowLong(Handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (int)value);
        }

        public BaseWindow()
        {
            this.InitializeComponent();
            m = new(this);
            Handle = new HWND(WindowNative.GetWindowHandle(this));
            ExStyle |= WINDOW_EX_STYLE.WS_EX_LAYERED;
            m.WindowMessageReceived += WindowMessageReceived;

            SystemBackdrop = new TransparentBackdrop();
        }

        private void WindowMessageReceived(object? sender, WindowMessageEventArgs e)
        {
            if (e.Message.MessageId == PInvoke.WM_ERASEBKGND)
            {
                e.Handled = true;
                e.Result = 1;
            }
        }
    }
}
