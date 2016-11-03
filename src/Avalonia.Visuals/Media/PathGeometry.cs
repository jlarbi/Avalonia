// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Collections;
using Avalonia.Metadata;
using Avalonia.Platform;
using System;

namespace Avalonia.Media
{
    /// <summary>
    /// Definition of the <see cref="PathGeometry"/> class.
    /// </summary>
    public class PathGeometry : StreamGeometry
    {
        /// <summary>
        /// Defines the <see cref="Figures"/> property.
        /// </summary>
        public static readonly DirectProperty<PathGeometry, PathFigures> FiguresProperty =
            AvaloniaProperty.RegisterDirect<PathGeometry, PathFigures>(nameof(Figures), g => g.Figures, (g, f) => g.Figures = f);

        /// <summary>
        /// Defines the <see cref="FillRule"/> property.
        /// </summary>
        public static readonly StyledProperty<FillRule> FillRuleProperty =
                                 AvaloniaProperty.Register<PathGeometry, FillRule>(nameof(FillRule));

        /// <summary>
        /// Initializes static member(s) of the <see cref="PathGeometry"/> class.
        /// </summary>
        static PathGeometry()
        {
            FiguresProperty.Changed.Subscribe(onNext: v =>
            {
                (v.Sender as PathGeometry)?.OnFiguresChanged(v.OldValue as PathFigures, v.NewValue as PathFigures);
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathGeometry"/> class.
        /// </summary>
        public PathGeometry()
        {
            Figures = new PathFigures();
        }

        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        /// <value>
        /// The figures.
        /// </value>
        [Content]
        public PathFigures Figures
        {
            get { return _figures; }
            set { SetAndRaise(FiguresProperty, ref _figures, value); }
        }

        /// <summary>
        /// Gets or sets the fill rule.
        /// </summary>
        /// <value>
        /// The fill rule.
        /// </value>
        public FillRule FillRule
        {
            get { return GetValue(FillRuleProperty); }
            set { SetValue(FillRuleProperty, value); }
        }

        /// <summary>
        /// Gets the platform-specific implementation of the geometry.
        /// </summary>
        public override IGeometryImpl PlatformImpl
        {
            get
            {
                PrepareIfNeeded();
                return base.PlatformImpl;
            }

            protected set
            {
                base.PlatformImpl = value;
            }
        }

        /// <summary>
        /// Clones the geometry.
        /// </summary>
        /// <returns></returns>
        public override Geometry Clone()
        {
            PrepareIfNeeded();

            return base.Clone();
        }

        /// <summary>
        /// Prepares the geometry if needed.
        /// </summary>
        public void PrepareIfNeeded()
        {
            if (_isDirty)
            {
                _isDirty = false;

                using (var ctx = Open())
                {
                    ctx.SetFillRule(FillRule);
                    foreach (var f in Figures)
                    {
                        f.ApplyTo(ctx);
                    }
                }
            }
        }

        /// <summary>
        /// Notifies changes.
        /// </summary>
        internal void NotifyChanged()
        {
            _isDirty = true;
        }

        private PathFigures _figures;
        private IDisposable _figuresObserver = null;
        private IDisposable _figuresPropertiesObserver = null;
        private bool _isDirty = true;

        /// <summary>
        /// Delegate called on figure(s) changed.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnFiguresChanged(PathFigures oldValue, PathFigures newValue)
        {
            _figuresObserver?.Dispose();
            _figuresPropertiesObserver?.Dispose();

            _figuresObserver = newValue?.ForEachItem(f => NotifyChanged(), f => NotifyChanged(), () => NotifyChanged());
            _figuresPropertiesObserver = newValue?.TrackItemPropertyChanged(t => NotifyChanged());
        }
    }
}