// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="IPointerDevice"/> interface.
    /// </summary>
    public interface IPointerDevice : IInputDevice
    {
        /// <summary>
        /// Gets the currently captured element.
        /// </summary>
        IInputElement Captured { get; }

        /// <summary>
        /// Captures a new element.
        /// </summary>
        /// <param name="control"></param>
        void Capture(IInputElement control);

        /// <summary>
        /// Gets the position relative to the given element.
        /// </summary>
        /// <param name="relativeTo">The relative element.</param>
        /// <returns>The pointer position.</returns>
        Point GetPosition(IVisual relativeTo);
    }
}
