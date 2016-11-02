using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="IRuntimePlatform"/> interface.
    /// </summary>
    public interface IRuntimePlatform
    {
        /// <summary>
        /// Gets the loaded assemblies.
        /// </summary>
        /// <returns></returns>
        Assembly[] GetLoadedAssemblies();

        /// <summary>
        /// Post a new item into the thread pool.
        /// </summary>
        /// <param name="cb"></param>
        void PostThreadPoolItem(Action cb);

        /// <summary>
        /// Starts the system timer.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        IDisposable StartSystemTimer(TimeSpan interval, Action tick);

        /// <summary>
        /// Gets the stack trace.
        /// </summary>
        /// <returns></returns>
        string GetStackTrace();

        /// <summary>
        /// Gets the runtime platform info.
        /// </summary>
        /// <returns></returns>
        RuntimePlatformInfo GetRuntimeInfo();
    }

    /// <summary>
    /// Definition of the <see cref="RuntimePlatformInfo"/> structure.
    /// </summary>
    public struct RuntimePlatformInfo
    {
        /// <summary>
        /// Gets or sets the operating system type.
        /// </summary>
        public OperatingSystemType OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether on Desktop or not.
        /// </summary>
        public bool IsDesktop { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether on Mobile or not.
        /// </summary>
        public bool IsMobile { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether on Core CLR or not.
        /// </summary>
        public bool IsCoreClr { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether on Mono or not.
        /// </summary>
        public bool IsMono { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether using .Net framework or not.
        /// </summary>
        public bool IsDotNetFramework { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether on Unix or not.
        /// </summary>
        public bool IsUnix { get; set; }
    }

    /// <summary>
    /// Definition of the <see cref="OperatingSystemType"/> enumeration.
    /// </summary>
    public enum OperatingSystemType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// Windows
        /// </summary>
        WinNT,

        /// <summary>
        /// Linux
        /// </summary>
        Linux,

        /// <summary>
        /// Mac OS X
        /// </summary>
        OSX,

        /// <summary>
        /// Android mobile
        /// </summary>
        Android,

        /// <summary>
        /// Apple iOS mobile.
        /// </summary>
        iOS
    }
}
