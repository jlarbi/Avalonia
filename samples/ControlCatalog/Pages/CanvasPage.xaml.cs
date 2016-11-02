using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="CanvasPage"/> class.
    /// </summary>
    public class CanvasPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasPage"/> class.
        /// </summary>
        public CanvasPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the canvas page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
