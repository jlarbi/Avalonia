using Avalonia;
using Avalonia.Markup.Xaml;

namespace ControlCatalog
{
    /// <summary>
    /// Definition of the <see cref="App"/> class.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Initializes the application.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
