using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="ButtonPage"/> class.
    /// </summary>
    public class ButtonPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPage"/> class.
        /// </summary>
        public ButtonPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the button page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
