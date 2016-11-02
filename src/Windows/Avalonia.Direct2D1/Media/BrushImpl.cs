// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="BrushImpl"/> class.
    /// </summary>
    public abstract class BrushImpl : IDisposable
    {
        /// <summary>
        /// Gets or sets the brush.
        /// </summary>
        public SharpDX.Direct2D1.Brush PlatformBrush { get; set; }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (PlatformBrush != null)
            {
                PlatformBrush.Dispose();
            }
        }
    }
}
