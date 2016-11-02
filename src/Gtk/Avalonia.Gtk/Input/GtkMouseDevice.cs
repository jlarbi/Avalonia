// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Input;
namespace Avalonia.Gtk
{
    /// <summary>
    /// Definition of the <see cref="GtkMouseDevice"/> class.
    /// </summary>
    public class GtkMouseDevice : MouseDevice
    {
        /// <summary>
        /// Stores the <see cref="GtkMouseDevice"/> instance.
        /// </summary>
        private static readonly GtkMouseDevice s_instance;

        /// <summary>
        /// Stores the client position.
        /// </summary>
        private Point _clientPosition;

        /// <summary>
        /// Initializes static member(s) of the <see cref="GtkMouseDevice"/> class.
        /// </summary>
        static GtkMouseDevice()
        {
            s_instance = new GtkMouseDevice();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GtkMouseDevice"/> class.
        /// </summary>
        private GtkMouseDevice()
        {
        }

        /// <summary>
        /// Gets the <see cref="GtkMouseDevice"/> instance.
        /// </summary>
        public static new GtkMouseDevice Instance => s_instance;

        /// <summary>
        /// Sets the client position.
        /// </summary>
        /// <param name="p">The new position.</param>
        internal void SetClientPosition(Point p)
        {
            _clientPosition = p;
        }
    }
}