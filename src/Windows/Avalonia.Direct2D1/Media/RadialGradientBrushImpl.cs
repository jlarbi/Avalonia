// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Linq;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="RadialGradientBrushImpl"/> class.
    /// </summary>
    public class RadialGradientBrushImpl : BrushImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrushImpl"/> class.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="target"></param>
        /// <param name="destinationSize"></param>
        public RadialGradientBrushImpl(
            Avalonia.Media.RadialGradientBrush brush,
            SharpDX.Direct2D1.RenderTarget target,
            Size destinationSize)
        {
            if (brush.GradientStops.Count == 0)
            {
                return;
            }

            var gradientStops = brush.GradientStops.Select(s => new SharpDX.Direct2D1.GradientStop
            {
                Color = s.Color.ToDirect2D(),
                Position = (float)s.Offset
            }).ToArray();

            var centerPoint = brush.Center.ToPixels(destinationSize);
            var GradientOriginOffset = brush.GradientOrigin.ToPixels(destinationSize);
            
            // Note: Direct2D supports RadiusX and RadiusY but Cairo backend supports only Radius property
            var radiusX = brush.Radius;
            var radiusY = brush.Radius;

            using (var stops = new SharpDX.Direct2D1.GradientStopCollection(
                target,
                gradientStops,
                brush.SpreadMethod.ToDirect2D()))
            {
                PlatformBrush = new SharpDX.Direct2D1.RadialGradientBrush(
                    target,
                    new SharpDX.Direct2D1.RadialGradientBrushProperties
                    {
                        Center = centerPoint.ToSharpDX(),
                        GradientOriginOffset = GradientOriginOffset.ToSharpDX(),
                        RadiusX = (float)radiusX,
                        RadiusY = (float)radiusY
                    },
                    new SharpDX.Direct2D1.BrushProperties
                    {
                        Opacity = (float)brush.Opacity,
                        Transform = PrimitiveExtensions.Matrix3x2Identity,
                    },
                    stops);
            }
        }
    }
}
