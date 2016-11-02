// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Layout;
using Splat;

namespace Avalonia.Diagnostics
{
    /// <summary>
    /// Definition of the <see cref="LogManager"/> class.
    /// </summary>
    public class LogManager : ILogManager
    {
        private static LogManager s_instance;

        /// <summary>
        /// Gets the <see cref="LogManager"/> instance.
        /// </summary>
        public static LogManager Instance => s_instance ?? (s_instance = new LogManager());

        /// <summary>
        /// Gets or sets the current logger.
        /// </summary>
        public ILogger Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property messages must be logged or not.
        /// </summary>
        public bool LogPropertyMessages
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the layout messages must be logged or not.
        /// </summary>
        public bool LogLayoutMessages
        {
            get;
            set;
        }

        /// <summary>
        /// Enables the given logger as the current one.
        /// </summary>
        /// <param name="logger">Teh new current logger.</param>
        public static void Enable(ILogger logger)
        {
            Instance.Logger = logger;
            Locator.CurrentMutable.Register(() => Instance, typeof(ILogManager));
        }

        /// <summary>
        /// Gets the logger corresponding to the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFullLogger GetLogger(Type type)
        {
            if ((type == typeof(AvaloniaObject) && LogPropertyMessages) ||
                (type == typeof(Layoutable) && LogLayoutMessages))
            {
                return new WrappingFullLogger(Logger, type);
            }
            else
            {
                return new WrappingFullLogger(new NullLogger(), type);
            }
        }
    }
}
