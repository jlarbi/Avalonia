// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Layout;

namespace Avalonia.Input.Raw
{
    /// <summary>
    /// Definition of the <see cref="RawMouseWheelEventArgs"/> class.
    /// </summary>
    public class RawMouseWheelEventArgs : RawMouseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawMouseWheelEventArgs"/> class.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="timestamp"></param>
        /// <param name="root"></param>
        /// <param name="position"></param>
        /// <param name="delta"></param>
        /// <param name="inputModifiers"></param>
        public RawMouseWheelEventArgs(
            IInputDevice device,
            uint timestamp,
            IInputRoot root,
            Point position,
            Vector delta, InputModifiers inputModifiers)
            : base(device, timestamp, root, RawMouseEventType.Wheel, position, inputModifiers)
        {
            Delta = delta;
        }

        /// <summary>
        /// Gets the wheel delta.
        /// </summary>
        public Vector Delta { get; private set; }
    }
}
