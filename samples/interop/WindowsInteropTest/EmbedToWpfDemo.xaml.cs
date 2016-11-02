using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Avalonia.Controls;
using ControlCatalog;
using Window = System.Windows.Window;

namespace WindowsInteropTest
{
    /// <summary>
    /// Interaction logic for EmbedToWpfDemo.xaml
    /// </summary>
    public partial class EmbedToWpfDemo : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedToWpfDemo"/> class.
        /// </summary>
        public EmbedToWpfDemo()
        {
            InitializeComponent();
            Host.Content =  new MainView();
        }
    }
}
