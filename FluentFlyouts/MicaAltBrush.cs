using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Composition;
using WinUIEx;

namespace FluentFlyouts
{
	public enum BackdropKind
	{
		Base = 0,
		BaseAlt = 1,
		Custom = 2
	}

	public class MicaAltBrush : XamlCompositionBrushBase, INotifyPropertyChanged
	{
		private double tintOpacity;

		public double TintOpacity
		{
			get => tintOpacity;
			set => tintOpacity = value;
		}

		private double luminosityOpacity;

		public double LuminosityOpacity
		{
			get => luminosityOpacity;
			set => luminosityOpacity = value;
		}

		private Color tintColor;

		public Color TintColor
		{
			get => tintColor;
			set => tintColor = value;
		}

		private Color luminosityColor;

		public Color LuminosityColor
		{
			get => luminosityColor;
			set => luminosityColor = value;
		}

		private ElementTheme _theme;

		public ElementTheme Theme
		{
			get => _theme;
			set
			{
				_theme = value;
				UpdateTheme();
			}
		}

		private BackdropKind _kind;

		public int Kind
		{
			get => (int)_kind;
			set
			{
				_kind = (BackdropKind)value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsKindCustom)));
				UpdateTheme();
			}
		}

		public bool IsKindCustom => _kind == BackdropKind.Custom;

		private void UpdateTheme()
		{
			switch (_kind)
			{
				case BackdropKind.Custom:
					return;
				case BackdropKind.Base:
					switch (Theme)
					{
						case ElementTheme.Light:
							tintColor = luminosityColor = Color.FromArgb(255, 243, 243, 243);
							tintOpacity = 50;
							luminosityOpacity = 100;
							break;
						case ElementTheme.Dark:
							tintColor = luminosityColor = Color.FromArgb(255, 32, 32, 32);
							tintOpacity = 80;
							luminosityOpacity = 100;
							break;
					}
					break;
				case BackdropKind.BaseAlt:
					switch (Theme)
					{
						case ElementTheme.Light:
							tintColor = luminosityColor = Color.FromArgb(255, 218, 218, 218);
							tintOpacity = 50;
							luminosityOpacity = 100;
							break;
						case ElementTheme.Dark:
							tintColor = luminosityColor = Color.FromArgb(255, 10, 10, 10);
							tintOpacity = 0;
							luminosityOpacity = 100;
							break;
					}
					break;
			}

			UpdateBrush();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TintColor)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TintOpacity)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LuminosityColor)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LuminosityOpacity)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public static CompositionBrush BuildMicaEffectBrush(Compositor compositor, Color tintColor, float tintOpacity, Color luminosityColor, float luminosityOpacity)
		{
			// Tint Color.

			var tintColorEffect = new ColorSourceEffect();
			tintColorEffect.Name = "TintColor";
			tintColorEffect.Color = tintColor;

			// OpacityEffect applied to Tint.
			var tintOpacityEffect = new OpacityEffect();
			tintOpacityEffect.Name = "TintOpacity";
			tintOpacityEffect.Opacity = tintOpacity;
			tintOpacityEffect.Source = tintColorEffect;

			// Apply Luminosity:

			// Luminosity Color.
			var luminosityColorEffect = new ColorSourceEffect();
			luminosityColorEffect.Color = luminosityColor;

			// OpacityEffect applied to Luminosity.
			var luminosityOpacityEffect = new OpacityEffect();
			luminosityOpacityEffect.Name = "LuminosityOpacity";
			luminosityOpacityEffect.Opacity = luminosityOpacity;
			luminosityOpacityEffect.Source = luminosityColorEffect;

			// Luminosity Blend.
			// NOTE: There is currently a bug where the names of BlendEffectMode::Luminosity and BlendEffectMode::Color are flipped.
			var luminosityBlendEffect = new BlendEffect();
			luminosityBlendEffect.Mode = BlendEffectMode.Color;
			luminosityBlendEffect.Background = new CompositionEffectSourceParameter("BlurredWallpaperBackdrop");
			luminosityBlendEffect.Foreground = luminosityOpacityEffect;

			// Apply Tint:

			// Color Blend.
			// NOTE: There is currently a bug where the names of BlendEffectMode::Luminosity and BlendEffectMode::Color are flipped.
			var colorBlendEffect = new BlendEffect();
			colorBlendEffect.Mode = BlendEffectMode.Luminosity;
			colorBlendEffect.Background = luminosityBlendEffect;
			colorBlendEffect.Foreground = tintOpacityEffect;

			//CompositionEffectBrush micaEffectBrush = compositor.CreateEffectFactory(colorBlendEffect).CreateBrush();
			CompositionBackdropBrush micaEffectBrush = compositor.CreateHostBackdropBrush();

			//micaEffectBrush.SetSourceParameter("BlurredWallpaperBackdrop", compositor.TryCreateBlurredWallpaperBackdropBrush());
			
			return micaEffectBrush;
		}

		private CompositionBrush CreateCrossFadeEffectBrush(Compositor compositor, CompositionBrush from, CompositionBrush to)
		{
			var crossFadeEffect = new CrossFadeEffect();
			crossFadeEffect.Name = "Crossfade"; // Name to reference when starting the animation.
			crossFadeEffect.Source1 = new CompositionEffectSourceParameter("source1");
			crossFadeEffect.Source2 = new CompositionEffectSourceParameter("source2");
			crossFadeEffect.CrossFade = 0;

			CompositionEffectBrush crossFadeEffectBrush = compositor.CreateEffectFactory(crossFadeEffect, new List<string>() { "Crossfade.CrossFade" }).CreateBrush();
			crossFadeEffectBrush.Comment = "Crossfade";
			// The inputs have to be swapped here to work correctly...
			crossFadeEffectBrush.SetSourceParameter("source1", to);
			crossFadeEffectBrush.SetSourceParameter("source2", from);
			return crossFadeEffectBrush;
		}

		private ScalarKeyFrameAnimation CreateCrossFadeAnimation(Compositor compositor)
		{
			ScalarKeyFrameAnimation animation = compositor.CreateScalarKeyFrameAnimation();
			LinearEasingFunction linearEasing = compositor.CreateLinearEasingFunction();
			animation.InsertKeyFrame(0.0f, 0.0f, linearEasing);
			animation.InsertKeyFrame(1.0f, 1.0f, linearEasing);
			animation.Duration = TimeSpan.FromMilliseconds(250);
			return animation;
		}

		private void UpdateBrush()
		{
		/*	Compositor compositor = Window.Current.Compositor;

			CompositionBrush newBrush = BuildMicaEffectBrush(compositor, TintColor, (float)(TintOpacity / 100), LuminosityColor, (float)(LuminosityOpacity / 100));

			CompositionBrush oldBrush = CompositionBrush;

			if (oldBrush == null || CompositionBrush.Comment == "Crossfade" || Kind == (int)BackdropKind.Custom)
			{
				// Set new brush directly
				if (oldBrush != null)
				{
					oldBrush.Dispose();
				}
				this.CompositionBrush = newBrush;
			}
			else
			{
				// Crossfade
				CompositionBrush crossFadeBrush = CreateCrossFadeEffectBrush(compositor, oldBrush, newBrush);
				ScalarKeyFrameAnimation animation = CreateCrossFadeAnimation(compositor);
				CompositionBrush = crossFadeBrush;

				var crossFadeAnimationBatch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
				crossFadeBrush.StartAnimation("CrossFade.CrossFade", animation);
				crossFadeAnimationBatch.End();

				crossFadeAnimationBatch.Completed += (o, a) =>
				{
					crossFadeBrush.Dispose();
					oldBrush.Dispose();
					this.CompositionBrush = newBrush;
				};
			}*/
		}

		protected override void OnConnected()
		{
			//if (DesignMode.DesignModeEnabled)
			//{
			var compositor = new Microsoft.UI.Composition.Compositor();
			//this.CompositionBrush = compositor.CreateHostBackdropBrush();
			this.CompositionBrush = compositor.CreateColorBrush(Color.FromArgb(255, 243, 243, 243));
				return;
			//}

			//UpdateBrush();
		}
	}
}
