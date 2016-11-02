using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="CheckBoxPage"/> class.
    /// </summary>
    public class CheckBoxPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxPage"/> class.
        /// </summary>
        public CheckBoxPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the checkbox page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
