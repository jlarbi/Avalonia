// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="IPlatformSettings"/> interface.
    /// </summary>
    public interface IPlatformSettings
    {
        /// <summary>
        /// Double clic area size
        /// </summary>
        Size DoubleClickSize { get; }

        /// <summary>
        /// Double click maximum elapsed time.
        /// </summary>
        TimeSpan DoubleClickTime { get; }
    }
}
