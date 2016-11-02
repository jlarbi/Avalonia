// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="TextInputEventArgs"/> class.
    /// </summary>
    public class TextInputEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets the keyboard device.
        /// </summary>
        public IKeyboardDevice Device { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}
