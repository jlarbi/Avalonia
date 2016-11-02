// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="PointerEventArgs"/> class.
    /// </summary>
    public class PointerEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventArgs"/> class.
        /// </summary>
        public PointerEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        public PointerEventArgs(RoutedEvent routedEvent)
           : base(routedEvent)
        {

        }

        /// <summary>
        /// Gets or sets the keyboard device.
        /// </summary>
        public IPointerDevice Device { get; set; }

        /// <summary>
        /// Gets or sets the input modifier(s) if any.
        /// </summary>
        public InputModifiers InputModifiers { get; set; }

        /// <summary>
        /// Gets the pointer position relative to an element.
        /// </summary>
        /// <param name="relativeTo">The relative element.</param>
        /// <returns>The relative pointer position.</returns>
        public Point GetPosition(IVisual relativeTo)
        {
            return Device.GetPosition(relativeTo);
        }
    }

    /// <summary>
    /// Definition of the <see cref="MouseButton"/> enumeration.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// No button.
        /// </summary>
        None,

        /// <summary>
        /// Left button.
        /// </summary>
        Left,

        /// <summary>
        /// Right button.
        /// </summary>
        Right,

        /// <summary>
        /// Middle button.
        /// </summary>
        Middle
    }

    /// <summary>
    /// Definition of the <see cref="PointerPressedEventArgs"/> class.
    /// </summary>
    public class PointerPressedEventArgs : PointerEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointerPressedEventArgs"/> class.
        /// </summary>
        public PointerPressedEventArgs()
            : base(InputElement.PointerPressedEvent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerPressedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        public PointerPressedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        /// <summary>
        /// Gets or sets the click count.
        /// </summary>
        public int ClickCount { get; set; }

        /// <summary>
        /// Gets or sets the involved mouse button.
        /// </summary>
        public MouseButton MouseButton { get; set; }
    }

    /// <summary>
    /// Definition of the <see cref="PointerReleasedEventArgs"/> class.
    /// </summary>
    public class PointerReleasedEventArgs : PointerEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointerReleasedEventArgs"/> class.
        /// </summary>
        public PointerReleasedEventArgs()
            : base(InputElement.PointerReleasedEvent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerReleasedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        public PointerReleasedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        /// <summary>
        /// Gets or sets the involved mouse button.
        /// </summary>
        public MouseButton MouseButton { get; set; }
    }
}
