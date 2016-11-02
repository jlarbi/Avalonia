using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Avalonia.Utilities
{
    /// <summary>
    /// Definition of the <see cref="WeakTimer"/> class.
    /// </summary>
    public class WeakTimer
    {
        /// <summary>
        /// Definition of the <see cref="IWeakTimerSubscriber"/> interface.
        /// </summary>
        public interface IWeakTimerSubscriber
        {
            /// <summary>
            /// Trigger a tick of the timer.
            /// </summary>
            /// <returns></returns>
            bool Tick();
        }

        private readonly WeakReference<IWeakTimerSubscriber> _subscriber;
        private DispatcherTimer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakTimer"/> class.
        /// </summary>
        /// <param name="subscriber"></param>
        public WeakTimer(IWeakTimerSubscriber subscriber)
        {
            _subscriber = new WeakReference<IWeakTimerSubscriber>(subscriber);
            _timer = new DispatcherTimer();
            
            _timer.Tick += delegate { OnTick(); };
            _timer.Start();
        }

        /// <summary>
        /// Called on ticks
        /// </summary>
        private void OnTick()
        {
            IWeakTimerSubscriber subscriber;
            if (!_subscriber.TryGetTarget(out subscriber) || !subscriber.Tick())
                Stop();
        }

        /// <summary>
        /// Gets or sets the timer ticks interval.
        /// </summary>
        public TimeSpan Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start() => _timer.Start();
        
        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop() => _timer.Stop();

        /// <summary>
        /// Creates a new weak timer and start it straight.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="interval">The ticks interval.</param>
        /// <returns>The new started weak timer.</returns>
        public static WeakTimer StartWeakTimer(IWeakTimerSubscriber subscriber, TimeSpan interval)
        {
            var timer = new WeakTimer(subscriber) {Interval = interval};
            timer.Start();
            return timer;
        }
    }
}
