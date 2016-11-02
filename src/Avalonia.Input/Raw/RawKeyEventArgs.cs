// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Input.Raw
{
    /// <summary>
    /// Definition of the <see cref="RawKeyEventType"/> enumeration.
    /// </summary>
    public enum RawKeyEventType
    {
        /// <summary>
        /// Key down.
        /// </summary>
        KeyDown,

        /// <summary>
        /// Key up.
        /// </summary>
        KeyUp
    }

    /// <summary>
    /// Definition of the <see cref="RawKeyEventArgs"/> class.
    /// </summary>
    public class RawKeyEventArgs : RawInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawKeyEventArgs"/> class.
        /// </summary>
        /// <param name="device">The keyboard device.</param>
        /// <param name="timestamp">The time stamp.</param>
        /// <param name="type">The key state.</param>
        /// <param name="key">The involved key.</param>
        /// <param name="modifiers">The key modifier(s) if any.</param>
        public RawKeyEventArgs(
            IKeyboardDevice device,
            uint timestamp,
            RawKeyEventType type,
            Key key, InputModifiers modifiers)
            : base(device, timestamp)
        {
            Key = key;
            Type = type;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Gets or sets the keyboard key.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets the key modifier(s) if any.
        /// </summary>
        public InputModifiers Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the key state.
        /// </summary>
        public RawKeyEventType Type { get; set; }
    }
}
