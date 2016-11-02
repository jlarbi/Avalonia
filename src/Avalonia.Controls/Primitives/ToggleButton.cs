// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Interactivity;
using Avalonia.Data;

namespace Avalonia.Controls.Primitives
{
    /// <summary>
    /// Definition of the <see cref="ToggleButton"/> class.
    /// </summary>
    public class ToggleButton : Button
    {
        /// <summary>
        /// The Is Checked property.
        /// </summary>
        public static readonly DirectProperty<ToggleButton, bool> IsCheckedProperty =
            AvaloniaProperty.RegisterDirect<ToggleButton, bool>(
                "IsChecked",
                o => o.IsChecked,
                (o,v) => o.IsChecked = v,
                defaultBindingMode: BindingMode.TwoWay);

        private bool _isChecked;

        /// <summary>
        /// Initializes static member(s) of the <see cref="ToggleButton"/> class.
        /// </summary>
        static ToggleButton()
        {
            PseudoClass(IsCheckedProperty, ":checked");
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the toogle button is checked or not.
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetAndRaise(IsCheckedProperty, ref _isChecked, value); }
        }

        /// <summary>
        /// Delegate called on button click
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnClick(RoutedEventArgs e)
        {
            Toggle();
            base.OnClick(e);
        }

        /// <summary>
        /// Toggles the button.
        /// </summary>
        protected virtual void Toggle()
        {
            IsChecked = !IsChecked;
        }
    }
}
