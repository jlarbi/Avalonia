// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;
using Avalonia.Media;

namespace Avalonia.Controls.Shapes
{
    /// <summary>
    /// Definition of the <see cref="Polyline"/> class.
    /// </summary>
    public class Polyline: Shape
    {
        /// <summary>
        /// The points property.
        /// </summary>
        public static readonly StyledProperty<IList<Point>> PointsProperty =
            AvaloniaProperty.Register<Polyline, IList<Point>>("Points");

        /// <summary>
        /// Initializes static member(s) of the <see cref="Polyline"/> class.
        /// </summary>
        static Polyline()
        {
            StrokeThicknessProperty.OverrideDefaultValue<Polyline>(1);
            AffectsGeometry<Polyline>(PointsProperty);
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
            return new PolylineGeometry(Points, false);
        }
    }
}
