// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.VisualTree;
using Avalonia.Media;

namespace Avalonia.Controls.Primitives
{
    /// <summary>
    /// Definition of the <see cref="AdornerLayer"/> class.
    /// </summary>
    // TODO: Need to track position of adorned elements and move the adorner if they move.
    public class AdornerLayer : Panel
    {
        /// <summary>
        /// The adorned element property.
        /// </summary>
        public static AttachedProperty<Visual> AdornedElementProperty =
            AvaloniaProperty.RegisterAttached<AdornerLayer, Visual, Visual>("AdornedElement");

        /// <summary>
        /// Stores the adorned element info property.
        /// </summary>
        private static readonly AttachedProperty<AdornedElementInfo> s_adornedElementInfoProperty =
            AvaloniaProperty.RegisterAttached<AdornerLayer, Visual, AdornedElementInfo>("AdornedElementInfo");

        private readonly BoundsTracker _tracker = new BoundsTracker();

        /// <summary>
        /// Initializes static member(s) of the <see cref="AdornerLayer"/> class.
        /// </summary>
        static AdornerLayer()
        {
            AdornedElementProperty.Changed.Subscribe(AdornedElementChanged);
            IsHitTestVisibleProperty.OverrideDefaultValue(typeof(AdornerLayer), false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerLayer"/> class.
        /// </summary>
        public AdornerLayer()
        {
            Children.CollectionChanged += ChildrenCollectionChanged;
        }

        /// <summary>
        /// Gets the adorned element of the given visual.
        /// </summary>
        /// <param name="adorner">The adorner visual.</param>
        /// <returns>The adorned element if any.</returns>
        public static Visual GetAdornedElement(Visual adorner)
        {
            return adorner.GetValue(AdornedElementProperty);
        }

        /// <summary>
        /// Sets the adorned element to the given adorner visual.
        /// </summary>
        /// <param name="adorner">The adorner visual.</param>
        /// <param name="adorned">The adorned visual.</param>
        public static void SetAdornedElement(Visual adorner, Visual adorned)
        {
            adorner.SetValue(AdornedElementProperty, adorned);
        }

        /// <summary>
        /// Gets the adorner's layer of the given visual's adorner if any.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static AdornerLayer GetAdornerLayer(IVisual visual)
        {
            return visual.GetVisualAncestors()
                .OfType<AdornerDecorator>()
                .FirstOrDefault()
                ?.AdornerLayer;
        }

        /// <summary>
        /// Custom arrangement of the UI element provided by sub classes.
        /// </summary>
        /// <param name="finalSize"></param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var parent = Parent;

            foreach (var child in Children)
            {
                var info = (AdornedElementInfo)child.GetValue(s_adornedElementInfoProperty);

                if (info != null && info.Bounds.HasValue)
                {
                    child.RenderTransform = new MatrixTransform(info.Bounds.Value.Transform);
                    child.RenderTransformOrigin = new RelativePoint(new Point(0,0), RelativeUnit.Absolute);
                    child.Arrange(info.Bounds.Value.Bounds);
                }
                else
                {
                    child.Arrange(new Rect(child.DesiredSize));
                }
            }

            return finalSize;
        }

        /// <summary>
        /// Delegate called on adorned element changes.
        /// </summary>
        /// <param name="e"></param>
        private static void AdornedElementChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var adorner = (Visual)e.Sender;
            var adorned = (Visual)e.NewValue;
            var layer = adorner.GetVisualParent<AdornerLayer>();
            layer?.UpdateAdornedElement(adorner, adorned);
        }

        /// <summary>
        /// Delegate called on children collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Visual i in e.NewItems)
                    {
                        UpdateAdornedElement(i, i.GetValue(AdornedElementProperty));
                    }

                    break;
            }

            InvalidateArrange();
        }

        /// <summary>
        /// Updates adorned element.
        /// </summary>
        /// <param name="adorner"></param>
        /// <param name="adorned"></param>
        private void UpdateAdornedElement(Visual adorner, Visual adorned)
        {
            var info = adorner.GetValue(s_adornedElementInfoProperty);

            if (info != null)
            {
                info.Subscription.Dispose();

                if (adorned == null)
                {
                    adorner.ClearValue(s_adornedElementInfoProperty);
                }
            }

            if (adorned != null)
            {
                if (info == null)
                {
                    info = new AdornedElementInfo();
                    adorner.SetValue(s_adornedElementInfoProperty, info);
                }

                info.Subscription = _tracker.Track(adorned).Subscribe(x =>
                {
                    info.Bounds = x;
                    InvalidateArrange();
                });
            }
        }

        /// <summary>
        /// Definition of the <see cref="AdornedElementInfo"/> class.
        /// </summary>
        private class AdornedElementInfo
        {
            /// <summary>
            /// TO DO: Comment...
            /// </summary>
            public IDisposable Subscription { get; set; }

            /// <summary>
            /// Gets or sets the adorned element bounds.
            /// </summary>
            public TransformedBounds? Bounds { get; set; }
        }
    }
}
