using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Avalonia.Controls;
using Avalonia.Win32.Interop;

namespace Avalonia.Win32.Embedding
{
    /// <summary>
    /// Definition of the <see cref="WpfAvaloniaControlHost"/> class.
    /// </summary>
    public class WpfAvaloniaControlHost : HwndHost
    {
        private WinFormsAvaloniaControlHost _host;
        private Avalonia.Controls.Control _content;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public Avalonia.Controls.Control Content
        {
            get { return _content; }
            set
            {
                if (_host != null)
                    _host.Content = value;
                _content = value;
                
            }
        }

        void DestroyHost()
        {
            _host?.Dispose();
            _host = null;
        }

        /// <summary>
        /// Builds the window.
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <returns></returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            DestroyHost();
            _host = new WinFormsAvaloniaControlHost {Content = _content};
            UnmanagedMethods.SetParent(_host.Handle, hwndParent.Handle);
            return new HandleRef(this, _host.Handle);
        }

        /// <summary>
        /// Destroys the window.
        /// </summary>
        /// <param name="hwnd"></param>
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyHost();
        }
    }
}
