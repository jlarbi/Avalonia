// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Specialized;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="VirtualizingStackPanel"/> class.
    /// </summary>
    public class VirtualizingStackPanel : StackPanel, IVirtualizingPanel
    {
        private Size _availableSpace;
        private double _takenSpace;
        private int _canBeRemoved;
        private double _averageItemSize;
        private int _averageCount;
        private double _pixelOffset;
        private bool _forceRemeasure;

        /// <summary>
        /// Gets a value indicating whether the panel is full.
        /// </summary>
        /// <remarks>
        /// This property should return false until enough children are added to fill the space
        /// passed into the last measure or arrange in the direction of scroll. It should be
        /// updated immediately after a child is added or removed.
        /// </remarks>
        bool IVirtualizingPanel.IsFull
        {
            get
            {
                return Orientation == Orientation.Horizontal ?
                    _takenSpace >= _availableSpace.Width :
                    _takenSpace >= _availableSpace.Height;
            }
        }

        /// <summary>
        /// Gets or sets the controller for the virtualizing panel.
        /// </summary>
        /// <remarks>
        /// A virtualizing controller is responsible for maintaing the controls in the virtualizing
        /// panel. This property will be set by the controller when virtualization is initialized.
        /// Note that this property may remain null if the panel is added to a control that does
        /// not act as a virtualizing controller.
        /// </remarks>
        IVirtualizingController IVirtualizingPanel.Controller { get; set; }

        /// <summary>
        /// Gets the number of items that can be removed while keeping the panel full.
        /// </summary>
        /// <remarks>
        /// This property should return the number of children that are completely out of the
        /// panel's current bounds in the direction of scroll. It should be updated after an
        /// arrange.
        /// </remarks>
        int IVirtualizingPanel.OverflowCount => _canBeRemoved;

        /// <summary>
        /// Gets the direction of scroll.
        /// </summary>
        Orientation IVirtualizingPanel.ScrollDirection => Orientation;

        /// <summary>
        /// Gets the average size of the materialized items in the direction of scroll.
        /// </summary>
        double IVirtualizingPanel.AverageItemSize => _averageItemSize;

        /// <summary>
        /// Gets or sets a size in pixels by which the content is overflowing the panel, in the
        /// direction of scroll.
        /// </summary>
        /// <remarks>
        /// This may be non-zero even when <see cref="IVirtualizingPanel.OverflowCount"/> is zero if the last item
        /// overflows the panel bounds.
        /// </remarks>
        double IVirtualizingPanel.PixelOverflow
        {
            get
            {
                var bounds = Orientation == Orientation.Horizontal ? 
                    _availableSpace.Width : _availableSpace.Height;
                return Math.Max(0, _takenSpace - bounds);
            }
        }

        /// <summary>
        /// Gets or sets the current pixel offset of the items in the direction of scroll.
        /// </summary>
        double IVirtualizingPanel.PixelOffset
        {
            get { return _pixelOffset; }

            set
            {
                if (_pixelOffset != value)
                {
                    _pixelOffset = value;
                    InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Gets the controller for the virtualizing panel.
        /// </summary>
        private IVirtualizingController Controller => ((IVirtualizingPanel)this).Controller;

        /// <summary>
        /// Invalidates the measure of the control and forces a call to 
        /// <see cref="IVirtualizingController.UpdateControls"/> on the next measure.
        /// </summary>
        /// <remarks>
        /// The implementation for this method should call
        /// <see cref="ILayoutable.InvalidateMeasure"/> and also ensure that the next call to
        /// <see cref="ILayoutable.Measure(Size)"/> calls
        /// <see cref="IVirtualizingController.UpdateControls"/> on the next measure even if
        /// the available size hasn't changed.
        /// </remarks>
        void IVirtualizingPanel.ForceInvalidateMeasure()
        {
            InvalidateMeasure();
            _forceRemeasure = true;
        }

        /// <summary>
        /// Measures the control and its child elements as part of a layout pass.
        /// </summary>
        /// <param name="availableSize">The size available to the control.</param>
        /// <returns>The desired size for the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (_forceRemeasure || availableSize != ((ILayoutable)this).PreviousMeasure)
            {
                _forceRemeasure = false;
                _availableSpace = availableSize;
                Controller?.UpdateControls();
            }

            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Positions child elements as part of a layout pass.
        /// </summary>
        /// <param name="finalSize">The size available to the control.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _availableSpace = finalSize;
            _canBeRemoved = 0;
            _takenSpace = 0;
            _averageItemSize = 0;
            _averageCount = 0;
            var result = base.ArrangeOverride(finalSize);
            _takenSpace += _pixelOffset;
            Controller?.UpdateControls();
            return result;
        }

        /// <summary>
        /// Called when the <see cref="Panel.Children"/> collection changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ChildrenChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IControl control in e.NewItems)
                    {
                        UpdateAdd(control);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (IControl control in e.OldItems)
                    {
                        UpdateRemove(control);
                    }

                    break;
            }
        }

        /// <summary>
        /// Gets the next control in the specified direction.
        /// </summary>
        /// <param name="direction">The movement direction.</param>
        /// <param name="from">The control from which movement begins.</param>
        /// <returns>The control.</returns>
        protected override IInputElement GetControlInDirection(NavigationDirection direction, IControl from)
        {
            var logicalScrollable = Parent as ILogicalScrollable;
            var fromControl = from as IControl;

            if (logicalScrollable?.IsLogicalScrollEnabled == true && fromControl != null)
            {
                return logicalScrollable.GetControlInDirection(direction, fromControl);
            }
            else
            {
                return base.GetControlInDirection(direction, from);
            }
        }

        /// <summary>
        /// Arrange the given child in the supplied region.
        /// </summary>
        /// <param name="child">The child to arrange.</param>
        /// <param name="rect">The region in which arrange the control.</param>
        /// <param name="panelSize">The panel size.</param>
        /// <param name="orientation">The orientation </param>
        internal override void ArrangeChild(
            IControl child, 
            Rect rect,
            Size panelSize,
            Orientation orientation)
        {
            if (orientation == Orientation.Vertical)
            {
                rect = new Rect(rect.X, rect.Y - _pixelOffset, rect.Width, rect.Height);
                child.Arrange(rect);

                if (rect.Y >= _availableSpace.Height)
                {
                    ++_canBeRemoved;
                }

                if (rect.Bottom >= _takenSpace)
                {
                    _takenSpace = rect.Bottom;
                }

                AddToAverageItemSize(rect.Height);
            }
            else
            {
                rect = new Rect(rect.X - _pixelOffset, rect.Y, rect.Width, rect.Height);
                child.Arrange(rect);

                if (rect.X >= _availableSpace.Width)
                {
                    ++_canBeRemoved;
                }

                if (rect.Right >= _takenSpace)
                {
                    _takenSpace = rect.Right;
                }

                AddToAverageItemSize(rect.Width);
            }
        }

        /// <summary>
        /// Update the average item size by adding a new control to take into account.
        /// </summary>
        /// <param name="child">The new control to take into account.</param>
        private void UpdateAdd(IControl child)
        {
            var bounds = Bounds;
            var gap = Gap;

            child.Measure(_availableSpace);
            ++_averageCount;

            if (Orientation == Orientation.Vertical)
            {
                var height = child.DesiredSize.Height;
                _takenSpace += height + gap;
                AddToAverageItemSize(height);
            }
            else
            {
                var width = child.DesiredSize.Width;
                _takenSpace += width + gap;
                AddToAverageItemSize(width);
            }
        }

        /// <summary>
        /// Update the average item size by removing a control from those taken into account.
        /// </summary>
        /// <param name="child">The control to stop taking into account.</param>
        private void UpdateRemove(IControl child)
        {
            var bounds = Bounds;
            var gap = Gap;

            if (Orientation == Orientation.Vertical)
            {
                var height = child.DesiredSize.Height;
                _takenSpace -= height + gap;
                RemoveFromAverageItemSize(height);
            }
            else
            {
                var width = child.DesiredSize.Width;
                _takenSpace -= width + gap;
                RemoveFromAverageItemSize(width);
            }

            if (_canBeRemoved > 0)
            {
                --_canBeRemoved;
            }
        }

        /// <summary>
        /// Adds to the average item size.
        /// </summary>
        /// <param name="value">The value to add</param>
        private void AddToAverageItemSize(double value)
        {
            ++_averageCount;
            _averageItemSize += (value - _averageItemSize) / _averageCount;
        }

        /// <summary>
        /// Removes from average item size.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        private void RemoveFromAverageItemSize(double value)
        {
            _averageItemSize = ((_averageItemSize * _averageCount) - value) / (_averageCount - 1);
            --_averageCount;
        }
    }
}
