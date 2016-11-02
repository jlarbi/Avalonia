// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Media
{
    /// <summary>
    /// Definition of the <see cref="LineSegment"/> class.
    /// </summary>
    public sealed class LineSegment : PathSegment
    {
        /// <summary>
        /// Defines the <see cref="Point"/> property.
        /// </summary>
        public static readonly StyledProperty<Point> PointProperty
                        = AvaloniaProperty.Register<LineSegment, Point>(nameof(Point));

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public Point Point
        {
            get { return GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        /// <summary>
        /// Applies a new segment to a line geometry.
        /// </summary>
        /// <param name="pContext">The geometry context.</param>
        protected internal override void ApplyTo(StreamGeometryContext pContext)
        {
            pContext.LineTo(Point);
        }
    }
}