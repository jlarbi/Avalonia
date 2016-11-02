using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="TextBoxPage"/> class.
    /// </summary>
    public class TextBoxPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxPage"/> class.
        /// </summary>
        public TextBoxPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the textbox page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
