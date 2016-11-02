// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Avalonia.Interactivity
{
    /// <summary>
    /// Definition of the <see cref="RoutingStrategies"/> enumeration
    /// </summary>
    [Flags]
    public enum RoutingStrategies
    {
        /// <summary>
        /// Direct
        /// </summary>
        Direct = 0x01,

        /// <summary>
        /// Tunnel
        /// </summary>
        Tunnel = 0x02,

        /// <summary>
        /// Bubble.
        /// </summary>
        Bubble = 0x04,
    }

    /// <summary>
    /// Definition of the <see cref="RoutedEvent"/> class.
    /// </summary>
    public class RoutedEvent
    {
        private Subject<Tuple<object, RoutedEventArgs>> _raised = new Subject<Tuple<object, RoutedEventArgs>>();
        private Subject<RoutedEventArgs> _routeFinished = new Subject<RoutedEventArgs>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEvent"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="routingStrategies"></param>
        /// <param name="eventArgsType"></param>
        /// <param name="ownerType"></param>
        public RoutedEvent(
            string name,
            RoutingStrategies routingStrategies,
            Type eventArgsType,
            Type ownerType)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(eventArgsType != null);
            Contract.Requires<ArgumentNullException>(ownerType != null);
            Contract.Requires<InvalidCastException>(typeof(RoutedEventArgs).GetTypeInfo().IsAssignableFrom(eventArgsType.GetTypeInfo()));

            EventArgsType = eventArgsType;
            Name = name;
            OwnerType = ownerType;
            RoutingStrategies = routingStrategies;
        }

        /// <summary>
        /// Gets the event argument class type.
        /// </summary>
        public Type EventArgsType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the routed event name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the routed event owner type.
        /// </summary>
        public Type OwnerType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the routing strategy.
        /// </summary>
        public RoutingStrategies RoutingStrategies
        {
            get;
            private set;
        }

        /// <summary>
        /// Raised event.
        /// </summary>
        public IObservable<Tuple<object, RoutedEventArgs>> Raised => _raised;

        /// <summary>
        /// Route finished event.
        /// </summary>
        public IObservable<RoutedEventArgs> RouteFinished => _routeFinished;

        /// <summary>
        /// Registers a RoutedEvent with the given details
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="name"></param>
        /// <param name="routingStrategy"></param>
        /// <returns></returns>
        public static RoutedEvent<TEventArgs> Register<TOwner, TEventArgs>(
            string name,
            RoutingStrategies routingStrategy)
                where TEventArgs : RoutedEventArgs
        {
            Contract.Requires<ArgumentNullException>(name != null);

            return new RoutedEvent<TEventArgs>(name, routingStrategy, typeof(TOwner));
        }

        /// <summary>
        /// Registers a RoutedEvent with the given details
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="name"></param>
        /// <param name="routingStrategy"></param>
        /// <param name="ownerType"></param>
        /// <returns></returns>
        public static RoutedEvent<TEventArgs> Register<TEventArgs>(
            string name,
            RoutingStrategies routingStrategy,
            Type ownerType)
                where TEventArgs : RoutedEventArgs
        {
            Contract.Requires<ArgumentNullException>(name != null);

            return new RoutedEvent<TEventArgs>(name, routingStrategy, ownerType);
        }

        /// <summary>
        /// Registers a new delegate for the given type and routed event.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="handler"></param>
        /// <param name="routes"></param>
        /// <param name="handledEventsToo"></param>
        /// <returns></returns>
        public IDisposable AddClassHandler(
            Type targetType,
            EventHandler<RoutedEventArgs> handler,
            RoutingStrategies routes = RoutingStrategies.Direct | RoutingStrategies.Bubble,
            bool handledEventsToo = false)
        {
            return Raised.Subscribe(args =>
            {
                var sender = args.Item1;
                var e = args.Item2;

                if (targetType.GetTypeInfo().IsAssignableFrom(sender.GetType().GetTypeInfo()) &&
                    ((e.Route == RoutingStrategies.Direct) || (e.Route & routes) != 0) &&
                    (!e.Handled || handledEventsToo))
                {
                    try
                    {
                        handler.DynamicInvoke(sender, e);
                    }
                    catch (TargetInvocationException ex)
                    {
                        // Unwrap the inner exception.
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                }
            });
        }

        /// <summary>
        /// Invokes the raised event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void InvokeRaised(object sender, RoutedEventArgs e)
        {
            _raised.OnNext(Tuple.Create(sender, e));
        }

        /// <summary>
        /// Invokes the route finished event.
        /// </summary>
        /// <param name="e"></param>
        internal void InvokeRouteFinished(RoutedEventArgs e)
        {
            _routeFinished.OnNext(e);
        }
    }

    /// <summary>
    /// Definition of the <see cref="RoutedEvent{TEventArgs}"/> class.
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public class RoutedEvent<TEventArgs> : RoutedEvent
        where TEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEvent{TEventArgs}"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="routingStrategies"></param>
        /// <param name="ownerType"></param>
        public RoutedEvent(string name, RoutingStrategies routingStrategies, Type ownerType)
            : base(name, routingStrategies, typeof(TEventArgs), ownerType)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(ownerType != null);
        }

        /// <summary>
        /// Registers a new delegate for the given type
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="handler"></param>
        /// <param name="routes"></param>
        /// <param name="handledEventsToo"></param>
        /// <returns></returns>
        public IDisposable AddClassHandler<TTarget>(
            Func<TTarget, Action<TEventArgs>> handler,
            RoutingStrategies routes = RoutingStrategies.Direct | RoutingStrategies.Bubble,
            bool handledEventsToo = false)
                where TTarget : class, IInteractive
        {
            EventHandler<RoutedEventArgs> adapter = (sender, e) =>
            {
                var target = sender as TTarget;
                var args = e as TEventArgs;

                if (target != null && args != null)
                {
                    handler(target)(args);
                }
            };

            return AddClassHandler(typeof(TTarget), adapter, routes, handledEventsToo);
        }
    }
}
