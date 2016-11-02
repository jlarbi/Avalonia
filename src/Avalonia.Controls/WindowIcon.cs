using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Avalonia.Controls
{
    /// <summary>
    /// Represents an icon for a window.
    /// </summary>
    public class WindowIcon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowIcon"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap to use to fill the icon.</param>
        public WindowIcon(IBitmap bitmap)
        {
            PlatformImpl = AvaloniaLocator.Current.GetService<IPlatformIconLoader>().LoadIcon(bitmap.PlatformImpl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowIcon"/> class.
        /// </summary>
        /// <param name="fileName">The icon filename.</param>
        public WindowIcon(string fileName)
        {
            PlatformImpl = AvaloniaLocator.Current.GetService<IPlatformIconLoader>().LoadIcon(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowIcon"/> class.
        /// </summary>
        /// <param name="stream">The stream to get the icon from.</param>
        public WindowIcon(Stream stream)
        {
            PlatformImpl = AvaloniaLocator.Current.GetService<IPlatformIconLoader>().LoadIcon(stream);
        }

        /// <summary>
        /// Gets the platform-specific icon.
        /// </summary>
        public IWindowIconImpl PlatformImpl { get; }
    }
}
