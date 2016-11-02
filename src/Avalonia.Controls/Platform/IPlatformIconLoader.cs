using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="IPlatformIconLoader"/> class.
    /// </summary>
    public interface IPlatformIconLoader
    {
        /// <summary>
        /// Loads an icon given its filename.
        /// </summary>
        /// <param name="fileName">The icon's filename.</param>
        /// <returns>The icon.</returns>
        IWindowIconImpl LoadIcon(string fileName);

        /// <summary>
        /// Loads an icon from stream.
        /// </summary>
        /// <param name="stream">The stream which load the icon from.</param>
        /// <returns>The icon.</returns>
        IWindowIconImpl LoadIcon(Stream stream);

        /// <summary>
        /// Loads an icon from a bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap to use to fill the icon.</param>
        /// <returns>The icon.</returns>
        IWindowIconImpl LoadIcon(IBitmapImpl bitmap);
    }
}
