// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Input.Raw
{
    /// <summary>
    /// Definition of the <see cref="RawTextInputEventArgs"/> class.
    /// </summary>
    public class RawTextInputEventArgs : RawInputEventArgs
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawTextInputEventArgs"/> class.
        /// </summary>
        /// <param name="device">The keyboard device.</param>
        /// <param name="timestamp">The time stamp.</param>
        /// <param name="text">The text.</param>
        public RawTextInputEventArgs(IKeyboardDevice device, uint timestamp, string text) : base(device, timestamp)
        {
            Text = text;
        }
    }
}
