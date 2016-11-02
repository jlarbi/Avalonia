using System;
using System.Reactive.Disposables;
using Avalonia.Platform;
using Gdk;

namespace Avalonia.Gtk
{
    using Gtk = global::Gtk;

    /// <summary>
    /// Definition of the <see cref="WindowImpl"/> class.
    /// </summary>
    public class WindowImpl : WindowImplBase
    {
        /// <summary>
        /// Stores the casted widget into a window.
        /// </summary>
        private Gtk.Window mWindow;

        private Gtk.Window Window => mWindow ?? (mWindow = (Gtk.Window) Widget);

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowImpl"/> class.
        /// </summary>
        /// <param name="type">The window type.</param>
        public WindowImpl(Gtk.WindowType type) : base(new PlatformHandleAwareWindow(type))
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowImpl"/> class.
        /// </summary>
        public WindowImpl()
            : base(new PlatformHandleAwareWindow(Gtk.WindowType.Toplevel) {DefaultSize = new Gdk.Size(900, 480)})
        {
            Init();
        }

        /// <summary>
        /// Initializes the window.
        /// </summary>
        void Init()
        {
            Window.FocusActivated += OnFocusActivated;
            Window.ConfigureEvent += OnConfigureEvent;
            _lastClientSize = ClientSize;
            _lastPosition = Position;
        }
        private Size _lastClientSize;
        private Point _lastPosition;
        void OnConfigureEvent(object o, Gtk.ConfigureEventArgs args)
        {
            var evnt = args.Event;
            args.RetVal = true;
            var newSize = new Size(evnt.Width, evnt.Height);

            if (newSize != _lastClientSize)
            {
                Resized(newSize);
                _lastClientSize = newSize;
            }

            var newPosition = new Point(evnt.X, evnt.Y);
            
            if (newPosition != _lastPosition)
            {
                PositionChanged(newPosition);
                _lastPosition = newPosition;
            }
        }

        /// <summary>
        /// Gets or sets the client size.
        /// </summary>
        public override Size ClientSize
        {
            get
            {
                int width;
                int height;
                Window.GetSize(out width, out height);
                return new Size(width, height);
            }

            set
            {
                Window.Resize((int)value.Width, (int)value.Height);
            }
        }

        /// <summary>
        /// Sets the window title.
        /// </summary>
        /// <param name="title"></param>
        public override void SetTitle(string title)
        {
            Window.Title = title;
        }

        void OnFocusActivated(object sender, EventArgs eventArgs)
        {
            Activated();
        }

        /// <summary>
        /// Starts a drag move.
        /// </summary>
        public override void BeginMoveDrag()
        {
            int x, y;
            ModifierType mod;
            Window.Screen.RootWindow.GetPointer(out x, out y, out mod);
            Window.BeginMoveDrag(1, x, y, 0);
        }

        /// <summary>
        /// Starts a drag to resize move.
        /// </summary>
        /// <param name="edge"></param>
        public override void BeginResizeDrag(Controls.WindowEdge edge)
        {
            int x, y;
            ModifierType mod;
            Window.Screen.RootWindow.GetPointer(out x, out y, out mod);
            Window.BeginResizeDrag((Gdk.WindowEdge)(int)edge, 1, x, y, 0);
        }

        /// <summary>
        /// Gets or sets the window position.
        /// </summary>
        public override Point Position
        {
            get
            {
                int x, y;
                Window.GetPosition(out x, out y);
                return new Point(x, y);
            }
            set
            {
                Window.Move((int)value.X, (int)value.Y);
            }
        }

        /// <summary>
        /// Shows a the window as dialog box.
        /// </summary>
        /// <returns></returns>
        public override IDisposable ShowDialog()
        {
            Window.Modal = true;
            Window.Show();

            return Disposable.Empty;
        }

        /// <summary>
        /// Sets the system decorations.
        /// </summary>
        /// <param name="enabled"></param>
        public override void SetSystemDecorations(bool enabled) => Window.Decorated = enabled;

        /// <summary>
        /// Sets the window icon.
        /// </summary>
        /// <param name="icon"></param>
        public override void SetIcon(IWindowIconImpl icon)
        {
            Window.Icon = ((IconImpl)icon).Pixbuf;
        }
    }
}
