using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BindingTest
{
    /// <summary>
    /// Definition of the <see cref="TestItemView"/> class.
    /// </summary>
    public class TestItemView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestItemView"/> class.
        /// </summary>
        public TestItemView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the test item view.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
