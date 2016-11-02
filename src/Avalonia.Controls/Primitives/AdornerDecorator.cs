// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Controls.Primitives
{
    /// <summary>
    /// Definition of the <see cref="AdornerDecorator"/> class.
    /// </summary>
    public class AdornerDecorator : Decorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerDecorator"/> class.
        /// </summary>
        public AdornerDecorator()
        {
            AdornerLayer = new AdornerLayer();
            ((ISetLogicalParent)AdornerLayer).SetParent(this);
            AdornerLayer.ZIndex = int.MaxValue;
            VisualChildren.Add(AdornerLayer);
        }

        /// <summary>
        /// Gets the adorner's layer
        /// </summary>
        public AdornerLayer AdornerLayer
        {
            get;
        }

        /// <summary>
        /// Custom measurement of the UI element provided by sub classes.
        /// </summary>
        /// <param name="availableSize">The available size that parent can give to the child.</param>
        /// <returns>The requested UI element size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            AdornerLayer.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Custom arrangement of the UI element provided by sub classes.
        /// </summary>
        /// <param name="finalSize"></param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            AdornerLayer.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }
    }
}
