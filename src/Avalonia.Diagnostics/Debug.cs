// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.VisualTree;

namespace Avalonia.Diagnostics
{
    /// <summary>
    /// Definition of the <see cref="Debug"/> class.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Prints the visual tree content as string.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static string PrintVisualTree(IVisual visual)
        {
            StringBuilder result = new StringBuilder();
            PrintVisualTree(visual, result, 0);
            return result.ToString();
        }

        private static void PrintVisualTree(IVisual visual, StringBuilder builder, int indent)
        {
            Control control = visual as Control;

            builder.Append(Indent(indent - 1));

            if (indent > 0)
            {
                builder.Append(" +- ");
            }

            builder.Append(visual.GetType().Name);

            if (control != null)
            {
                builder.Append(" ");
                builder.AppendLine(control.Classes.ToString());

                foreach (var property in AvaloniaPropertyRegistry.Instance.GetRegistered(control))
                {
                    var value = control.GetDiagnostic(property);

                    if (value.Priority != BindingPriority.Unset)
                    {
                        builder.Append(Indent(indent));
                        builder.Append(" |  ");
                        builder.Append(value.Property.Name);
                        builder.Append(" = ");
                        builder.Append(value.Value ?? "(null)");
                        builder.Append(" [");
                        builder.Append(value.Priority);
                        builder.AppendLine("]");
                    }
                }
            }
            else
            {
                builder.AppendLine();
            }

            foreach (var child in visual.VisualChildren)
            {
                PrintVisualTree(child, builder, indent + 1);
            }
        }

        private static string Indent(int indent)
        {
            return string.Join(string.Empty, Enumerable.Repeat("    ", Math.Max(indent, 0)));
        }
    }
}
