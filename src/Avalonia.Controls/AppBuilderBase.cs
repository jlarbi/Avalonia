﻿// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Reflection;
using System.Linq;
using Avalonia.Platform;

namespace Avalonia.Controls
{
    /// <summary>
    /// Base class for initializing platform-specific services for an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="TAppBuilder">The type of the AppBuilder class itself.</typeparam>
    public abstract class AppBuilderBase<TAppBuilder> where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
    {
        /// <summary>
        /// Gets or sets the <see cref="IRuntimePlatform"/> instance.
        /// </summary>
        public IRuntimePlatform RuntimePlatform { get; set; }

        /// <summary>
        /// Gets or sets a method to call the initialize the runtime platform services (e. g. AssetLoader)
        /// </summary>
        public Action RuntimePlatformServicesInitializer { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="Application"/> instance being initialized.
        /// </summary>
        public Application Instance { get; protected set; }

        /// <summary>
        /// Gets or sets a method to call the initialize the windowing subsystem.
        /// </summary>
        public Action WindowingSubsystemInitializer { get; private set; }

        /// <summary>
        /// Gets the name of the currently selected windowing subsystem.
        /// </summary>
        public string WindowingSubsystemName { get; private set; }

        /// <summary>
        /// Gets or sets a method to call the initialize the windowing subsystem.
        /// </summary>
        public Action RenderingSubsystemInitializer { get; private set; }

        /// <summary>
        /// Gets the name of the currently selected rendering subsystem.
        /// </summary>
        public string RenderingSubsystemName { get; private set; }

        /// <summary>
        /// Gets or sets a method to call after the <see cref="Application"/> is setup.
        /// </summary>
        public Action<TAppBuilder> AfterSetupCallback { get; private set; } = builder => { };

        /// <summary>
        /// Gets or sets a method to call before <see cref="Start{TMainWindow}"/> is called on the
        /// <see cref="Application"/>.
        /// </summary>
        public Action<TAppBuilder> BeforeStartCallback { get; private set; } = builder => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="AppBuilderBase{TAppBuilder}"/> class.
        /// </summary>
        /// <param name="platform">The platform the app will run on.</param>
        /// <param name="platformSevices">The services.</param>
        protected AppBuilderBase(IRuntimePlatform platform, Action platformSevices)
        {
            RuntimePlatform = platform;
            RuntimePlatformServicesInitializer = platformSevices;
        }

        /// <summary>
        /// Begin configuring an <see cref="Application"/>.
        /// </summary>
        /// <typeparam name="TApp">The subclass of <see cref="Application"/> to configure.</typeparam>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public static TAppBuilder Configure<TApp>()
            where TApp : Application, new()
        {
            return Configure(new TApp());
        }

        /// <summary>
        /// Begin configuring an <see cref="Application"/>.
        /// </summary>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public static TAppBuilder Configure(Application app)
        {
            AvaloniaLocator.CurrentMutable.BindToSelf(app);

            return new TAppBuilder()
            {
                Instance = app,
            };
        }

        /// <summary>
        /// Gets the application builder.
        /// </summary>
        protected TAppBuilder Self => (TAppBuilder) this;

        /// <summary>
        /// Registers a callback to call before <see cref="Start{TMainWindow}"/> is called on the
        /// <see cref="Application"/>.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public TAppBuilder BeforeStarting(Action<TAppBuilder> callback)
        {
            BeforeStartCallback = (Action<TAppBuilder>)Delegate.Combine(BeforeStartCallback, callback);
            return Self;
        }

        /// <summary>
        /// Post new setup action(s) to do on application setup ending.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>The application builder.</returns>
        public TAppBuilder AfterSetup(Action<TAppBuilder> callback)
        {
            AfterSetupCallback = (Action<TAppBuilder>)Delegate.Combine(AfterSetupCallback, callback);
            return Self;
        }

        /// <summary>
        /// Starts the application with an instance of <typeparamref name="TMainWindow"/>.
        /// </summary>
        /// <typeparam name="TMainWindow">The window type.</typeparam>
        public void Start<TMainWindow>()
            where TMainWindow : Window, new()
        {
            Setup();
            BeforeStartCallback(Self);

            var window = new TMainWindow();
            window.Show();
            Instance.Run(window);
        }

        /// <summary>
        /// Sets up the platform-specific services for the application, but does not run it.
        /// </summary>
        /// <returns></returns>
        public TAppBuilder SetupWithoutStarting()
        {
            Setup();
            return Self;
        }

        /// <summary>
        /// Specifies a windowing subsystem to use.
        /// </summary>
        /// <param name="initializer">The method to call to initialize the windowing subsystem.</param>
        /// <param name="name">The name of the windowing subsystem.</param>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public TAppBuilder UseWindowingSubsystem(Action initializer, string name = "")
        {
            WindowingSubsystemInitializer = initializer;
            WindowingSubsystemName = name;
            return Self;
        }

        /// <summary>
        /// Specifies a windowing subsystem to use.
        /// </summary>
        /// <param name="dll">The dll in which to look for subsystem.</param>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public TAppBuilder UseWindowingSubsystem(string dll) => UseWindowingSubsystem(GetInitializer(dll), dll.Replace("Avalonia.", string.Empty));

        /// <summary>
        /// Specifies a rendering subsystem to use.
        /// </summary>
        /// <param name="initializer">The method to call to initialize the rendering subsystem.</param>
        /// <param name="name">The name of the rendering subsystem.</param>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public TAppBuilder UseRenderingSubsystem(Action initializer, string name = "")
        {
            RenderingSubsystemInitializer = initializer;
            RenderingSubsystemName = name;
            return Self;
        }

        /// <summary>
        /// Specifies a rendering subsystem to use.
        /// </summary>
        /// <param name="dll">The dll in which to look for subsystem.</param>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public TAppBuilder UseRenderingSubsystem(string dll) => UseRenderingSubsystem(GetInitializer(dll));

        static Action GetInitializer(string assemblyName) => () =>
        {
            var assembly = Assembly.Load(new AssemblyName(assemblyName));
            var platformClassName = assemblyName.Replace("Avalonia.", string.Empty) + "Platform";
            var platformClassFullName = assemblyName + "." + platformClassName;
            var platformClass = assembly.GetType(platformClassFullName);
            var init = platformClass.GetRuntimeMethod("Initialize", new Type[0]);
            init.Invoke(null, null);
        };

        /// <summary>
        /// Add new modules setup for the application.
        /// </summary>
        /// <returns>The application builder.</returns>
        public TAppBuilder UseAvaloniaModules() => AfterSetup(builder => SetupAvaloniaModules());

        private void SetupAvaloniaModules()
        {
            var moduleInitializers = from assembly in AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetLoadedAssemblies()
                                          from attribute in assembly.GetCustomAttributes<ExportAvaloniaModuleAttribute>()
                                          where attribute.ForWindowingSubsystem == ""
                                           || attribute.ForWindowingSubsystem == WindowingSubsystemName
                                          where attribute.ForRenderingSubsystem == ""
                                           || attribute.ForRenderingSubsystem == RenderingSubsystemName
                                          group attribute by attribute.Name into exports
                                          select (from export in exports
                                                  orderby export.ForWindowingSubsystem.Length descending
                                                  orderby export.ForRenderingSubsystem.Length descending
                                                  select export).First().ModuleType into moduleType
                                          select (from constructor in moduleType.GetTypeInfo().DeclaredConstructors
                                                  where constructor.GetParameters().Length == 0 && !constructor.IsStatic
                                                  select constructor).Single() into constructor
                                          select (Action)(() => constructor.Invoke(new object[0]));
            Delegate.Combine(moduleInitializers.ToArray()).DynamicInvoke();
        }

        /// <summary>
        /// Sets up the platform-speciic services for the <see cref="Application"/>.
        /// </summary>
        private void Setup()
        {
            if (Instance == null)
            {
                throw new InvalidOperationException("No App instance configured.");
            }

            if (RuntimePlatformServicesInitializer == null)
            {
                throw new InvalidOperationException("No runtime platform services configured.");
            }

            if (WindowingSubsystemInitializer == null)
            {
                throw new InvalidOperationException("No windowing system configured.");
            }

            if (RenderingSubsystemInitializer == null)
            {
                throw new InvalidOperationException("No rendering system configured.");
            }

            Instance.RegisterServices();
            RuntimePlatformServicesInitializer();
            WindowingSubsystemInitializer();
            RenderingSubsystemInitializer();
            Instance.Initialize();
            AfterSetupCallback(Self);
        }
    }
}
