// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Input;

namespace Avalonia.Controls.Primitives
{
    /// <summary>
    /// Definition of the <see cref="TabStrip"/> class.
    /// </summary>
    public class TabStrip : SelectingItemsControl
    {
        /// <summary>
        /// The default panel.
        /// </summary>
        private static readonly FuncTemplate<IPanel> DefaultPanel =
            new FuncTemplate<IPanel>(() => new WrapPanel { Orientation = Orientation.Horizontal });

        private static IMemberSelector s_MemberSelector = new FuncMemberSelector<object, object>(SelectHeader);

        /// <summary>
        /// Initializes static member(s) of the <see cref="TabStrip"/> class.
        /// </summary>
        static TabStrip()
        {
            MemberSelectorProperty.OverrideDefaultValue<TabStrip>(s_MemberSelector);
            SelectionModeProperty.OverrideDefaultValue<TabStrip>(SelectionMode.AlwaysSelected);
            FocusableProperty.OverrideDefaultValue(typeof(TabStrip), false);
            ItemsPanelProperty.OverrideDefaultValue<TabStrip>(DefaultPanel);
        }

        /// <summary>
        /// Creates the <see cref="ItemContainerGenerator"/> for the control.
        /// </summary>
        /// <returns>
        /// An <see cref="IItemContainerGenerator"/> or null.
        /// </returns>
        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<TabStripItem>(
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);
        }

        /// <summary>
        /// Called before the <see cref="InputElement.GotFocus"/> event occurs.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

            if (e.NavigationMethod == NavigationMethod.Directional)
            {
                e.Handled = UpdateSelectionFromEventSource(e.Source);
            }
        }

        /// <summary>
        /// Called before the <see cref="InputElement.PointerPressed"/> event occurs.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.MouseButton == MouseButton.Left)
            {
                e.Handled = UpdateSelectionFromEventSource(e.Source);
            }
        }

        /// <summary>
        /// Selects the header if any.
        /// </summary>
        /// <param name="o">The object to extract the header from.</param>
        /// <returns>The header if any.</returns>
        private static object SelectHeader(object o)
        {
            var headered = o as IHeadered;
            return (headered != null) ? (headered.Header ?? string.Empty) : o;
        }
    }
}
