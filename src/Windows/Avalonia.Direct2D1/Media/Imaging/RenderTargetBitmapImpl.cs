// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering;
using SharpDX.Direct2D1;
using SharpDX.WIC;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="RenderTargetBitmapImpl"/> class.
    /// </summary>
    public class RenderTargetBitmapImpl : BitmapImpl, IRenderTargetBitmapImpl, IDisposable
    {
        private readonly WicRenderTarget _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBitmapImpl"/> class.
        /// </summary>
        /// <param name="imagingFactory"></param>
        /// <param name="d2dFactory"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RenderTargetBitmapImpl(
            ImagingFactory imagingFactory,
            Factory d2dFactory,
            int width,
            int height)
            : base(imagingFactory, width, height)
        {
            var props = new RenderTargetProperties
            {
                DpiX = 96,
                DpiY = 96,
            };

            _target = new WicRenderTarget(
                d2dFactory,
                WicImpl,
                props);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public override void Dispose()
        {
            _target.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Creates a drawing context.
        /// </summary>
        /// <returns>The drawing context.</returns>
        public Avalonia.Media.DrawingContext CreateDrawingContext() => new RenderTarget(_target).CreateDrawingContext();
        
    }
}
