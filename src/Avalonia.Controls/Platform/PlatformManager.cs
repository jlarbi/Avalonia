using System;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Platform;

namespace Avalonia.Controls.Platform
{
    /// <summary>
    /// Definition of the <see cref="PlatformManager"/> class.
    /// </summary>
    public static partial class PlatformManager
    {
        static IPlatformSettings GetSettings()
            => AvaloniaLocator.Current.GetService<IPlatformSettings>();

        static bool s_designerMode;

        /// <summary>
        /// Gets the designer mode.
        /// </summary>
        /// <returns></returns>
        public static IDisposable DesignerMode()
        {
            s_designerMode = true;
            return Disposable.Create(() => s_designerMode = false);
        }

        /// <summary>
        /// Sets the designer scaling factor.
        /// </summary>
        /// <param name="factor">The new scaling factor.</param>
        public static void SetDesignerScalingFactor(double factor)
        {
        }

        /// <summary>
        /// Creates a new window.
        /// </summary>
        /// <returns></returns>
        public static IWindowImpl CreateWindow()
        {
            var platform = AvaloniaLocator.Current.GetService<IWindowingPlatform>();
            
            if (platform == null)
            {
                throw new Exception("Could not CreateWindow(): IWindowingPlatform is not registered.");
            }

            return s_designerMode ? platform.CreateEmbeddableWindow() : platform.CreateWindow();
        }

        /// <summary>
        /// Create a new embeddable window.
        /// </summary>
        /// <returns></returns>
        public static IEmbeddableWindowImpl CreateEmbeddableWindow()
        {
            var platform = AvaloniaLocator.Current.GetService<IWindowingPlatform>();
            if (platform == null)
                throw new Exception("Could not CreateEmbeddableWindow(): IWindowingPlatform is not registered.");
            return platform.CreateEmbeddableWindow();
        }

        /// <summary>
        /// Creates a new pop up.
        /// </summary>
        /// <returns></returns>
        public static IPopupImpl CreatePopup()
        {
            return AvaloniaLocator.Current.GetService<IWindowingPlatform>().CreatePopup();
        }
    }
}
