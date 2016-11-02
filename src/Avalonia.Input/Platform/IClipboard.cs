// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Input.Platform
{
    /// <summary>
    /// Definition of the <see cref="IClipboard"/> interface.
    /// </summary>
    public interface IClipboard
    {
        /// <summary>
        /// Gets the text
        /// </summary>
        /// <returns></returns>
        Task<string> GetTextAsync();

        /// <summary>
        /// Sets the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task SetTextAsync(string text);

        /// <summary>
        /// Clears.
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();
    }
}
