// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Linq;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="LinearGradientBrushImpl"/> class.
    /// </summary>
    public class LinearGradientBrushImpl : BrushImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrushImpl"/> class.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="target"></param>
        /// <param name="destinationSize"></param>
        public LinearGradientBrushImpl(
            Avalonia.Media.LinearGradientBrush brush,
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

            var startPoint = brush.StartPoint.ToPixels(destinationSize);
            var endPoint = brush.EndPoint.ToPixels(destinationSize);

            using (var stops = new SharpDX.Direct2D1.GradientStopCollection(
                target,
                gradientStops,
                brush.SpreadMethod.ToDirect2D()))
            {
                PlatformBrush = new SharpDX.Direct2D1.LinearGradientBrush(
                    target,
                    new SharpDX.Direct2D1.LinearGradientBrushProperties
                    {
                        StartPoint = startPoint.ToSharpDX(),
                        EndPoint = endPoint.ToSharpDX()
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
