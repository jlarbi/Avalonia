using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="DropDownPage"/> class.
    /// </summary>
    public class DropDownPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownPage"/> class.
        /// </summary>
        public DropDownPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the drop down page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
