// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Specialized;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using static Avalonia.Utilities.MathUtilities;

namespace Avalonia.Controls.Presenters
{
    /// <summary>
    /// Displays items inside an <see cref="ItemsControl"/>.
    /// </summary>
    public class ItemsPresenter : ItemsPresenterBase, ILogicalScrollable
    {
        /// <summary>
        /// Defines the <see cref="VirtualizationMode"/> property.
        /// </summary>
        public static readonly StyledProperty<ItemVirtualizationMode> VirtualizationModeProperty =
            AvaloniaProperty.Register<ItemsPresenter, ItemVirtualizationMode>(
                nameof(VirtualizationMode),
                defaultValue: ItemVirtualizationMode.None);

        private ItemVirtualizer _virtualizer;

        /// <summary>
        /// Initializes static members of the <see cref="ItemsPresenter"/> class.
        /// </summary>
        static ItemsPresenter()
        {
            KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue(
                typeof(ItemsPresenter),
                KeyboardNavigationMode.Once);

            VirtualizationModeProperty.Changed
                .AddClassHandler<ItemsPresenter>(x => x.VirtualizationModeChanged);
        }

        /// <summary>
        /// Gets or sets the virtualization mode for the items.
        /// </summary>
        public ItemVirtualizationMode VirtualizationMode
        {
            get { return GetValue(VirtualizationModeProperty); }
            set { SetValue(VirtualizationModeProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether logical scrolling is enabled on the control.
        /// </summary>
        bool ILogicalScrollable.IsLogicalScrollEnabled
        {
            get { return _virtualizer?.IsLogicalScrollEnabled ?? false; }
        }

        /// <summary>
        /// Gets the extent of the scrollable content, in logical units
        /// </summary>
        Size IScrollable.Extent => _virtualizer.Extent;

        /// <summary>
        /// Gets or sets the current scroll offset, in logical units.
        /// </summary>
        Vector IScrollable.Offset
        {
            get { return _virtualizer.Offset; }
            set { _virtualizer.Offset = CoerceOffset(value); }
        }

        /// <summary>
        /// Gets the size of the viewport, in logical units.
        /// </summary>
        Size IScrollable.Viewport => _virtualizer.Viewport;

        /// <summary>
        /// Gets or sets the scroll invalidation method.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method notifies the attached <see cref="ScrollViewer"/> of a change in 
        /// the <see cref="IScrollable.Extent"/>, <see cref="IScrollable.Offset"/> or 
        /// <see cref="IScrollable.Viewport"/> properties.
        /// </para>
        /// <para>
        /// This property is set by the parent <see cref="ScrollViewer"/> when the 
        /// <see cref="ILogicalScrollable"/> is placed inside it.
        /// </para>
        /// </remarks>
        Action ILogicalScrollable.InvalidateScroll { get; set; }

        /// <summary>
        /// Gets the size to scroll by, in logical units.
        /// </summary>
        Size ILogicalScrollable.ScrollSize => new Size(1, 1);

        /// <summary>
        /// Gets the size to page by, in logical units.
        /// </summary>
        Size ILogicalScrollable.PageScrollSize => new Size(0, 1);

        /// <summary>
        /// Attempts to bring a portion of the target visual into view by scrolling the content.
        /// </summary>
        /// <param name="target">The target visual.</param>
        /// <param name="targetRect">The portion of the target visual to bring into view.</param>
        /// <returns>True if the scroll offset was changed; otherwise false.</returns>
        bool ILogicalScrollable.BringIntoView(IControl target, Rect targetRect)
        {
            return false;
        }

        /// <summary>
        /// Gets the next control in the specified direction.
        /// </summary>
        /// <param name="direction">The movement direction.</param>
        /// <param name="from">The control from which movement begins.</param>
        /// <returns>The control.</returns>
        IControl ILogicalScrollable.GetControlInDirection(NavigationDirection direction, IControl from)
        {
            return _virtualizer?.GetControlInDirection(direction, from);
        }

        /// <summary>
        /// Scrolls into the view until the given item.
        /// </summary>
        /// <param name="item">The item to reach.</param>
        public override void ScrollIntoView(object item)
        {
            _virtualizer?.ScrollIntoView(item);
        }

        /// <summary>
        /// Measures the control and its child elements as part of a layout pass.
        /// </summary>
        /// <param name="availableSize">The size available to the control.</param>
        /// <returns>The desired size for the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            return _virtualizer?.MeasureOverride(availableSize) ?? Size.Empty;
        }

        /// <summary>
        /// Positions child elements as part of a layout pass.
        /// </summary>
        /// <param name="finalSize">The size available to the control.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            return _virtualizer?.ArrangeOverride(finalSize) ?? Size.Empty;
        }

        /// <summary>
        /// Called when the <see cref="Panel"/> is created.
        /// </summary>
        /// <param name="panel">The panel.</param>
        protected override void PanelCreated(IPanel panel)
        {
            _virtualizer = ItemVirtualizer.Create(this);
            ((ILogicalScrollable)this).InvalidateScroll?.Invoke();

            if (!Panel.IsSet(KeyboardNavigation.DirectionalNavigationProperty))
            {
                KeyboardNavigation.SetDirectionalNavigation(
                    (InputElement)Panel,
                    KeyboardNavigationMode.Contained);
            }

            KeyboardNavigation.SetTabNavigation(
                (InputElement)Panel,
                KeyboardNavigation.GetTabNavigation(this));
        }

        /// <summary>
        /// Called when the items for the presenter change, either because items
        /// has been set, the items collection has been modified, or the panel has been created.
        /// </summary>
        /// <param name="e">A description of the change.</param>
        protected override void ItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            _virtualizer?.ItemsChanged(Items, e);
        }

        /// <summary>
        /// Ensures an offset value is within the value range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The coerced value.</returns>
        private Vector CoerceOffset(Vector value)
        {
            var scrollable = (ILogicalScrollable)this;
            var maxX = Math.Max(scrollable.Extent.Width - scrollable.Viewport.Width, 0);
            var maxY = Math.Max(scrollable.Extent.Height - scrollable.Viewport.Height, 0);
            return new Vector(Clamp(value.X, 0, maxX), Clamp(value.Y, 0, maxY));
        }

        /// <summary>
        /// Delegate called on virtualization mode changes.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void VirtualizationModeChanged(AvaloniaPropertyChangedEventArgs e)
        {
            _virtualizer?.Dispose();
            _virtualizer = ItemVirtualizer.Create(this);
            ((ILogicalScrollable)this).InvalidateScroll?.Invoke();
        }
    }
}