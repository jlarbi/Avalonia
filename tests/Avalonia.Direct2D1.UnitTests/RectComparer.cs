// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Avalonia.Direct2D1.UnitTests
{
    /// <summary>
    /// Definition of the <see cref="RectComparer"/> class.
    /// </summary>
    public class RectComparer : IEqualityComparer<Rect>
    {
        /// <summary>
        /// Checks whether two rectangle are equal to each other or not.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True of equal, false otherwise.</returns>
        public bool Equals(Rect a, Rect b)
        {
            return Math.Round(a.X, 3) == Math.Round(b.X, 3) &&
                   Math.Round(a.Y, 3) == Math.Round(b.Y, 3) &&
                   Math.Round(a.Width, 3) == Math.Round(b.Width, 3) &&
                   Math.Round(a.Height, 3) == Math.Round(b.Height, 3);
        }

        /// <summary>
        /// Gets the hash code of the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The hash code.</returns>
        public int GetHashCode(Rect obj)
        {
            throw new NotImplementedException();
        }
    }
}
