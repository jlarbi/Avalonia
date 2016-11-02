// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Input;
using Avalonia.Metadata;

namespace Avalonia.Controls.Primitives
{
    /// <summary>
    /// Definition of the <see cref="Track"/> class.
    /// </summary>
    public class Track : Control
    {
        /// <summary>
        /// The minimum property.
        /// </summary>
        public static readonly DirectProperty<Track, double> MinimumProperty =
            RangeBase.MinimumProperty.AddOwner<Track>(o => o.Minimum, (o,v) => o.Minimum = v);

        /// <summary>
        /// The maximum property.
        /// </summary>
        public static readonly DirectProperty<Track, double> MaximumProperty =
            RangeBase.MaximumProperty.AddOwner<Track>(o => o.Maximum, (o, v) => o.Maximum = v);

        /// <summary>
        /// The value property.
        /// </summary>
        public static readonly DirectProperty<Track, double> ValueProperty =
            RangeBase.ValueProperty.AddOwner<Track>(o => o.Value, (o, v) => o.Value = v);

        /// <summary>
        /// The viewport size property.
        /// </summary>
        public static readonly StyledProperty<double> ViewportSizeProperty =
            ScrollBar.ViewportSizeProperty.AddOwner<Track>();

        /// <summary>
        /// The orientation property.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            ScrollBar.OrientationProperty.AddOwner<Track>();

        /// <summary>
        /// The thumb property.
        /// </summary>
        public static readonly StyledProperty<Thumb> ThumbProperty =
            AvaloniaProperty.Register<Track, Thumb>("Thumb");

        private double _minimum;
        private double _maximum = 100.0;
        private double _value;

        /// <summary>
        /// Initializes static member(s) of the <see cref="Track"/> class.
        /// </summary>
        static Track()
        {
            ThumbProperty.Changed.AddClassHandler<Track>(x => x.ThumbChanged);
            AffectsArrange(MinimumProperty, MaximumProperty, ValueProperty, OrientationProperty);
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public double Minimum
        {
            get { return _minimum; }
            set { SetAndRaise(MinimumProperty, ref _minimum, value); }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public double Maximum
        {
            get { return _maximum; }
            set { SetAndRaise(MaximumProperty, ref _maximum, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { SetAndRaise(ValueProperty, ref _value, value); }
        }

        /// <summary>
        /// Gets or sets the viewport size.
        /// </summary>
        public double ViewportSize
        {
            get { return GetValue(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thumb.
        /// </summary>
        [Content]
        public Thumb Thumb
        {
            get { return GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        /// <summary>
        /// Custom measurement of the UI element provided by sub classes.
        /// </summary>
        /// <param name="availableSize">The available size that parent can give to the child.</param>
        /// <returns>The requested UI element size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var thumb = Thumb;

            if (thumb != null)
            {
                thumb.Measure(availableSize);

                if (Orientation == Orientation.Horizontal)
                {
                    return new Size(0, thumb.DesiredSize.Height);
                }
                else
                {
                    return new Size(thumb.DesiredSize.Width, 0);
                }
            }

            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Custom arrangement of the UI element provided by sub classes.
        /// </summary>
        /// <param name="finalSize"></param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var thumb = Thumb;

            if (thumb != null)
            {
                var range = Maximum - Minimum;
                var thumbFraction = ViewportSize / range;
                var valueFraction = (Value - Minimum) / range;

                if (double.IsNaN(valueFraction) || double.IsInfinity(valueFraction))
                {
                    valueFraction = 0;
                    thumbFraction = 1;
                }
                else if (double.IsNaN(thumbFraction) || double.IsInfinity(thumbFraction))
                {
                    thumbFraction = 0;
                }

                if (Orientation == Orientation.Horizontal)
                {
                    var width = Math.Max(finalSize.Width * thumbFraction, thumb.MinWidth);
                    var x = (finalSize.Width - width) * valueFraction;
                    thumb.Arrange(new Rect(x, 0, width, finalSize.Height));
                }
                else
                {
                    var height = Math.Max(finalSize.Height * thumbFraction, thumb.MinHeight);
                    var y = (finalSize.Height - height) * valueFraction;
                    thumb.Arrange(new Rect(0, y, finalSize.Width, height));
                }
            }

            return finalSize;
        }

        /// <summary>
        /// Delegate called on thumb changed.
        /// </summary>
        /// <param name="e"></param>
        private void ThumbChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var oldThumb = (Thumb)e.OldValue;
            var newThumb = (Thumb)e.NewValue;

            if (oldThumb != null)
            {
                oldThumb.DragDelta -= ThumbDragged;
            }

            LogicalChildren.Clear();
            VisualChildren.Clear();

            if (newThumb != null)
            {
                newThumb.DragDelta += ThumbDragged;
                LogicalChildren.Add(newThumb);
                VisualChildren.Add(newThumb);
            }
        }

        /// <summary>
        /// Delegate called on thumb dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbDragged(object sender, VectorEventArgs e)
        {
            double range = Maximum - Minimum;
            double value = Value;
            double offset;

            if (Orientation == Orientation.Horizontal)
            {
                offset = e.Vector.X / ((Bounds.Size.Width - Thumb.Bounds.Size.Width) / range);
            }
            else
            {
                offset = e.Vector.Y * (range / (Bounds.Size.Height - Thumb.Bounds.Size.Height));
            }

            if (!double.IsNaN(offset) && !double.IsInfinity(offset))
            {
                value += offset;
                value = Math.Max(value, Minimum);
                value = Math.Min(value, Maximum);
                Value = value;
            }
        }
    }
}
