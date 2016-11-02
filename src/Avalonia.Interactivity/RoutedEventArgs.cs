// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Interactivity
{
    /// <summary>
    /// Definition of the <see cref="RoutedEventArgs"/> class.
    /// </summary>
    public class RoutedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventArgs"/> class.
        /// </summary>
        public RoutedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The associated routed event.</param>
        public RoutedEventArgs(RoutedEvent routedEvent)
        {
            RoutedEvent = routedEvent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The associated routed event.</param>
        /// <param name="source">The event source.</param>
        public RoutedEventArgs(RoutedEvent routedEvent, IInteractive source)
        {
            RoutedEvent = routedEvent;
            Source = source;
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the event is handled or not.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the associated routed event.
        /// </summary>
        public RoutedEvent RoutedEvent { get; set; }

        /// <summary>
        /// Gets or sets the routed event routing strategy.
        /// </summary>
        public RoutingStrategies Route { get; set; }

        /// <summary>
        /// Gets or sets the routed event source.
        /// </summary>
        public IInteractive Source { get; set; }
    }
}
