using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="ImagePage"/> class.
    /// </summary>
    public class ImagePage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePage"/> class.
        /// </summary>
        public ImagePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the image page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
