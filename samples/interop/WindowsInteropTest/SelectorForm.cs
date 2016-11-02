using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsInteropTest
{
    /// <summary>
    /// Definition of the <see cref="SelectorForm"/> class.
    /// </summary>
    public partial class SelectorForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorForm"/> class.
        /// </summary>
        public SelectorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delegate called on buttonembed to winform click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmbedToWinForms_Click(object sender, EventArgs e)
        {
            new EmbedToWinFormsDemo().ShowDialog(this);
        }

        /// <summary>
        /// Delegate called on buttonembed to Wpf click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmbedToWpf_Click(object sender, EventArgs e)
        {
            new EmbedToWpfDemo().ShowDialog();
        }
    }
}
