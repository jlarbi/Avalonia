using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="SliderPage"/> class.
    /// </summary>
    public class SliderPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SliderPage"/> class.
        /// </summary>
        public SliderPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the slider page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
