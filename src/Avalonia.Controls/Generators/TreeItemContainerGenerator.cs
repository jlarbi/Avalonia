// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Templates;
using Avalonia.Data;

namespace Avalonia.Controls.Generators
{
    /// <summary>
    /// Creates containers for tree items and maintains a list of created containers.
    /// </summary>
    /// <typeparam name="T">The type of the container.</typeparam>
    public class TreeItemContainerGenerator<T> : ItemContainerGenerator<T>, ITreeItemContainerGenerator
        where T : class, IControl, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItemContainerGenerator{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        /// <param name="contentProperty">The container's Content property.</param>
        /// <param name="contentTemplateProperty">The container's ContentTemplate property.</param>
        /// <param name="itemsProperty">The container's Items property.</param>
        /// <param name="isExpandedProperty">The container's IsExpanded property.</param>
        /// <param name="index">The container index for the tree</param>
        public TreeItemContainerGenerator(
            IControl owner,
            AvaloniaProperty contentProperty,
            AvaloniaProperty contentTemplateProperty,
            AvaloniaProperty itemsProperty,
            AvaloniaProperty isExpandedProperty,
            TreeContainerIndex index)
            : base(owner, contentProperty, contentTemplateProperty)
        {
            Contract.Requires<ArgumentNullException>(owner != null);
            Contract.Requires<ArgumentNullException>(contentProperty != null);
            Contract.Requires<ArgumentNullException>(itemsProperty != null);
            Contract.Requires<ArgumentNullException>(isExpandedProperty != null);
            Contract.Requires<ArgumentNullException>(index != null);

            ItemsProperty = itemsProperty;
            IsExpandedProperty = isExpandedProperty;
            Index = index;
        }

        /// <summary>
        /// Gets the container index for the tree.
        /// </summary>
        public TreeContainerIndex Index { get; }

        /// <summary>
        /// Gets the item container's Items property.
        /// </summary>
        protected AvaloniaProperty ItemsProperty { get; }

        /// <summary>
        /// Gets the item container's IsExpanded property.
        /// </summary>
        protected AvaloniaProperty IsExpandedProperty { get; }

        /// <summary>
        /// Creates the container for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The created container control.</returns>
        protected override IControl CreateContainer(object item)
        {
            var container = item as T;

            if (item == null)
            {
                return null;
            }
            else if (container != null)
            {
                Index.Add(item, container);
                return container;
            }
            else
            {
                var template = GetTreeDataTemplate(item, ItemTemplate);
                var result = new T();

                result.SetValue(ContentProperty, template.Build(item), BindingPriority.Style);

                var itemsSelector = template.ItemsSelector(item);

                if (itemsSelector != null)
                {
                    BindingOperations.Apply(result, ItemsProperty, itemsSelector, null);
                }

                if (!(item is IControl))
                {
                    result.DataContext = item;
                }

                NameScope.SetNameScope((Control)(object)result, new NameScope());
                Index.Add(item, result);

                return result;
            }
        }

        /// <summary>
        /// Clears all created containers and returns the removed controls.
        /// </summary>
        /// <returns>The removed controls.</returns>
        public override IEnumerable<ItemContainerInfo> Clear()
        {
            var items = base.Clear();
            Index.Remove(0, items);
            return items;
        }

        /// <summary>
        /// Removes a set of created containers.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item in the control's items.
        /// </param>
        /// <param name="count">The the number of items to remove.</param>
        /// <returns>The removed containers.</returns>
        public override IEnumerable<ItemContainerInfo> Dematerialize(int startingIndex, int count)
        {
            Index.Remove(startingIndex, GetContainerRange(startingIndex, count));
            return base.Dematerialize(startingIndex, count);
        }

        /// <summary>
        /// Removes a set of created containers and updates the index of later containers to fill
        /// the gap.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item in the control's items.
        /// </param>
        /// <param name="count">The the number of items to remove.</param>
        /// <returns>The removed containers.</returns>
        public override IEnumerable<ItemContainerInfo> RemoveRange(int startingIndex, int count)
        {
            Index.Remove(startingIndex, GetContainerRange(startingIndex, count));
            return base.RemoveRange(startingIndex, count);
        }

        /// <summary>
        /// Attempts to recycle the given element by moving it from a position to another.
        /// </summary>
        /// <param name="oldIndex">The old position.</param>
        /// <param name="newIndex">The new position.</param>
        /// <param name="item">The item to recycle.</param>
        /// <param name="selector">The member selector if needed.</param>
        /// <returns>True if recycled, false otherwise.</returns>
        public override bool TryRecycle(int oldIndex, int newIndex, object item, IMemberSelector selector)
        {
            return false;
        }

        /// <summary>
        /// Gets the data template used for the given item to decorate it.
        /// </summary>
        /// <param name="item">The item to look the data template for.</param>
        /// <param name="primary"></param>
        /// <returns>The found or default data template.</returns>
        private ITreeDataTemplate GetTreeDataTemplate(object item, IDataTemplate primary)
        {
            var template = Owner.FindDataTemplate(item, primary) ?? FuncDataTemplate.Default;
            var treeTemplate = template as ITreeDataTemplate ??
                new FuncTreeDataTemplate(typeof(object), template.Build, x => null);
            return treeTemplate;
        }
    }
}
