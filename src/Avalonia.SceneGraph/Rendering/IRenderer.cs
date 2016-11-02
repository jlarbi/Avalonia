// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.VisualTree;

namespace Avalonia.Rendering
{
    /// <summary>
    /// Definition of the <see cref="IRenderer"/> interface.
    /// </summary>
    public interface IRenderer : IDisposable
    {
        /// <summary>
        /// Adds a dirty visual.
        /// </summary>
        /// <param name="pVisual"></param>
        void AddDirty(IVisual pVisual);

        /// <summary>
        /// Renders in the given region.
        /// </summary>
        /// <param name="pRegion"></param>
        void Render(Rect pRegion);
    }
}