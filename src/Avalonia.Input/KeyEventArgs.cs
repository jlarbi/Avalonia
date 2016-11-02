// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Interactivity;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="KeyEventArgs"/> class.
    /// </summary>
    public class KeyEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets the keyboard device.
        /// </summary>
        public IKeyboardDevice Device { get; set; }

        /// <summary>
        /// Gets or sets the currently involved key.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets the key modifier(s) if any.
        /// </summary>
        public InputModifiers Modifiers { get; set; }
    }
}
