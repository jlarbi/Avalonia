// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Styling;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="UserControl"/> class.
    /// </summary>
    public class UserControl : ContentControl, IStyleable, INameScope
    {
        private readonly NameScope _nameScope = new NameScope();

        /// <summary>
        /// Raised when an element is registered with the name scope.
        /// </summary>
        event EventHandler<NameScopeEventArgs> INameScope.Registered
        {
            add { _nameScope.Registered += value; }
            remove { _nameScope.Registered -= value; }
        }

        /// <summary>
        /// Raised when an element is unregistered with the name scope.
        /// </summary>
        event EventHandler<NameScopeEventArgs> INameScope.Unregistered
        {
            add { _nameScope.Unregistered += value; }
            remove { _nameScope.Unregistered -= value; }
        }

        /// <summary>
        /// Gets the type by which the control is styled.
        /// </summary>
        Type IStyleable.StyleKey => typeof(ContentControl);

        /// <summary>
        /// Registers an element with the name scope.
        /// </summary>
        /// <param name="name">The element name.</param>
        /// <param name="element">The element.</param>
        void INameScope.Register(string name, object element)
        {
            _nameScope.Register(name, element);
        }

        /// <summary>
        /// Finds a named element in the name scope.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The element, or null if the name was not found.</returns>
        object INameScope.Find(string name)
        {
            return _nameScope.Find(name);
        }

        /// <summary>
        /// Unregisters an element with the name scope.
        /// </summary>
        /// <param name="name">The name.</param>
        void INameScope.Unregister(string name)
        {
            _nameScope.Unregister(name);
        }
    }
}
