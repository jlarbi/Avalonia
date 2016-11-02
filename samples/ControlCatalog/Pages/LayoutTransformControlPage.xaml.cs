using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="LayoutTransformControlPage"/> class.
    /// </summary>
    public class LayoutTransformControlPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTransformControlPage"/> class.
        /// </summary>
        public LayoutTransformControlPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the layout transform control page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
