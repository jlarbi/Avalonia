using System;
using Avalonia.Controls.Platform;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform;
using Avalonia.Styling;

namespace Avalonia.Controls.Embedding
{
    /// <summary>
    /// Definition of the <see cref="EmbeddableControlRoot"/> class.
    /// </summary>
    public class EmbeddableControlRoot : TopLevel, IStyleable, IFocusScope, INameScope, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddableControlRoot"/> class.
        /// </summary>
        /// <param name="impl">The window.</param>
        public EmbeddableControlRoot(IEmbeddableWindowImpl impl) : base(impl)
        {
            PlatformImpl.Show();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddableControlRoot"/> class.
        /// </summary>
        public EmbeddableControlRoot() : base(PlatformManager.CreateEmbeddableWindow())
        {
            PlatformImpl.Show();
        }

        /// <summary>
        /// Gets the platform-specific window
        /// </summary>
        public new IEmbeddableWindowImpl PlatformImpl => (IEmbeddableWindowImpl) base.PlatformImpl;

        /// <summary>
        /// Prepares the control host.
        /// </summary>
        public void Prepare()
        {
            EnsureInitialized();
            ApplyTemplate();
            PlatformImpl.Show();
            LayoutManager.Instance.ExecuteInitialLayoutPass(this);
        }

        /// <summary>
        /// Ensures the control host is initialized.
        /// </summary>
        private void EnsureInitialized()
        {
            if (!this.IsInitialized)
            {
                var init = (ISupportInitialize)this;
                init.BeginInit();
                init.EndInit();
            }
        }

        /// <summary>
        /// Measures the control and its child elements as part of a layout pass.
        /// </summary>
        /// <param name="availableSize">The size available to the control.</param>
        /// <returns>The desired size for the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            base.MeasureOverride(PlatformImpl.ClientSize);
            return PlatformImpl.ClientSize;
        }

        private readonly NameScope _nameScope = new NameScope();

        /// <summary>
        /// Raised when an element is registered with the name scope.
        /// </summary>
        public event EventHandler<NameScopeEventArgs> Registered
        {
            add { _nameScope.Registered += value; }
            remove { _nameScope.Registered -= value; }
        }

        /// <summary>
        /// Raised when an element is unregistered with the name scope.
        /// </summary>
        public event EventHandler<NameScopeEventArgs> Unregistered
        {
            add { _nameScope.Unregistered += value; }
            remove { _nameScope.Unregistered -= value; }
        }

        /// <summary>
        /// Registers the name and element into the name scope.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="element"></param>
        public void Register(string name, object element) => _nameScope.Register(name, element);

        /// <summary>
        /// Finds the associated element given its scope name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Find(string name) => _nameScope.Find(name);

        /// <summary>
        /// Unregisters the scoped element given its scope name.
        /// </summary>
        /// <param name="name"></param>
        public void Unregister(string name) => _nameScope.Unregister(name);

        Type IStyleable.StyleKey => typeof(EmbeddableControlRoot);

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            PlatformImpl.Dispose();
        }
    }
}
