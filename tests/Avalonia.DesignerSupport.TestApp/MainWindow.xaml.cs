using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.DesignerSupport.TestApp
{
    /// <summary>
    /// Definition of the <see cref="MainWindow"/> class.
    /// </summary>
    public class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
