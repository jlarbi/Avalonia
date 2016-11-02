// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace Avalonia.Controls.Presenters
{
    /// <summary>
    /// Definition of the <see cref="IItemsPresenter"/> interface.
    /// </summary>
    public interface IItemsPresenter : IPresenter
    {
        /// <summary>
        /// Gets the panel.
        /// </summary>
        IPanel Panel { get; }

        /// <summary>
        /// Scrolls into the view until the given item.
        /// </summary>
        /// <param name="item">The item to reach.</param>
        void ScrollIntoView(object item);
    }
}
