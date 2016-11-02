// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Avalonia.Controls.Primitives
{
    /// <summary>
    /// Definition of the <see cref="Thumb"/> class.
    /// </summary>
    public class Thumb : TemplatedControl
    {
        /// <summary>
        /// The Drag started event.
        /// </summary>
        public static readonly RoutedEvent<VectorEventArgs> DragStartedEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>("DragStarted", RoutingStrategies.Bubble);

        /// <summary>
        /// The Drag delta event.
        /// </summary>
        public static readonly RoutedEvent<VectorEventArgs> DragDeltaEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>("DragDelta", RoutingStrategies.Bubble);

        /// <summary>
        /// The Drag completed event.
        /// </summary>
        public static readonly RoutedEvent<VectorEventArgs> DragCompletedEvent =
            RoutedEvent.Register<Thumb, VectorEventArgs>("DragCompleted", RoutingStrategies.Bubble);

        private Point? _lastPoint;

        /// <summary>
        /// Initializes static member(s) of the <see cref="Thumb"/> class.
        /// </summary>
        static Thumb()
        {
            DragStartedEvent.AddClassHandler<Thumb>(x => x.OnDragStarted, RoutingStrategies.Bubble);
            DragDeltaEvent.AddClassHandler<Thumb>(x => x.OnDragDelta, RoutingStrategies.Bubble);
            DragCompletedEvent.AddClassHandler<Thumb>(x => x.OnDragCompleted, RoutingStrategies.Bubble);
        }

        /// <summary>
        /// Event fired on drag started.
        /// </summary>
        public event EventHandler<VectorEventArgs> DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }

        /// <summary>
        /// Event fired on drag delta changes.
        /// </summary>
        public event EventHandler<VectorEventArgs> DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }

        /// <summary>
        /// Event fired on drag completed.
        /// </summary>
        public event EventHandler<VectorEventArgs> DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        /// <summary>
        /// Delegate called on drag started.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDragStarted(VectorEventArgs e)
        {
        }

        /// <summary>
        /// Delegate called on drag delta changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDragDelta(VectorEventArgs e)
        {
        }

        /// <summary>
        /// Delegate called on drag completed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnDragCompleted(VectorEventArgs e)
        {
        }

        /// <summary>
        /// Delegate called on pointer moved.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragDeltaEvent,
                    Vector = e.GetPosition(this) - _lastPoint.Value,
                };

                RaiseEvent(ev);
            }
        }

        /// <summary>
        /// Delegate called on pointer pressed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            e.Device.Capture(this);
            _lastPoint = e.GetPosition(this);

            var ev = new VectorEventArgs
            {
                RoutedEvent = DragStartedEvent,
                Vector = (Vector)_lastPoint,
            };

            RaiseEvent(ev);
        }

        /// <summary>
        /// Delegate called on pointer released
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPointerReleased(PointerEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                e.Device.Capture(null);
                _lastPoint = null;

                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragCompletedEvent,
                    Vector = (Vector)e.GetPosition(this),
                };

                RaiseEvent(ev);
            }
        }
    }
}
