// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Media;

namespace Avalonia.Controls.Shapes
{
    /// <summary>
    /// Definition of the <see cref="Ellipse"/> class.
    /// </summary>
    public class Ellipse : Shape
    {
        /// <summary>
        /// Initializes static member(s) of the <see cref="Ellipse"/> class.
        /// </summary>
        static Ellipse()
        {
            AffectsGeometry<Ellipse>(BoundsProperty, StrokeThicknessProperty);
        }

        /// <summary>
        /// Creates the defining geometry.
        /// </summary>
        /// <returns>The geometry.</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            var rect = new Rect(Bounds.Size).Deflate(StrokeThickness);
            return new EllipseGeometry(rect);
        }

        /// <summary>
        /// Measures the control and its child elements as part of a layout pass.
        /// </summary>
        /// <param name="availableSize">The size available to the control.</param>
        /// <returns>The desired size for the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(StrokeThickness, StrokeThickness);
        }
    }
}
