using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="ToolTipPage"/> class.
    /// </summary>
    public class ToolTipPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTipPage"/> class.
        /// </summary>
        public ToolTipPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the tooltip page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
