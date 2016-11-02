using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace XamlTestApplication
{
    /// <summary>
    /// Definition of the <see cref="TestScrollable"/> class.
    /// </summary>
    public class TestScrollable : Control, ILogicalScrollable
    {
        private int itemCount = 100;
        private Size _extent;
        private Vector _offset;
        private Size _viewport;
        private Size _lineSize;

        /// <summary>
        /// Gets the flag indicating whether the scroll is enabled or not.
        /// </summary>
        public bool IsLogicalScrollEnabled => true;

        /// <summary>
        /// Gets or sets the callback called on scroll invalidation.
        /// </summary>
        public Action InvalidateScroll { get; set; }

        Size IScrollable.Extent
        {
            get { return _extent; }
        }

        Vector IScrollable.Offset
        {
            get { return _offset; }

            set
            {
                _offset = value;
                InvalidateVisual();
            }
        }

        Size IScrollable.Viewport
        {
            get { return _viewport; }
        }

        /// <summary>
        /// Gets the scroll size.
        /// </summary>
        public Size ScrollSize
        {
            get
            {
                return new Size(double.PositiveInfinity, 1);
            }
        }

        /// <summary>
        /// Gets the page scroll size.
        /// </summary>
        public Size PageScrollSize
        {
            get
            {
                return new Size(double.PositiveInfinity, Bounds.Height);
            }
        }

        /// <summary>
        /// Render the scrollable test control.
        /// </summary>
        /// <param name="context"></param>
        public override void Render(DrawingContext context)
        {
            var y = 0.0;

            for (var i = (int)_offset.Y; i < itemCount; ++i)
            {
                using (var line = new FormattedText(
                    "Item " + (i + 1),
                    TextBlock.GetFontFamily(this),
                    TextBlock.GetFontSize(this),
                    TextBlock.GetFontStyle(this),
                    TextAlignment.Left,
                    TextBlock.GetFontWeight(this)))
                {
                    context.DrawText(Brushes.Black, new Point(-_offset.X, y), line);
                    y += _lineSize.Height;
                }
            }
        }

        /// <summary>
        /// Attempts to bring a portion of the target visual into view by scrolling the content.
        /// </summary>
        /// <param name="target">The target visual.</param>
        /// <param name="targetRect">The portion of the target visual to bring into view.</param>
        /// <returns>True if the scroll offset was changed; otherwise false.</returns>
        public bool BringIntoView(IControl target, Rect targetRect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the next control in the specified direction.
        /// </summary>
        /// <param name="direction">The movement direction.</param>
        /// <param name="from">The control from which movement begins.</param>
        /// <returns>The control.</returns>
        public IControl GetControlInDirection(NavigationDirection direction, IControl from)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Measures the control and its child elements as part of a layout pass.
        /// </summary>
        /// <param name="availableSize">The size available to the control.</param>
        /// <returns>The desired size for the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            using (var line = new FormattedText(
                "Item 100",
                TextBlock.GetFontFamily(this),
                TextBlock.GetFontSize(this),
                TextBlock.GetFontStyle(this),
                TextAlignment.Left,
                TextBlock.GetFontWeight(this)))
            {
                line.Constraint = availableSize;
                _lineSize = line.Measure();
                return new Size(_lineSize.Width, _lineSize.Height * itemCount);
            }
        }

        /// <summary>
        /// Positions child elements as part of a layout pass.
        /// </summary>
        /// <param name="finalSize">The size available to the control.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _viewport = new Size(finalSize.Width, finalSize.Height / _lineSize.Height);
            _extent = new Size(_lineSize.Width, itemCount + 1);
            InvalidateScroll?.Invoke();
            return finalSize;
        }
    }
}