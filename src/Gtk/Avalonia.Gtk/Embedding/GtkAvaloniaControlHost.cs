using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Embedding;
using Avalonia.Diagnostics;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Platform;
using Avalonia.VisualTree;
using Gdk;
using Gtk;

namespace Avalonia.Gtk.Embedding
{
    /// <summary>
    /// Definition of the <see cref="GtkAvaloniaControlHost"/> class.
    /// </summary>
    public class GtkAvaloniaControlHost : DrawingArea, IPlatformHandle
    {
        private EmbeddableControlRoot _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="GtkAvaloniaControlHost"/> class.
        /// </summary>
        public GtkAvaloniaControlHost()
        {
            _root = new EmbeddableControlRoot(new EmbeddableImpl(this));
            _root.Prepare();
            if (_root.IsFocused)
                Unfocus();
            _root.GotFocus += RootGotFocus;
            CanFocus = true;
        }

        void Unfocus()
        {
            var focused = (IVisual)FocusManager.Instance.Current;
            if (focused == null)
                return;
            while (focused.VisualParent != null)
                focused = focused.VisualParent;

            if (focused == _root)
                KeyboardDevice.Instance.SetFocusedElement(null, NavigationMethod.Unspecified, InputModifiers.None);
        }

        /// <summary>
        /// Delegate called on loosing focus event.
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        protected override bool OnFocusOutEvent(EventFocus evnt)
        {
            Unfocus();
            return false;
        }

        private void RootGotFocus(object sender, RoutedEventArgs e)
        {
            this.HasFocus = true;
            GdkWindow.Focus(0);
        }

        private Control _content;

        /// <summary>
        /// Gets or sets the control host content.
        /// </summary>
        public Control Content
        {
            get { return _content; }
            set
            {
                _content = value;
                if (_root != null)
                {
                    _root.Content = value;
                    _root.Prepare();
                }
            }
        }

        IntPtr IPlatformHandle.Handle => PlatformHandleAwareWindow.GetNativeWindow(GdkWindow);

        string IPlatformHandle.HandleDescriptor => "HWND";
    }
}
