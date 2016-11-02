using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="IWindowingPlatform"/> class.
    /// </summary>
    public interface IWindowingPlatform
    {
        /// <summary>
        /// Creates a window.
        /// </summary>
        /// <returns></returns>
        IWindowImpl CreateWindow();

        /// <summary>
        /// Create an embeddable window.
        /// </summary>
        /// <returns></returns>
        IEmbeddableWindowImpl CreateEmbeddableWindow();

        /// <summary>
        /// Create a pop up window.
        /// </summary>
        /// <returns></returns>
        IPopupImpl CreatePopup();
    }
}
