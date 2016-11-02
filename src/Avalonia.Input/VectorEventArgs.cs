// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Interactivity;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="VectorEventArgs"/> class.
    /// </summary>
    public class VectorEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets the vector.
        /// </summary>
        public Vector Vector { get; set; }
    }
}
