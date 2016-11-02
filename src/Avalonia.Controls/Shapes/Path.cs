// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Media;

namespace Avalonia.Controls.Shapes
{
    /// <summary>
    /// Definition of the <see cref="Path"/> class.
    /// </summary>
    public class Path : Shape
    {
        /// <summary>
        /// The data property.
        /// </summary>
        public static readonly StyledProperty<Geometry> DataProperty =
            AvaloniaProperty.Register<Path, Geometry>("Data");

        /// <summary>
        /// Initializes static member(s) of the <see cref="Path"/> class.
        /// </summary>
        static Path()
        {
            AffectsGeometry<Path>(DataProperty);
        }

        /// <summary>
        /// Gets or sets the geometry data.
        /// </summary>
        public Geometry Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Creates the defining geometry.
        /// </summary>
        /// <returns>The geometry.</returns>
        protected override Geometry CreateDefiningGeometry() => Data;
    }
}
