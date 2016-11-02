using Avalonia.Animation;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="ExpandDirection"/> enumeration.
    /// </summary>
    public enum ExpandDirection
    {
        /// <summary>
        /// Expands downward
        /// </summary>
        Down,

        /// <summary>
        /// Expands upward
        /// </summary>
        Up,

        /// <summary>
        /// Expands to the left.
        /// </summary>
        Left,

        /// <summary>
        /// Expands to the right.
        /// </summary>
        Right
    }

    /// <summary>
    /// Definition of the <see cref="Expander"/> class.
    /// </summary>
    public class Expander : HeaderedContentControl
    {
        /// <summary>
        /// The content transition property.
        /// </summary>
        public static readonly DirectProperty<Expander, IPageTransition> ContentTransitionProperty =
            AvaloniaProperty.RegisterDirect<Expander, IPageTransition>(
                nameof(ContentTransition),
                o => o.ContentTransition,
                (o, v) => o.ContentTransition = v);

        /// <summary>
        /// The expand direction property.
        /// </summary>
        public static readonly DirectProperty<Expander, ExpandDirection> ExpandDirectionProperty =
            AvaloniaProperty.RegisterDirect<Expander, ExpandDirection>(
                nameof(ExpandDirection),
                o => o.ExpandDirection,
                (o, v) => o.ExpandDirection = v,
                ExpandDirection.Down);

        /// <summary>
        /// The Is expanded property.
        /// </summary>
        public static readonly DirectProperty<Expander, bool> IsExpandedProperty =
            AvaloniaProperty.RegisterDirect<Expander, bool>(
                nameof(IsExpanded),
                o => o.IsExpanded,
                (o, v) => o.IsExpanded = v);

        /// <summary>
        /// Initializes static member(s) of the <see cref="Expander"/> class.
        /// </summary>
        static Expander()
        {
            PseudoClass(ExpandDirectionProperty, d => d == ExpandDirection.Down, ":down");
            PseudoClass(ExpandDirectionProperty, d => d == ExpandDirection.Up, ":up");
            PseudoClass(ExpandDirectionProperty, d => d == ExpandDirection.Left, ":left");
            PseudoClass(ExpandDirectionProperty, d => d == ExpandDirection.Right, ":right");

            PseudoClass(IsExpandedProperty, ":expanded");

            IsExpandedProperty.Changed.AddClassHandler<Expander>(x => x.OnIsExpandedChanged);
        }

        /// <summary>
        /// Gets or sets the content transition type between two pages.
        /// </summary>
        public IPageTransition ContentTransition
        {
            get { return _contentTransition; }
            set { SetAndRaise(ContentTransitionProperty, ref _contentTransition, value); }
        }

        /// <summary>
        /// Gets or sets the expand direction.
        /// </summary>
        public ExpandDirection ExpandDirection
        {
            get { return _expandDirection; }
            set { SetAndRaise(ExpandDirectionProperty, ref _expandDirection, value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the expandable element is expanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetAndRaise(IsExpandedProperty, ref _isExpanded, value); }
        }

        /// <summary>
        /// Delegate called on Is expanded flag changes.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnIsExpandedChanged(AvaloniaPropertyChangedEventArgs e)
        {
            IVisual visualContent = Presenter;

            if (Content != null && ContentTransition != null && visualContent != null)
            {
                bool forward = ExpandDirection == ExpandDirection.Left ||
                                ExpandDirection == ExpandDirection.Up;
                if (IsExpanded)
                {
                    ContentTransition.Start(null, visualContent, forward);
                }
                else
                {
                    ContentTransition.Start(visualContent, null, !forward);
                }
            }
        }

        /// <summary>
        /// Stores the content transition type between two pages.
        /// </summary>
        private IPageTransition _contentTransition;

        /// <summary>
        /// Stores the expand direction.
        /// </summary>
        private ExpandDirection _expandDirection;

        /// <summary>
        /// Stores the flag indicating whether the expandable element is expanded or not.
        /// </summary>
        private bool _isExpanded;
    }
}