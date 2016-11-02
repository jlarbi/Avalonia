using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="BorderPage"/> class.
    /// </summary>
    public class BorderPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BorderPage"/> class.
        /// </summary>
        public BorderPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the border page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
