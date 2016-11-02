// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace Avalonia.Controls.Templates
{
    /// <summary>
    /// Definition of the <see cref="TemplateExtensions"/> class.
    /// </summary>
    public static class TemplateExtensions
    {
        /// <summary>
        /// Gets the template children of the given control.
        /// </summary>
        /// <param name="control">The control template children msut be found for.</param>
        /// <returns>The set of template children.</returns>
        public static IEnumerable<IControl> GetTemplateChildren(this ITemplatedControl control)
        {
            foreach (IControl child in GetTemplateChildren((IControl)control, control))
            {
                yield return child;
            }
        }

        /// <summary>
        /// Gets the template children of the given control.
        /// </summary>
        /// <param name="control">The control template children msut be found for.</param>
        /// <param name="templatedParent">The templated parent.</param>
        /// <returns>The set of template children.</returns>
        private static IEnumerable<IControl> GetTemplateChildren(IControl control, ITemplatedControl templatedParent)
        {
            foreach (IControl child in control.GetVisualChildren())
            {
                if (child.TemplatedParent == templatedParent)
                {
                    yield return child;
                }

                if (child.TemplatedParent != null)
                {
                    foreach (var descendent in GetTemplateChildren(child, templatedParent))
                    {
                        yield return descendent;
                    }
                }
            }
        }
    }
}
