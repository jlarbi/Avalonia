using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform;

namespace Avalonia
{
    /// <summary>
    /// Definition of the abstract <see cref="AvaloniaDisposable"/> class.
    /// </summary>
    public abstract class AvaloniaDisposable : IDisposable
    {
#if DEBUG_DISPOSE
        public string DisposedAt { get; private set; }
#endif

        /// <summary>
        /// Gets the flag indicating whether it is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
#if DEBUG_DISPOSE
            DisposedAt = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetStackTrace();
#endif
            DoDispose();
        }

        /// <summary>
        /// Checks whether the object is already disposed or not.
        /// </summary>
        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName
#if DEBUG_DISPOSE
                    , "Disposed at: \n" + DisposedAt
#endif

                    );
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        protected abstract void DoDispose();
    }
}
