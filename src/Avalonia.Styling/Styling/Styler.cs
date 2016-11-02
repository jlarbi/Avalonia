// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Styling
{
    /// <summary>
    /// Definition of the <see cref="Styler"/> class.
    /// </summary>
    public class Styler : IStyler
    {
        /// <summary>
        /// Applies the style(s) to the given control.
        /// </summary>
        /// <param name="control">The control to apply the style(s) to.</param>
        public void ApplyStyles(IStyleable control)
        {
            var styleHost = control as IStyleHost;

            if (styleHost != null)
            {
                ApplyStyles(control, styleHost);
            }
        }

        /// <summary>
        /// Applies the given style(s) to the given control.
        /// </summary>
        /// <param name="control">The control to apply the style(s) to.</param>
        /// <param name="styleHost">The style set to apply.</param>
        private void ApplyStyles(IStyleable control, IStyleHost styleHost)
        {
            Contract.Requires<ArgumentNullException>(control != null);
            Contract.Requires<ArgumentNullException>(styleHost != null);

            var parentContainer = styleHost.StylingParent;

            if (parentContainer != null)
            {
                ApplyStyles(control, parentContainer);
            }

            styleHost.Styles.Attach(control, styleHost);
        }
    }
}
