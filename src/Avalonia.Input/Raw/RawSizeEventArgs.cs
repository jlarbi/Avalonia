// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Input.Raw
{
    /// <summary>
    /// Definition of the <see cref="RawSizeEventArgs"/> class.
    /// </summary>
    public class RawSizeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawSizeEventArgs"/> class.
        /// </summary>
        /// <param name="size"></param>
        public RawSizeEventArgs(Size size)
        {
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawSizeEventArgs"/> class.
        /// </summary>
        /// <param name="width">The size width.</param>
        /// <param name="height">The size height.</param>
        public RawSizeEventArgs(double width, double height)
        {
            Size = new Size(width, height);
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Size Size { get; private set; }
    }
}
