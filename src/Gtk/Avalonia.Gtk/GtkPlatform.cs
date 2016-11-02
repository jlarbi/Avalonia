// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Threading;
using Avalonia.Controls.Platform;
using Avalonia.Input.Platform;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Controls;

namespace Avalonia
{
    /// <summary>
    /// Definition of the <see cref="GtkApplicationExtensions"/> class.
    /// </summary>
    public static class GtkApplicationExtensions
    {
        /// <summary>
        /// Uses the Gtk windowing sub system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static T UseGtk<T>(this T builder) where T : AppBuilderBase<T>, new()
        {
            builder.UseWindowingSubsystem(Gtk.GtkPlatform.Initialize, "Gtk");
            return builder;
        }
    }
}

namespace Avalonia.Gtk
{
    using System.IO;
    using Rendering;
    using Gtk = global::Gtk;

    /// <summary>
    /// Definition of the <see cref="GtkPlatform"/> class.
    /// </summary>
    public class GtkPlatform : IPlatformThreadingInterface, IPlatformSettings, IWindowingPlatform, IPlatformIconLoader, IRendererFactory
    {
        private static readonly GtkPlatform s_instance = new GtkPlatform();
        private static Thread _uiThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="GtkPlatform"/> class.
        /// </summary>
        public GtkPlatform()
        {
            Gtk.Application.Init();
        }

        /// <summary>
        /// Gets the double click relevant area size.
        /// </summary>
        public Size DoubleClickSize => new Size(4, 4);

        /// <summary>
        /// Gets the double click relevant elapsed time.
        /// </summary>
        public TimeSpan DoubleClickTime => TimeSpan.FromMilliseconds(Gtk.Settings.Default.DoubleClickTime);

        /// <summary>
        /// Gets the render scaling factor.
        /// </summary>
        public double RenderScalingFactor { get; } = 1;

        /// <summary>
        /// Gets the layout scaling factor.
        /// </summary>
        public double LayoutScalingFactor { get; } = 1;

        /// <summary>
        /// Initializes the Gtk platform.
        /// </summary>
        public static void Initialize()
        {
            AvaloniaLocator.CurrentMutable
                .Bind<IWindowingPlatform>().ToConstant(s_instance)
                .Bind<IClipboard>().ToSingleton<ClipboardImpl>()
                .Bind<IStandardCursorFactory>().ToConstant(CursorFactory.Instance)
                .Bind<IKeyboardDevice>().ToConstant(GtkKeyboardDevice.Instance)
                .Bind<IMouseDevice>().ToConstant(GtkMouseDevice.Instance)
                .Bind<IPlatformSettings>().ToConstant(s_instance)
                .Bind<IPlatformThreadingInterface>().ToConstant(s_instance)
                .Bind<IRendererFactory>().ToConstant(s_instance)
                .Bind<IRenderLoop>().ToConstant(new DefaultRenderLoop(60))
                .Bind<ISystemDialogImpl>().ToSingleton<SystemDialogImpl>()
                .Bind<IPlatformIconLoader>().ToConstant(s_instance);
            _uiThread = Thread.CurrentThread;
        }

        /// <summary>
        /// Checks whether there are messages or not.
        /// </summary>
        /// <returns></returns>
        public bool HasMessages()
        {
            return Gtk.Application.EventsPending();
        }

        /// <summary>
        /// Processes messages.
        /// </summary>
        public void ProcessMessage()
        {
            Gtk.Application.RunIteration();
        }

        /// <summary>
        /// Runs the application loop.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public void RunLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
                Gtk.Application.RunIteration();
        }

        /// <summary>
        /// Starts the application timer.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        public IDisposable StartTimer(TimeSpan interval, Action tick)
        {
            var result = true;
            var handle = GLib.Timeout.Add(
                (uint)interval.TotalMilliseconds,
                () =>
                {
                    tick();
                    return result;
                });

            return Disposable.Create(() => result = false);
        }

        /// <summary>
        /// Signals about what???
        /// </summary>
        // TO DO: Comment...
        public void Signal()
        {
            Gtk.Application.Invoke(delegate { Signaled?.Invoke(); });
        }

        /// <summary>
        /// Checks whether the current thread is this owner thread.
        /// </summary>
        public bool CurrentThreadIsLoopThread => Thread.CurrentThread == _uiThread;

        /// <summary>
        /// Event raised on Gtk platform signal calls.
        /// </summary>
        public event Action Signaled;

        /// <summary>
        /// Creates a Gtk window.
        /// </summary>
        /// <returns>The window.</returns>
        public IWindowImpl CreateWindow()
        {
            return new WindowImpl();
        }

        /// <summary>
        /// Creates a new Gtk embeddable window.
        /// </summary>
        /// <returns>The embeddable window.</returns>
        public IEmbeddableWindowImpl CreateEmbeddableWindow() => new EmbeddableImpl();

        /// <summary>
        /// Creates a new Gtk pop up.
        /// </summary>
        /// <returns></returns>
        public IPopupImpl CreatePopup()
        {
            return new PopupImpl();
        }

        /// <summary>
        /// Creates a new Gtk renderer.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="renderLoop"></param>
        /// <returns></returns>
        public IRenderer CreateRenderer(IRenderRoot root, IRenderLoop renderLoop)
        {
            return new Renderer(root, renderLoop);
        }

        /// <summary>
        /// Loads an icon from file.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The icon.</returns>
        public IWindowIconImpl LoadIcon(string fileName)
        {
            return new IconImpl(new Gdk.Pixbuf(fileName));
        }

        /// <summary>
        /// Loads an icon from stream.
        /// </summary>
        /// <param name="stream">The stream the icon must be loaded from.</param>
        /// <returns>The icon.</returns>
        public IWindowIconImpl LoadIcon(Stream stream)
        {
            return new IconImpl(new Gdk.Pixbuf(stream));
        }

        /// <summary>
        /// Loads an icon using a bitmap
        /// </summary>
        /// <param name="bitmap">The bitmap to use to fill the icon.</param>
        /// <returns>The icon.</returns>
        public IWindowIconImpl LoadIcon(IBitmapImpl bitmap)
        {
            if (bitmap is Gdk.Pixbuf)
            {
                return new IconImpl((Gdk.Pixbuf)bitmap);
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream);
                    return new IconImpl(new Gdk.Pixbuf(memoryStream));
                } 
            }
        }
    }
}