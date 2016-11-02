using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="RadioButtonPage"/> class.
    /// </summary>
    public class RadioButtonPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonPage"/> class.
        /// </summary>
        public RadioButtonPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the radio button page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
