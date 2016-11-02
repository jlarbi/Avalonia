// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Threading;

namespace Avalonia.Platform
{
    /// <summary>
    /// Provides platform-specific services relating to threading.
    /// </summary>
    public interface IPlatformThreadingInterface
    {
        /// <summary>
        /// Runs the thread loop.
        /// </summary>
        /// <param name="cancellationToken"></param>
        void RunLoop(CancellationToken cancellationToken);

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="tick">The action to call on each tick.</param>
        /// <returns>An <see cref="IDisposable"/> used to stop the timer.</returns>
        IDisposable StartTimer(TimeSpan interval, Action tick);

        /// <summary>
        /// Signals listeners about a job done.
        /// </summary>
        void Signal();

        /// <summary>
        /// Gets the flag indicating whether the thread is looped or not.
        /// </summary>
        bool CurrentThreadIsLoopThread { get; }

        /// <summary>
        /// Event informing about a job done 
        /// </summary>
        event Action Signaled;
    }
}
