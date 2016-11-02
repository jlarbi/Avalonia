// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="ICloseable"/> interface.
    /// </summary>
    public interface ICloseable
    {
        /// <summary>
        /// Event informing about the element closed.
        /// </summary>
        event EventHandler Closed;
    }
}
