// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;
using Avalonia.Metadata;

namespace Avalonia.Media
{
    /// <summary>
    /// Definition of the abstract <see cref="GradientBrush"/> class.
    /// </summary>
    public abstract class GradientBrush : Brush
    {
        /// <summary>
        /// Spread method property.
        /// </summary>
        public static readonly StyledProperty<GradientSpreadMethod> SpreadMethodProperty =
            AvaloniaProperty.Register<GradientBrush, GradientSpreadMethod>(nameof(SpreadMethod));

        /// <summary>
        /// Gradient stops property.
        /// </summary>
        public static readonly StyledProperty<List<GradientStop>> GradientStopsProperty =
            AvaloniaProperty.Register<GradientBrush, List<GradientStop>>(nameof(Opacity));

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientBrush"/> class.
        /// </summary>
        public GradientBrush()
        {
            this.GradientStops = new List<GradientStop>();
        }

        /// <summary>
        /// Gets or sets the spread method.
        /// </summary>
        public GradientSpreadMethod SpreadMethod
        {
            get { return GetValue(SpreadMethodProperty); }
            set { SetValue(SpreadMethodProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gradient stops.
        /// </summary>
        [Content]
        public List<GradientStop> GradientStops
        {
            get { return GetValue(GradientStopsProperty); }
            set { SetValue(GradientStopsProperty, value); }
        }
    }
}