// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Styling
{
    /// <summary>
    /// Definition of the <see cref="IStyler"/> interface.
    /// </summary>
    public interface IStyler
    {
        /// <summary>
        /// Applies the style(s) to the given control.
        /// </summary>
        /// <param name="control">The control to apply the style(s) to.</param>
        void ApplyStyles(IStyleable control);
    }
}
