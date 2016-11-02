using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="ExpanderPage"/> class.
    /// </summary>
    public class ExpanderPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpanderPage"/> class.
        /// </summary>
        public ExpanderPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the expander page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
