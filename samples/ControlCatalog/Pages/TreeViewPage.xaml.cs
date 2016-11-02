using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    /// <summary>
    /// Definition of the <see cref="TreeViewPage"/> class.
    /// </summary>
    public class TreeViewPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewPage"/> class.
        /// </summary>
        public TreeViewPage()
        {
            this.InitializeComponent();
            DataContext = CreateNodes(0);
        }

        /// <summary>
        /// Initializes the tree view page.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Creates all the tree nodes.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private IList<Node> CreateNodes(int level)
        {
            return Enumerable.Range(0, 10).Select(x => new Node
            {
                Header = $"Item {x}",
                Children = level < 5 ? CreateNodes(level + 1) : null,
            }).ToList();
        }

        /// <summary>
        /// Definition of the <see cref="Node"/> class.
        /// </summary>
        private class Node
        {
            public string Header { get; set; }
            public IList<Node> Children { get; set; }
        }
    }
}
