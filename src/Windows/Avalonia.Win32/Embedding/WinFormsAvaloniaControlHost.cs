using System;
using System.ComponentModel;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Controls.Embedding;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Win32.Interop;
using WinFormsControl = System.Windows.Forms.Control;

namespace Avalonia.Win32.Embedding
{
    /// <summary>
    /// Definition fo the <see cref="WinFormsAvaloniaControlHost"/> class.
    /// </summary>
    [ToolboxItem(true)]
    public class WinFormsAvaloniaControlHost : WinFormsControl
    {
        private readonly EmbeddableControlRoot _root = new EmbeddableControlRoot();

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsAvaloniaControlHost"/> class.
        /// </summary>
        public WinFormsAvaloniaControlHost()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            UnmanagedMethods.SetParent(_root.PlatformImpl.Handle.Handle, Handle);
            _root.Prepare();
            if (_root.IsFocused)
                FocusManager.Instance.Focus(null);
            _root.GotFocus += RootGotFocus;
            _root.PlatformImpl.LostFocus += PlatformImpl_LostFocus;
            FixPosition();
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public Avalonia.Controls.Control Content
        {
            get { return (Avalonia.Controls.Control)_root.Content; }
            set { _root.Content = value; }
        }

        /// <summary>
        /// Stop focusing the control.
        /// </summary>
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

        private void PlatformImpl_LostFocus()
        {
            Unfocus();
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _root.Dispose();
            base.Dispose(disposing);
        }

        private void RootGotFocus(object sender, Interactivity.RoutedEventArgs e)
        {
            UnmanagedMethods.SetFocus(_root.PlatformImpl.Handle.Handle);
        }

        /// <summary>
        /// Called when got focus event occurs.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (_root != null)
                UnmanagedMethods.SetFocus(_root.PlatformImpl.Handle.Handle);
        }


        void FixPosition()
        {
            if (_root != null && Width > 0 && Height > 0)
                UnmanagedMethods.MoveWindow(_root.PlatformImpl.Handle.Handle, 0, 0, Width, Height, true);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Resize event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnResize(EventArgs e)
        {
            FixPosition();
            base.OnResize(e);
        }

        /// <summary>
        /// Delegate called on control paint.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {

        }
    }
}
