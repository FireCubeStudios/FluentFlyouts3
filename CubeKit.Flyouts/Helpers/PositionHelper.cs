using Microsoft.UI.Windowing;
using WinUIEx;

namespace CubeKit.Flyouts.Helpers
{
    public class PositionHelper
    {
        public static void Positionflyout(BaseWindow Flyout)
        {
            var DisplayBounds = DisplayArea.Primary.OuterBounds;
            int DisplayHeight = DisplayBounds.Height;
            int DisplayWidth = DisplayBounds.Width;

            double W = Flyout.Width;
            double H = Flyout.Height;
            // 1.17
            Flyout.MoveAndResize((DisplayWidth / 1.17) - (W / 2), (DisplayHeight / 1.17) - (H / 2), W, H);
        }
    }
}
