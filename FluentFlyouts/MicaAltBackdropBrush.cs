using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace FluentFlyouts
{
	public class MicaAltBackdropBrush : CompositionBrushBackdrop
	{
		protected override Windows.UI.Composition.CompositionBrush CreateBrush(Windows.UI.Composition.Compositor compositor)
			=> compositor.CreateHostBackdropBrush();
	}
}
