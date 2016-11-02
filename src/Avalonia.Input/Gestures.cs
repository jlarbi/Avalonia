// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Interactivity;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="Gestures"/> class.
    /// </summary>
    public static class Gestures
    {
        /// <summary>
        /// The tapped event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> TappedEvent = RoutedEvent.Register<RoutedEventArgs>(
            "Tapped",
            RoutingStrategies.Bubble,
            typeof(Gestures));

        /// <summary>
        /// The double tapped event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> DoubleTappedEvent = RoutedEvent.Register<RoutedEventArgs>(
            "DoubleTapped",
            RoutingStrategies.Bubble,
            typeof(Gestures));

        private static WeakReference s_lastPress;

        /// <summary>
        /// Initializes static member(s) of the <see cref="Gestures"/> class.
        /// </summary>
        static Gestures()
        {
            InputElement.PointerPressedEvent.RouteFinished.Subscribe(PointerPressed);
            InputElement.PointerReleasedEvent.RouteFinished.Subscribe(PointerReleased);
        }

        /// <summary>
        /// Delegate called on pointer pressed.
        /// </summary>
        /// <param name="ev">The event arguments.</param>
        private static void PointerPressed(RoutedEventArgs ev)
        {
            if (ev.Route == RoutingStrategies.Bubble)
            {
                var e = (PointerPressedEventArgs)ev;

                if (e.ClickCount <= 1)
                {
                    s_lastPress = new WeakReference(e.Source);
                }
                else if (s_lastPress?.IsAlive == true && e.ClickCount == 2 && s_lastPress.Target == e.Source)
                {
                    e.Source.RaiseEvent(new RoutedEventArgs(DoubleTappedEvent));
                }
            }
        }

        /// <summary>
        /// Delegate called on pointer released
        /// </summary>
        /// <param name="ev">The event arguments.</param>
        private static void PointerReleased(RoutedEventArgs ev)
        {
            if (ev.Route == RoutingStrategies.Bubble)
            {
                var e = (PointerReleasedEventArgs)ev;

                if (s_lastPress?.IsAlive == true && s_lastPress.Target == e.Source)
                {
                    ((IInteractive)s_lastPress.Target).RaiseEvent(new RoutedEventArgs(TappedEvent));
                }
            }
        }
    }
}
