using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CubeKit.UI.Icons
{
    /// <summary>
    /// Represents an icon source that uses a Fluent System Icon as its content.
    /// </summary>
    public class FluentIconSource : PathIconSource
    {
        /// <summary>
        /// Constructs an empty <see cref="FluentIconSource"/>.
        /// </summary>
        public FluentIconSource() { }

        /// <summary>
        /// Constructs an <see cref="IconSource"/> that uses a Fluent System Icon as its content.
        /// </summary>
        public FluentIconSource(FluentSymbol symbol)
        {
            Symbol = symbol;
        }

        /// <summary>
        /// Gets or sets the Fluent System Icons glyph used as the icon content.
        /// </summary>
        public FluentSymbol Symbol
        {
            get { return (FluentSymbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Symbol"/> property.
        /// </summary>
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            nameof(Symbol), typeof(FluentSymbol), typeof(FluentIconSource),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSymbolChanged))
        );

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentIconSource self && (e.NewValue is FluentSymbol || e.NewValue is int))
            {
                // Set internal Data to the Path from the look-up table
                self.Data = FluentSymbolIcon.GetPathData((FluentSymbol)e.NewValue);
            }
        }
    }
}
