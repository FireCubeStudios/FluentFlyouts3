using System;
using Microsoft.UI.Xaml.Media;
using Compositor = Windows.UI.Composition.Compositor;
using Windows.UI.Composition;
using ICompositionSupportsSystemBackdrop = Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop;

namespace CubeKit.Flyouts.Transparency
{
    internal class TransparentBackdrop : SystemBackdrop
    {
        static readonly Lazy<Compositor> _Compositor = new(() =>
        {
            WindowsSystemDispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
            return new();
        });
        static Compositor Compositor => _Compositor.Value;
        protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, Microsoft.UI.Xaml.XamlRoot xamlRoot)
        {
            connectedTarget.SystemBackdrop = Compositor.CreateColorBrush(
                Windows.UI.Color.FromArgb(0, 255, 255, 255)
            );
        }
        protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
        {
            disconnectedTarget.SystemBackdrop = null;
        }
    }
}
