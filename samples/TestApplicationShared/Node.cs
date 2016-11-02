using Avalonia.Collections;

namespace TestApplication
{
    /// <summary>
    /// Definition of the <see cref="Node"/> class.
    /// </summary>
    internal class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
            Children = new AvaloniaList<Node>();
        }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the node's children.
        /// </summary>
        public AvaloniaList<Node> Children { get; set; }
    }

}
