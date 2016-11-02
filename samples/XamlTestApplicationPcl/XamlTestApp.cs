using Avalonia;
using Avalonia.Markup.Xaml;
using XamlTestApplication.Views;

namespace XamlTestApplication
{
    /// <summary>
    /// Definition of the <see cref="XamlTestApp"/> class.
    /// </summary>
    public class XamlTestApp : Application
    {
        /// <summary>
        /// Initializes the Xaml test application.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
