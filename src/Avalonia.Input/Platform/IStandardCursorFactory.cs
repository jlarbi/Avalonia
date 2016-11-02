// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="IStandardCursorFactory"/> interface.
    /// </summary>
    public interface IStandardCursorFactory
    {
        /// <summary>
        /// Gets the platform-specific cursor.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        /// <returns>The platform-specific cursor.</returns>
        IPlatformHandle GetCursor(StandardCursorType cursorType);
    }
}
