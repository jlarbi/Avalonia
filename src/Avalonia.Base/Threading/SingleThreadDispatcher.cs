using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform;

namespace Avalonia.Threading
{
    /// <summary>
    /// Definition of the <see cref="SingleThreadDispatcher"/> class.
    /// </summary>
    public class SingleThreadDispatcher : Dispatcher
    {
        /// <summary>
        /// Definition of the <see cref="ThreadingInterface"/> class.
        /// </summary>
        class ThreadingInterface : IPlatformThreadingInterface
        {
            private readonly AutoResetEvent _evnt = new AutoResetEvent(false);
            private readonly JobRunner _timerJobRunner;

            /// <summary>
            /// Initializes of the <see cref="ThreadingInterface"/> class.
            /// </summary>
            public ThreadingInterface()
            {
                _timerJobRunner = new JobRunner(this);
            }

            /// <summary>
            /// Runs the thread loop.
            /// </summary>
            /// <param name="cancellationToken"></param>
            public void RunLoop(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _evnt.WaitOne();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Signaled?.Invoke();
                    _timerJobRunner.RunJobs();
                }
            }

            /// <summary>
            /// Starts the timer.
            /// </summary>
            /// <param name="interval">The interval.</param>
            /// <param name="tick">The action to call on each tick.</param>
            /// <returns>An <see cref="IDisposable"/> used to stop the timer.</returns>
            public IDisposable StartTimer(TimeSpan interval, Action tick)
                => AvaloniaLocator.Current.GetService<IRuntimePlatform>().StartSystemTimer(interval,
                    () => _timerJobRunner.Post(tick, DispatcherPriority.Normal));

            /// <summary>
            /// Signals listeners about a job done.
            /// </summary>
            public void Signal() => _evnt.Set();

            /// <summary>
            /// Gets the flag indicating whether the thread is looped or not.
            /// </summary>
            // TODO: Actually perform a check
            public bool CurrentThreadIsLoopThread => true;

            /// <summary>
            /// Event informing about a job done 
            /// </summary>
            public event Action Signaled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleThreadDispatcher"/> class.
        /// </summary>
        public SingleThreadDispatcher() : base(new ThreadingInterface())
        {
        }

        /// <summary>
        /// Creates a new dispatcher and its associated thread
        /// </summary>
        /// <param name="token"></param>
        /// <returns>The new dispatcher.</returns>
        public static Dispatcher StartNew(CancellationToken token)
        {
            var dispatcher = new SingleThreadDispatcher();
            AvaloniaLocator.Current.GetService<IRuntimePlatform>().PostThreadPoolItem(() =>
            {
                dispatcher.MainLoop(token);
            });
            return dispatcher;
        }
    }
}
