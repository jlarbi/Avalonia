using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Controls.Generators
{
    /// <summary>
    /// Definition of the <see cref="MenuItemContainerGenerator"/> class.
    /// </summary>
    public class MenuItemContainerGenerator : ItemContainerGenerator<MenuItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainerGenerator{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        public MenuItemContainerGenerator(IControl owner)
            : base(owner, MenuItem.HeaderProperty, null)
        {
        }

        /// <summary>
        /// Creates the container for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The created container control.</returns>
        protected override IControl CreateContainer(object item)
        {
            var separator = item as Separator;
            return separator != null ? separator : base.CreateContainer(item);
        }
    }
}
