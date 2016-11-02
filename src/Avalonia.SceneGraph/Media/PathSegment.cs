// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Media
{
    /// <summary>
    /// Definition of the abstract <see cref="PathSegment"/> class.
    /// </summary>
    public abstract class PathSegment : AvaloniaObject
    {
        /// <summary>
        /// Applies geometry changes to the given one.
        /// </summary>
        /// <param name="pContext">The geometry context.</param>
        protected internal abstract void ApplyTo(StreamGeometryContext pContext);
    }
}