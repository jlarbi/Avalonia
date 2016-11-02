// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="RadioButton"/> class.
    /// </summary>
    public class RadioButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        public RadioButton()
        {
            this.GetObservable(IsCheckedProperty).Subscribe(IsCheckedChanged);
        }

        /// <summary>
        /// Toggles the button.
        /// </summary>
        protected override void Toggle()
        {
            if (!IsChecked)
            {
                IsChecked = true;
            }
        }

        /// <summary>
        /// Delegate called on Is checked changes.
        /// </summary>
        /// <param name="value">The new flag value.</param>
        private void IsCheckedChanged(bool value)
        {
            var parent = this.GetVisualParent();

            if (value && parent != null)
            {
                var siblings = parent
                    .GetVisualChildren()
                    .OfType<RadioButton>()
                    .Where(x => x != this);

                foreach (var sibling in siblings)
                {
                    sibling.IsChecked = false;
                }
            }
        }
    }
}
