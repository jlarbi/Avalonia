// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;

namespace Avalonia.Controls.Generators
{
    /// <summary>
    /// Creates containers for items and maintains a list of created containers.
    /// </summary>
    public class ItemContainerGenerator : IItemContainerGenerator
    {
        private Dictionary<int, ItemContainerInfo> _containers = new Dictionary<int, ItemContainerInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainerGenerator"/> class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        public ItemContainerGenerator(IControl owner)
        {
            Contract.Requires<ArgumentNullException>(owner != null);

            Owner = owner;
        }

        /// <summary>
        /// Gets the currently realized containers.
        /// </summary>
        public IEnumerable<ItemContainerInfo> Containers => _containers.Values;

        /// <summary>
        /// Signalled whenever new containers are materialized.
        /// </summary>
        public event EventHandler<ItemContainerEventArgs> Materialized;

        /// <summary>
        /// Event raised whenever containers are dematerialized.
        /// </summary>
        public event EventHandler<ItemContainerEventArgs> Dematerialized;

        /// <summary>
        /// Event raised whenever containers are recycled.
        /// </summary>
        public event EventHandler<ItemContainerEventArgs> Recycled;

        /// <summary>
        /// Gets or sets the data template used to display the items in the control.
        /// </summary>
        public IDataTemplate ItemTemplate { get; set; }

        /// <summary>
        /// Gets the owner control.
        /// </summary>
        public IControl Owner { get; }

        /// <summary>
        /// Creates a container control for an item.
        /// </summary>
        /// <param name="index">
        /// The index of the item of data in the control's items.
        /// </param>
        /// <param name="item">The item.</param>
        /// <param name="selector">An optional member selector.</param>
        /// <returns>The created controls.</returns>
        public ItemContainerInfo Materialize(
            int index,
            object item,
            IMemberSelector selector)
        {
            var i = selector != null ? selector.Select(item) : item;
            var container = new ItemContainerInfo(CreateContainer(i), item, index);

            _containers.Add(container.Index, container);
            Materialized?.Invoke(this, new ItemContainerEventArgs(container));

            return container;
        }

        /// <summary>
        /// Removes a set of created containers.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item in the control's items.
        /// </param>
        /// <param name="count">The the number of items to remove.</param>
        /// <returns>The removed containers.</returns>
        public virtual IEnumerable<ItemContainerInfo> Dematerialize(int startingIndex, int count)
        {
            var result = new List<ItemContainerInfo>();

            for (int i = startingIndex; i < startingIndex + count; ++i)
            {
                result.Add(_containers[i]);
                _containers.Remove(i);
            }

            Dematerialized?.Invoke(this, new ItemContainerEventArgs(startingIndex, result));

            return result;
        }

        /// <summary>
        /// Inserts space for newly inserted containers in the index.
        /// </summary>
        /// <param name="index">The index at which space should be inserted.</param>
        /// <param name="count">The number of blank spaces to create.</param>
        public virtual void InsertSpace(int index, int count)
        {
            if (count > 0)
            {
                var toMove = _containers.Where(x => x.Key >= index)
                    .OrderByDescending(x => x.Key)
                    .ToList();

                foreach (var i in toMove)
                {
                    _containers.Remove(i.Key);
                    i.Value.Index += count;
                    _containers.Add(i.Value.Index, i.Value);
                }
            }
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
        public virtual IEnumerable<ItemContainerInfo> RemoveRange(int startingIndex, int count)
        {
            var result = new List<ItemContainerInfo>();

            if (count > 0)
            {
                for (var i = startingIndex; i < startingIndex + count; ++i)
                {
                    ItemContainerInfo found;

                    if (_containers.TryGetValue(i, out found))
                    {
                        result.Add(found);
                    }

                    _containers.Remove(i);
                }

                var toMove = _containers.Where(x => x.Key >= startingIndex)
                                        .OrderBy(x => x.Key).ToList();

                foreach (var i in toMove)
                {
                    _containers.Remove(i.Key);
                    i.Value.Index -= count;
                    _containers.Add(i.Value.Index, i.Value);
                }

                Dematerialized?.Invoke(this, new ItemContainerEventArgs(startingIndex, result));
            }

            return result;
        }

        /// <summary>
        /// Attempts to recycle the given element by moving it from a position to another.
        /// </summary>
        /// <param name="oldIndex">The old position.</param>
        /// <param name="newIndex">The new position.</param>
        /// <param name="item">The item to recycle.</param>
        /// <param name="selector">The member selector if needed.</param>
        /// <returns>True if recycled, false otherwise.</returns>
        public virtual bool TryRecycle(
            int oldIndex,
            int newIndex,
            object item,
            IMemberSelector selector)
        {
            return false;
        }

        /// <summary>
        /// Clears all created containers and returns the removed controls.
        /// </summary>
        /// <returns>The removed controls.</returns>
        public virtual IEnumerable<ItemContainerInfo> Clear()
        {
            var result = Containers.ToList();
            _containers.Clear();

            if (result.Count > 0)
            {
                Dematerialized?.Invoke(this, new ItemContainerEventArgs(0, result));
            }

            return result;
        }

        /// <summary>
        /// Gets the container control representing the item with the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The container, or null if no container created.</returns>
        public IControl ContainerFromIndex(int index)
        {
            ItemContainerInfo result;
            _containers.TryGetValue(index, out result);
            return result?.ContainerControl;
        }

        /// <summary>
        /// Gets the index of the specified container control.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The index of the container, or -1 if not found.</returns>
        public int IndexFromContainer(IControl container)
        {
            foreach (var i in _containers)
            {
                if (i.Value.ContainerControl == container)
                {
                    return i.Key;
                }
            }

            return -1;
        }

        /// <summary>
        /// Creates the container for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The created container control.</returns>
        protected virtual IControl CreateContainer(object item)
        {
            var result = item as IControl;

            if (result == null)
            {
                result = new ContentPresenter();
                result.SetValue(ContentPresenter.ContentProperty, item, BindingPriority.Style);

                if (ItemTemplate != null)
                {
                    result.SetValue(
                        ContentPresenter.ContentTemplateProperty,
                        ItemTemplate,
                        BindingPriority.TemplatedParent);
                }
            }

            return result;
        }

        /// <summary>
        /// Moves a container.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="item">The new item.</param>
        /// <returns>The container info.</returns>
        protected ItemContainerInfo MoveContainer(int oldIndex, int newIndex, object item)
        {
            var container = _containers[oldIndex];
            container.Index = newIndex;
            container.Item = item;
            _containers.Remove(oldIndex);
            _containers.Add(newIndex, container);
            return container;
        }

        /// <summary>
        /// Gets all containers with an index that fall within a range.
        /// </summary>
        /// <param name="index">The first index.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>The containers.</returns>
        protected IEnumerable<ItemContainerInfo> GetContainerRange(int index, int count)
        {
            return _containers.Where(x => x.Key >= index && x.Key < index + count).Select(x => x.Value);
        }

        /// <summary>
        /// Raises the <see cref="Recycled"/> event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected void RaiseRecycled(ItemContainerEventArgs e)
        {
            Recycled?.Invoke(this, e);
        }
    }
}