// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Media;

namespace Avalonia.Controls.Shapes
{
    /// <summary>
    /// Definition of the <see cref="Line"/> class.
    /// </summary>
    public class Line : Shape
    {
        /// <summary>
        /// The start point property.
        /// </summary>
        public static readonly StyledProperty<Point> StartPointProperty =
            AvaloniaProperty.Register<Line, Point>("StartPoint");

        /// <summary>
        /// The end point property.
        /// </summary>
        public static readonly StyledProperty<Point> EndPointProperty =
            AvaloniaProperty.Register<Line, Point>("EndPoint");

        /// <summary>
        /// Initializes static member(s) of the <see cref="Line"/> class.
        /// </summary>
        static Line()
        {
            StrokeThicknessProperty.OverrideDefaultValue<Line>(1);
            AffectsGeometry<Line>(StartPointProperty, EndPointProperty);
        }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        public Point StartPoint
        {
            get { return GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        public Point EndPoint
        {
            get { return GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        /// <summary>
        /// Creates the defining geometry.
        /// </summary>
        /// <returns>The geometry.</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            return new LineGeometry(StartPoint, EndPoint);
        }
    }
}
