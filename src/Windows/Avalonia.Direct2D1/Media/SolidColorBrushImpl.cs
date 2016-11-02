// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Media;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="SolidColorBrushImpl"/> class.
    /// </summary>
    public class SolidColorBrushImpl : BrushImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolidColorBrushImpl"/> class.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="target"></param>
        public SolidColorBrushImpl(ISolidColorBrush brush, SharpDX.Direct2D1.RenderTarget target)
        {
            PlatformBrush = new SharpDX.Direct2D1.SolidColorBrush(
                target,
                brush?.Color.ToDirect2D() ?? new SharpDX.Mathematics.Interop.RawColor4(),
                new SharpDX.Direct2D1.BrushProperties
                {
                    Opacity = brush != null ? (float)brush.Opacity : 1.0f,
                    Transform = target.Transform
                }
            );
        }
    }
}
