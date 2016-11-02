// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="NameScopeEventArgs"/> class.
    /// </summary>
    public class NameScopeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameScopeEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="element">The element.</param>
        public NameScopeEventArgs(string name, object element)
        {
            Name = name;
            Element = element;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the involved element.
        /// </summary>
        public object Element { get; }
    }
}
