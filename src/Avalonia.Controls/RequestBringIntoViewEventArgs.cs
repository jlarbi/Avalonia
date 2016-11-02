// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="RequestBringIntoViewEventArgs"/> class.
    /// </summary>
    public class RequestBringIntoViewEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets the visual target object.
        /// </summary>
        public IVisual TargetObject { get; set; }

        /// <summary>
        /// Gets or sets the target region.
        /// </summary>
        public Rect TargetRect { get; set; }
    }
}
