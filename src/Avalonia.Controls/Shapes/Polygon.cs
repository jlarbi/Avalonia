// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;
using Avalonia.Media;

namespace Avalonia.Controls.Shapes
{
    /// <summary>
    /// Definition of the <see cref="Polygon"/> class.
    /// </summary>
    public class Polygon : Shape
    {
        /// <summary>
        /// The points property.
        /// </summary>
        public static readonly StyledProperty<IList<Point>> PointsProperty =
            AvaloniaProperty.Register<Polygon, IList<Point>>("Points");

        /// <summary>
        /// Initializes static member(s) of the <see cref="Polygon"/> class.
        /// </summary>
        static Polygon()
        {
            AffectsGeometry<Polygon>(PointsProperty);
        }

        /// <summary>
        /// Gets or sets the set of points.
        /// </summary>
        public IList<Point> Points
        {
            get { return GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        /// <summary>
        /// Creates the defining geometry.
        /// </summary>
        /// <returns>The geometry.</returns>
        protected override Geometry CreateDefiningGeometry()
        {
            return new PolylineGeometry(Points, true);
        }
    }
}
