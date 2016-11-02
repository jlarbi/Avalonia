// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia.LogicalTree
{
    /// <summary>
    /// Definition of the <see cref="LogicalExtensions"/> class.
    /// </summary>
    public static class LogicalExtensions
    {
        /// <summary>
        /// Gets the logical ancestors of the given logical element.
        /// </summary>
        /// <param name="logical">The logical ancestors must be found for.</param>
        /// <returns>The set of logical ancestors.</returns>
        public static IEnumerable<ILogical> GetLogicalAncestors(this ILogical logical)
        {
            Contract.Requires<ArgumentNullException>(logical != null);

            logical = logical.LogicalParent;

            while (logical != null)
            {
                yield return logical;
                logical = logical.LogicalParent;
            }
        }

        /// <summary>
        /// Gets the logical ancestors of the given logical element included.
        /// </summary>
        /// <param name="logical">The logical ancestors must be found for.</param>
        /// <returns>The set of logical ancestors with the calling element included.</returns>
        public static IEnumerable<ILogical> GetSelfAndLogicalAncestors(this ILogical logical)
        {
            yield return logical;

            foreach (var ancestor in logical.GetLogicalAncestors())
            {
                yield return ancestor;
            }
        }

        /// <summary>
        /// Gets the logical children of the given logical element.
        /// </summary>
        /// <param name="logical">The logical children must be found for.</param>
        /// <returns>The set of logical children.</returns>
        public static IEnumerable<ILogical> GetLogicalChildren(this ILogical logical)
        {
            return logical.LogicalChildren;
        }

        /// <summary>
        /// Gets recursively all the logical descendents starting from the given logical element.
        /// </summary>
        /// <param name="logical">The logical element descendents must be found for.</param>
        /// <returns>The set of logical descedents.</returns>
        public static IEnumerable<ILogical> GetLogicalDescendents(this ILogical logical)
        {
            foreach (ILogical child in logical.LogicalChildren)
            {
                yield return child;

                foreach (ILogical descendent in child.GetLogicalDescendents())
                {
                    yield return descendent;
                }
            }
        }

        /// <summary>
        /// Gets the logical parent of the given one.
        /// </summary>
        /// <param name="logical">The logical element the parent must be found for.</param>
        /// <returns>The logical parent.</returns>
        public static ILogical GetLogicalParent(this ILogical logical)
        {
            return logical.LogicalParent;
        }

        /// <summary>
        /// Gets the logical parent as the requested template parameter type.
        /// </summary>
        /// <typeparam name="T">The requested type for the parent.</typeparam>
        /// <param name="logical">The logical element the parent must be found for.</param>
        /// <returns>The logical parent.</returns>
        public static T GetLogicalParent<T>(this ILogical logical) where T : class
        {
            return logical.LogicalParent as T;
        }

        /// <summary>
        /// Gets the logical siblings of the given one.
        /// </summary>
        /// <param name="logical">The logical element siblings must be found for.</param>
        /// <returns>The set of logical siblings.</returns>
        public static IEnumerable<ILogical> GetLogicalSiblings(this ILogical logical)
        {
            ILogical parent = logical.LogicalParent;

            if (parent != null)
            {
                foreach (ILogical sibling in parent.LogicalChildren)
                {
                    yield return sibling;
                }
            }
        }

        /// <summary>
        /// Checks whether the supplied target logical element has this logical element as parent.
        /// </summary>
        /// <param name="logical">The logical element to check as parent of the target one.</param>
        /// <param name="target">The target logical element as potential child.</param>
        /// <returns>True if this logical element is a parent of the given target one, false otherwise.</returns>
        public static bool IsLogicalParentOf(this ILogical logical, ILogical target)
        {
            return target.GetLogicalAncestors().Any(x => x == logical);
        }
    }
}
