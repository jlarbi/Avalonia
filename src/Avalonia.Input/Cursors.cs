// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform;

namespace Avalonia.Input
{
    /*
    =========================================================================================
        NOTE: Cursors are NOT disposable and are cached in platform implementation.
        To support loading custom cursors some measures about that should be taken beforehand
    =========================================================================================
    */

    /// <summary>
    /// Definition of the <see cref="StandardCursorType"/> enumeration
    /// </summary>
    public enum StandardCursorType
    {
        /// <summary>
        /// Arrow.
        /// </summary>
        Arrow,

        /// <summary>
        /// IBeam.
        /// </summary>
        Ibeam,

        /// <summary>
        /// Wait
        /// </summary>
        Wait,

        /// <summary>
        /// Cross.
        /// </summary>
        Cross,

        /// <summary>
        /// Up arrow.
        /// </summary>
        UpArrow,

        /// <summary>
        /// Size west east.
        /// </summary>
        SizeWestEast,

        /// <summary>
        /// Size north south.
        /// </summary>
        SizeNorthSouth,

        /// <summary>
        /// Size all.
        /// </summary>
        SizeAll,

        /// <summary>
        /// No.
        /// </summary>
        No,

        /// <summary>
        /// Hand.
        /// </summary>
        Hand,

        /// <summary>
        /// Application starting.
        /// </summary>
        AppStarting,

        /// <summary>
        /// Help
        /// </summary>
        Help,

        /// <summary>
        /// Top side.
        /// </summary>
        TopSide,

        /// <summary>
        /// Bottom size.
        /// </summary>
        BottomSize,

        /// <summary>
        /// Left side.
        /// </summary>
        LeftSide,

        /// <summary>
        /// Right side.
        /// </summary>
        RightSide,

        /// <summary>
        /// Top left corner.
        /// </summary>
        TopLeftCorner,

        /// <summary>
        /// Top right corner.
        /// </summary>
        TopRightCorner,

        /// <summary>
        /// Bottom left corner.
        /// </summary>
        BottomLeftCorner,

        /// <summary>
        /// Bottom right corner.
        /// </summary>
        BottomRightCorner

        // Not available in GTK directly, see http://www.pixelbeat.org/programming/x_cursors/ 
        // We might enable them later, preferably, by loading pixmax direclty from theme with fallback image
        // SizeNorthWestSouthEast,
        // SizeNorthEastSouthWest,
    }

    /// <summary>
    /// Definition of the <see cref="Cursor"/> class.
    /// </summary>
    public class Cursor
    {
        /// <summary>
        /// The default cursor 
        /// </summary>
        public static Cursor Default = new Cursor(StandardCursorType.Arrow);

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="platformCursor">The platform-specific cursor</param>
        internal Cursor(IPlatformHandle platformCursor)
        {
            PlatformCursor = platformCursor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public Cursor(StandardCursorType cursorType)
            : this(GetCursor(cursorType))
        {
        }

        /// <summary>
        /// Gets the platform-specific cursor.
        /// </summary>
        public IPlatformHandle PlatformCursor { get; }

        /// <summary>
        /// Creates the platform-specific cursor given its type.
        /// </summary>
        /// <param name="type">The cursor type.</param>
        /// <returns>The new platform-specific cursor.</returns>
        private static IPlatformHandle GetCursor(StandardCursorType type)
        {
            var platform = AvaloniaLocator.Current.GetService<IStandardCursorFactory>();

            if (platform == null)
            {
                throw new Exception("Could not create Cursor: IStandardCursorFactory not registered.");
            }

            return platform.GetCursor(type);
        }
    }
}
