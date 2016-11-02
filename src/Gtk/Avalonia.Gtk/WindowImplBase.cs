// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;
using Gdk;
using Avalonia.Controls;
using Avalonia.Input.Raw;
using Avalonia.Platform;
using Avalonia.Input;
using Avalonia.Threading;
using Action = System.Action;
using WindowEdge = Avalonia.Controls.WindowEdge;

namespace Avalonia.Gtk
{
    using Gtk = global::Gtk;

    /// <summary>
    /// Definition of the <see cref="WindowImplBase"/> class.
    /// </summary>
    public abstract class WindowImplBase : IWindowImpl
    {
        private IInputRoot _inputRoot;

        /// <summary>
        /// Stores the window.
        /// </summary>
        protected Gtk.Widget _window;

        /// <summary>
        /// Gets the window.
        /// </summary>
        public Gtk.Widget Widget => _window;
        
        private Gtk.IMContext _imContext;

        private uint _lastKeyEventTimestamp;

        private static readonly Gdk.Cursor DefaultCursor = new Gdk.Cursor(CursorType.LeftPtr);

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowImplBase"/> class.
        /// </summary>
        /// <param name="window"></param>
        protected WindowImplBase(Gtk.Widget window)
        {
            _window = window;
            Init();
        }

        void Init()
        {
            Handle = _window as IPlatformHandle;
            _window.Events = EventMask.AllEventsMask;
            _imContext = new Gtk.IMMulticontext();
            _imContext.Commit += ImContext_Commit;
            _window.Realized += OnRealized;
            _window.DoubleBuffered = false;
            _window.Realize();
            _window.ButtonPressEvent += OnButtonPressEvent;
            _window.ButtonReleaseEvent += OnButtonReleaseEvent;
            _window.ScrollEvent += OnScrollEvent;
            _window.Destroyed += OnDestroyed;
            _window.KeyPressEvent += OnKeyPressEvent;
            _window.KeyReleaseEvent += OnKeyReleaseEvent;
            _window.ExposeEvent += OnExposeEvent;
            _window.MotionNotifyEvent += OnMotionNotifyEvent;
            
        }

        /// <summary>
        /// Gets the platform-specific handle.
        /// </summary>
        public IPlatformHandle Handle { get; private set; }

        /// <summary>
        /// Delegate called on window realized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void OnRealized (object sender, EventArgs eventArgs)
        {
            _imContext.ClientWindow = _window.GdkWindow;
        }

        /// <summary>
        /// Gets or sets the window client size.
        /// </summary>
        public abstract Size ClientSize { get; set; }

        /// <summary>
        /// Gets the maximum client size.
        /// </summary>
        public Size MaxClientSize
        {
            get
            {
                // TODO: This should take into account things such as taskbar and window border
                // thickness etc.
                return new Size(_window.Screen.Width, _window.Screen.Height);
            }
        }

        /// <summary>
        /// Gets or sets the window state.
        /// </summary>
        public Avalonia.Controls.WindowState WindowState
        {
            get
            {
                switch (_window.GdkWindow.State)
                {
                    case Gdk.WindowState.Iconified:
                        return Controls.WindowState.Minimized;
                    case Gdk.WindowState.Maximized:
                        return Controls.WindowState.Maximized;
                    default:
                        return Controls.WindowState.Normal;
                }
            }

            set
            {
                switch (value)
                {
                    case Controls.WindowState.Minimized:
                        _window.GdkWindow.Iconify();
                        break;
                    case Controls.WindowState.Maximized:
                        _window.GdkWindow.Maximize();
                        break;
                    case Controls.WindowState.Normal:
                        _window.GdkWindow.Deiconify();
                        _window.GdkWindow.Unmaximize();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the window scaling.
        /// </summary>
        public double Scaling => 1;

        /// <summary>
        /// Gets or sets the callback to call on window activated.
        /// </summary>
        public Action Activated { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window closed.
        /// </summary>
        public Action Closed { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window deactivated.
        /// </summary>
        public Action Deactivated { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window inputs.
        /// </summary>
        public Action<RawInputEventArgs> Input { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window paint.
        /// </summary>
        public Action<Rect> Paint { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window resized.
        /// </summary>
        public Action<Size> Resized { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window position changed.
        /// </summary>
        public Action<Point> PositionChanged { get; set; }

        /// <summary>
        /// Gets or sets the callback to call on window scaling changed.
        /// </summary>
        public Action<double> ScalingChanged { get; set; }

        /// <summary>
        /// Creates a new pop up.
        /// </summary>
        /// <returns></returns>
        public IPopupImpl CreatePopup()
        {
            return new PopupImpl();
        }

        /// <summary>
        /// Invalidates the given window region.
        /// </summary>
        /// <param name="rect"></param>
        public void Invalidate(Rect rect)
        {
            if (_window?.GdkWindow != null)
                _window.GdkWindow.InvalidateRect(
                    new Rectangle((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height), true);
        }

        /// <summary>
        /// Turns a point from screen space to client space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point PointToClient(Point point)
        {
            int x, y;
            _window.GdkWindow.GetDeskrelativeOrigin(out x, out y);

            return new Point(point.X - x, point.Y - y);
        }

        /// <summary>
        /// Turns a point from client space to screen space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point PointToScreen(Point point)
        {
            int x, y;
            _window.GdkWindow.GetDeskrelativeOrigin(out x, out y);
            return new Point(point.X + x, point.Y + y);
        }

        /// <summary>
        /// Sets the input root element.
        /// </summary>
        /// <param name="inputRoot"></param>
        public void SetInputRoot(IInputRoot inputRoot)
        {
            _inputRoot = inputRoot;
        }

        /// <summary>
        /// Sets teh window title.
        /// </summary>
        /// <param name="title"></param>
        public abstract void SetTitle(string title);

        /// <summary>
        /// Show a dialog pop up.
        /// </summary>
        /// <returns></returns>
        public abstract IDisposable ShowDialog();

        /// <summary>
        /// Set system decorations.
        /// </summary>
        /// <param name="enabled"></param>
        public abstract void SetSystemDecorations(bool enabled);

        /// <summary>
        /// Set the window icon.
        /// </summary>
        /// <param name="icon"></param>
        public abstract void SetIcon(IWindowIconImpl icon);

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="cursor"></param>
        public void SetCursor(IPlatformHandle cursor)
        {
            _window.GdkWindow.Cursor = cursor != null ? new Gdk.Cursor(cursor.Handle) : DefaultCursor;
        }

        /// <summary>
        /// Shows the windo up to the screen.
        /// </summary>
        public void Show() => _window.Show();

        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide() => _window.Hide();

        /// <summary>
        /// Starts a drag move.
        /// </summary>
        public abstract void BeginMoveDrag();

        /// <summary>
        /// Starts a drag to resize move.
        /// </summary>
        /// <param name="edge"></param>
        public abstract void BeginResizeDrag(WindowEdge edge);

        /// <summary>
        /// Gets or sets the window position.
        /// </summary>
        public abstract Point Position { get; set; }

        void ITopLevelImpl.Activate()
        {
            _window.Activate();
        }

        private static InputModifiers GetModifierKeys(ModifierType state)
        {
            var rv = InputModifiers.None;
            if (state.HasFlag(ModifierType.ControlMask))
                rv |= InputModifiers.Control;
            if (state.HasFlag(ModifierType.ShiftMask))
                rv |= InputModifiers.Shift;
            if (state.HasFlag(ModifierType.Mod1Mask))
                rv |= InputModifiers.Control;
            if(state.HasFlag(ModifierType.Button1Mask))
                rv |= InputModifiers.LeftMouseButton;
            if (state.HasFlag(ModifierType.Button2Mask))
                rv |= InputModifiers.RightMouseButton;
            if (state.HasFlag(ModifierType.Button3Mask))
                rv |= InputModifiers.MiddleMouseButton;
            return rv;
        }

        void OnButtonPressEvent(object o, Gtk.ButtonPressEventArgs args)
        {
            var evnt = args.Event;
            var e = new RawMouseEventArgs(
                GtkMouseDevice.Instance,
                evnt.Time,
                _inputRoot,
                evnt.Button == 1
                    ? RawMouseEventType.LeftButtonDown
                    : evnt.Button == 3 ? RawMouseEventType.RightButtonDown : RawMouseEventType.MiddleButtonDown,
                new Point(evnt.X, evnt.Y), GetModifierKeys(evnt.State));
            Input(e);
        }

        void OnScrollEvent(object o, Gtk.ScrollEventArgs args)
        {
            var evnt = args.Event;
            double step = 1;
            var delta = new Vector();
            if (evnt.Direction == ScrollDirection.Down)
                delta = new Vector(0, -step);
            else if (evnt.Direction == ScrollDirection.Up)
                delta = new Vector(0, step);
            else if (evnt.Direction == ScrollDirection.Right)
                delta = new Vector(-step, 0);
            if (evnt.Direction == ScrollDirection.Left)
                delta = new Vector(step, 0);
            var e = new RawMouseWheelEventArgs(GtkMouseDevice.Instance, evnt.Time, _inputRoot, new Point(evnt.X, evnt.Y), delta, GetModifierKeys(evnt.State));
            Input(e);
        }

        /// <summary>
        /// Delegate called on button release event.
        /// </summary>
        /// <param name="o">The button</param>
        /// <param name="args">The event arguments.</param>
        protected void OnButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
        {
            var evnt = args.Event;
            var e = new RawMouseEventArgs(
                GtkMouseDevice.Instance,
                evnt.Time,
                _inputRoot,
                evnt.Button == 1
                    ? RawMouseEventType.LeftButtonUp
                    : evnt.Button == 3 ? RawMouseEventType.RightButtonUp : RawMouseEventType.MiddleButtonUp,
                new Point(evnt.X, evnt.Y), GetModifierKeys(evnt.State));
            Input(e);
        }

        void OnDestroyed(object sender, EventArgs eventArgs)
        {
            Closed();
        }

        private void ProcessKeyEvent(EventKey evnt)
        {
            
            _lastKeyEventTimestamp = evnt.Time;
            if (_imContext.FilterKeypress(evnt))
                return;
            var e = new RawKeyEventArgs(
                GtkKeyboardDevice.Instance,
                evnt.Time,
                evnt.Type == EventType.KeyPress ? RawKeyEventType.KeyDown : RawKeyEventType.KeyUp,
                GtkKeyboardDevice.ConvertKey(evnt.Key), GetModifierKeys(evnt.State));
            Input(e);
        }

        void OnKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
        {
            args.RetVal = true;
            ProcessKeyEvent(args.Event);
        }

        void OnKeyReleaseEvent(object o, Gtk.KeyReleaseEventArgs args)
        {
            args.RetVal = true;
            ProcessKeyEvent(args.Event);
        }

        private void ImContext_Commit(object o, Gtk.CommitArgs args)
        {
            Input(new RawTextInputEventArgs(GtkKeyboardDevice.Instance, _lastKeyEventTimestamp, args.Str));
        }

        void OnExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            Paint(args.Event.Area.ToAvalonia());
            args.RetVal = true;
        }

        void OnMotionNotifyEvent(object o, Gtk.MotionNotifyEventArgs args)
        {
            var evnt = args.Event;
            var position = new Point(evnt.X, evnt.Y);

            GtkMouseDevice.Instance.SetClientPosition(position);

            var e = new RawMouseEventArgs(
                GtkMouseDevice.Instance,
                evnt.Time,
                _inputRoot,
                RawMouseEventType.Move,
                position, GetModifierKeys(evnt.State));
            Input(e);
            args.RetVal = true;
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            _window.Hide();
            _window.Dispose();
            _window = null;
        }
    }
}
