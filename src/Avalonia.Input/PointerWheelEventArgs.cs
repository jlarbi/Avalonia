// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="PointerWheelEventArgs"/> class.
    /// </summary>
    public class PointerWheelEventArgs : PointerEventArgs
    {
        /// <summary>
        /// Gets or sets the wheel delta.
        /// </summary>
        public Vector Delta { get; set; }
    }
}
