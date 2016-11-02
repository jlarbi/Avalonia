using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using Avalonia.Input;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="HotKeyManager"/> class.
    /// </summary>
    public class HotKeyManager
    {
        /// <summary>
        /// The hot key property.
        /// </summary>
        public static readonly AttachedProperty<KeyGesture> HotKeyProperty
            = AvaloniaProperty.RegisterAttached<Control, KeyGesture>("HotKey", typeof(HotKeyManager));

        /// <summary>
        /// Definition of the <see cref="HotkeyCommandWrapper"/> class.
        /// </summary>
        class HotkeyCommandWrapper : ICommand
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HotkeyCommandWrapper"/> class.
            /// </summary>
            /// <param name="control"></param>
            public HotkeyCommandWrapper(IControl control)
            {
                Control = control;
            }

            /// <summary>
            /// Gets the control the command is for.
            /// </summary>
            public readonly IControl Control;

            /// <summary>
            /// Gets the command.
            /// </summary>
            private ICommand GetCommand() => Control.GetValue(Button.CommandProperty);

            /// <summary>
            /// Checks whether the command can be executed or not.
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            public bool CanExecute(object parameter) => GetCommand()?.CanExecute(parameter) ?? false;

            /// <summary>
            /// Executes the command.
            /// </summary>
            /// <param name="parameter"></param>
            public void Execute(object parameter) => GetCommand()?.Execute(parameter);

#pragma warning disable 67 // Event not used
            public event EventHandler CanExecuteChanged;
#pragma warning restore 67
        }

        /// <summary>
        /// Definition of the <see cref="Manager"/> class.
        /// </summary>
        class Manager
        {
            private readonly IControl _control;
            private TopLevel _root;
            private IDisposable _parentSub;
            private IDisposable _hotkeySub;
            private KeyGesture _hotkey;
            private readonly HotkeyCommandWrapper _wrapper;
            private KeyBinding _binding;

            /// <summary>
            /// Initializes a new instance of the <see cref="Manager"/> class.
            /// </summary>
            /// <param name="control"></param>
            public Manager(IControl control)
            {
                _control = control;
                _wrapper = new HotkeyCommandWrapper(_control);
            }

            /// <summary>
            /// Initializes the manager.
            /// </summary>
            public void Init()
            {
                _hotkeySub = _control.GetObservable(HotKeyProperty).Subscribe(OnHotkeyChanged);
                _parentSub = AncestorFinder.Create(_control, typeof (TopLevel)).Subscribe(OnParentChanged);
            }

            /// <summary>
            /// Delegate called on parent changes.
            /// </summary>
            /// <param name="control"></param>
            private void OnParentChanged(IControl control)
            {
                Unregister();
                _root = (TopLevel) control;
                Register();
            }

            /// <summary>
            /// Delegate called on hot key changes.
            /// </summary>
            /// <param name="hotkey"></param>
            private void OnHotkeyChanged(KeyGesture hotkey)
            {
                if (hotkey == null)
                    //Subscription will be recreated by static property watcher
                    Stop();
                else
                {
                    Unregister();
                    _hotkey = hotkey;
                    Register();
                }
            }

            /// <summary>
            /// Unregisters key binding
            /// </summary>
            void Unregister()
            {
                if (_root != null && _binding != null)
                    _root.KeyBindings.Remove(_binding);
                _binding = null;
            }

            /// <summary>
            /// Registers key binding
            /// </summary>
            void Register()
            {
                if (_root != null && _hotkey != null)
                {
                    _binding = new KeyBinding() {Gesture = _hotkey, Command = _wrapper};
                    _root.KeyBindings.Add(_binding);
                }
            }

            /// <summary>
            /// TO DO: Comment...
            /// </summary>
            void Stop()
            {
                Unregister();
                _parentSub.Dispose();
                _hotkeySub.Dispose();
            }
        }

        /// <summary>
        /// Initializes static member(s) of the <see cref="HotKeyManager"/> class.
        /// </summary>
        static HotKeyManager()
        {
            HotKeyProperty.Changed.Subscribe(args =>
            {
                var control = args.Sender as IControl;
                if (args.OldValue != null|| control == null)
                    return;
                new Manager(control).Init();
            });
        }

        /// <summary>
        /// Sets a new hot key value for the given target element.
        /// </summary>
        /// <param name="target">The target element to modify.</param>
        /// <param name="value">The new value.</param>
        public static void SetHotKey(AvaloniaObject target, KeyGesture value) => target.SetValue(HotKeyProperty, value);

        /// <summary>
        /// Gets the hot key value from the given target element.
        /// </summary>
        /// <param name="target">The target element to get the value from.</param>
        /// <returns>The hot key value.</returns>
        public static KeyGesture GetHotKey(AvaloniaObject target) => target.GetValue(HotKeyProperty);
    }
}
