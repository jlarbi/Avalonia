// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="InputModifiers"/> enumeration.
    /// </summary>
    [Flags]
    public enum InputModifiers
    {
        /// <summary>
        /// No input modifier.
        /// </summary>
        None = 0,

        /// <summary>
        /// Alt input modifier.
        /// </summary>
        Alt = 1,

        /// <summary>
        /// Ctrl input modifier.
        /// </summary>
        Control = 2,

        /// <summary>
        /// Shift input modifier.
        /// </summary>
        Shift = 4,

        /// <summary>
        /// Windows input modifier.
        /// </summary>
        Windows = 8,

        /// <summary>
        /// Left mouse button input modifier.
        /// </summary>
        LeftMouseButton = 16,

        /// <summary>
        /// Right mouse button input modifier.
        /// </summary>
        RightMouseButton = 32,

        /// <summary>
        /// Middle mouse button input modifier.
        /// </summary>
        MiddleMouseButton = 64
    }

    /// <summary>
    /// Definition of the <see cref="KeyStates"/> enumeration.
    /// </summary>
    [Flags]
    public enum KeyStates
    {
        /// <summary>
        /// No state.
        /// </summary>
        None = 0,

        /// <summary>
        /// Down state.
        /// </summary>
        Down = 1,

        /// <summary>
        /// Toggled state.
        /// </summary>
        Toggled = 2,
    }

    /// <summary>
    /// Definition of the <see cref="IKeyboardDevice"/> interface.
    /// </summary>
    public interface IKeyboardDevice : IInputDevice
    {
        /// <summary>
        /// Gets the currently focused element.
        /// </summary>
        IInputElement FocusedElement { get; }

        /// <summary>
        /// Sets the currently focused element.
        /// </summary>
        /// <param name="element">The newly focused element.</param>
        /// <param name="method">The navigation method.</param>
        /// <param name="modifiers">The input modifier(s) if any.</param>
        void SetFocusedElement(
            IInputElement element, 
            NavigationMethod method,
            InputModifiers modifiers);
    }
}
