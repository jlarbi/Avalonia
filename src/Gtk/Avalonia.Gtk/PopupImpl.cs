// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Gtk;
using Avalonia.Platform;

namespace Avalonia.Gtk
{
    /// <summary>
    /// Definition of the <see cref="PopupImpl"/> class.
    /// </summary>
    public class PopupImpl : WindowImpl, IPopupImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupImpl"/> class.
        /// </summary>
        public PopupImpl()
            : base(WindowType.Popup)
        {
        }
    }
}
