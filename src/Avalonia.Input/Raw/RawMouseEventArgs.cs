// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Input.Raw
{
    /// <summary>
    /// Definition of the <see cref="RawMouseEventType"/> enumeration.
    /// </summary>
    public enum RawMouseEventType
    {
        /// <summary>
        /// Leaving window.
        /// </summary>
        LeaveWindow,

        /// <summary>
        /// Left button down.
        /// </summary>
        LeftButtonDown,

        /// <summary>
        /// Left button up.
        /// </summary>
        LeftButtonUp,

        /// <summary>
        /// Right button down.
        /// </summary>
        RightButtonDown,

        /// <summary>
        /// Right button up.
        /// </summary>
        RightButtonUp,

        /// <summary>
        /// Middle button down.
        /// </summary>
        MiddleButtonDown,

        /// <summary>
        /// Middle button up.
        /// </summary>
        MiddleButtonUp,

        /// <summary>
        /// Moving.
        /// </summary>
        Move,

        /// <summary>
        /// Wheeling.
        /// </summary>
        Wheel,

        /// <summary>
        /// Non client left button down.
        /// </summary>
        NonClientLeftButtonDown,
    }

    /// <summary>
    /// A raw mouse event.
    /// </summary>
    public class RawMouseEventArgs : RawInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawMouseEventArgs"/> class.
        /// </summary>
        /// <param name="device">The associated device.</param>
        /// <param name="timestamp">The event timestamp.</param>
        /// <param name="root">The root from which the event originates.</param>
        /// <param name="type">The type of the event.</param>
        /// <param name="position">The mouse position, in client DIPs.</param>
        /// <param name="inputModifiers">The input modifiers.</param>
        public RawMouseEventArgs(
            IInputDevice device,
            uint timestamp,
            IInputRoot root,
            RawMouseEventType type,
            Point position, 
            InputModifiers inputModifiers)
            : base(device, timestamp)
        {
            Contract.Requires<ArgumentNullException>(device != null);
            Contract.Requires<ArgumentNullException>(root != null);

            Root = root;
            Position = position;
            Type = type;
            InputModifiers = inputModifiers;
        }

        /// <summary>
        /// Gets the root from which the event originates.
        /// </summary>
        public IInputRoot Root { get; }

        /// <summary>
        /// Gets the mouse position, in client DIPs.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public RawMouseEventType Type { get; private set; }

        /// <summary>
        /// Gets the input modifiers.
        /// </summary>
        public InputModifiers InputModifiers { get; private set; }
    }
}
