using System; 
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Controls.Utils
{
    /// <summary>
    /// Definition of the <see cref="AncestorFinder"/> class.
    /// </summary>
    public static class AncestorFinder
    {
        /// <summary>
        /// Definition of the <see cref="FinderNode"/> class.
        /// </summary>
        class FinderNode : IDisposable
        {
            private readonly IControl _control;
            private readonly TypeInfo _ancestorType;
            public IObservable<IControl> Observable => _subject;
            private readonly Subject<IControl> _subject = new Subject<IControl>();

            private FinderNode _child;
            private IDisposable _disposable;

            /// <summary>
            /// Initializes a new instance of the <see cref="FinderNode"/> class.
            /// </summary>
            /// <param name="control"></param>
            /// <param name="ancestorType"></param>
            public FinderNode(IControl control, TypeInfo ancestorType)
            {
                _control = control;
                _ancestorType = ancestorType;
            }

            /// <summary>
            /// Initializes the node.
            /// </summary>
            public void Init()
            {
                _disposable = _control.GetObservable(Control.ParentProperty).Subscribe(OnValueChanged);
            }

            /// <summary>
            /// Delegate called on value changes.
            /// </summary>
            /// <param name="next">The next control.</param>
            private void OnValueChanged(IControl next)
            {
                if (next == null || _ancestorType.IsAssignableFrom(next.GetType().GetTypeInfo()))
                    _subject.OnNext(next);
                else
                {
                    _child?.Dispose();
                    _child = new FinderNode(next, _ancestorType);
                    _child.Observable.Subscribe(OnChildValueChanged);
                    _child.Init();
                }
            }

            /// <summary>
            /// Delegate called on child value changes.
            /// </summary>
            /// <param name="control"></param>
            private void OnChildValueChanged(IControl control) => _subject.OnNext(control);

            /// <summary>
            /// Releases resources.
            /// </summary>
            public void Dispose()
            {
                _disposable.Dispose();
            }
        }

        /// <summary>
        /// Creates a new control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ancestorType"></param>
        /// <returns></returns>
        public static IObservable<IControl> Create(IControl control, Type ancestorType)
        {
            return new AnonymousObservable<IControl>(observer =>
            {
                var finder = new FinderNode(control, ancestorType.GetTypeInfo());
                var subscription = finder.Observable.Subscribe(observer);
                finder.Init();

                return Disposable.Create(() =>
                {
                    subscription.Dispose();
                    finder.Dispose();
                });
            });


        }
    }
}
