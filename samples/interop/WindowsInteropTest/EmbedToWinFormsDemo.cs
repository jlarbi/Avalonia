using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls;
using ControlCatalog;

namespace WindowsInteropTest
{
    /// <summary>
    /// Definition of the <see cref="EmbedToWinFormsDemo"/> class.
    /// </summary>
    public partial class EmbedToWinFormsDemo : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedToWinFormsDemo"/> class.
        /// </summary>
        public EmbedToWinFormsDemo()
        {
            InitializeComponent();
            avaloniaHost.Content = new MainView();
        }
    }
}
