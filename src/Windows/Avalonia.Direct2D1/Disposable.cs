// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Direct2D1
{
    /// <summary>
    /// Definition of the <see cref="Disposable{T}"/> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Disposable<T> : IDisposable where T : IDisposable
    {
        private readonly IDisposable _extra;

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable{T}"/> class.
        /// </summary>
        /// <param name="inner">The disposable object.</param>
        public Disposable(T inner)
        {
            Inner = inner;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable{T}"/> class.
        /// </summary>
        /// <param name="inner">The disposable object.</param>
        /// <param name="extra">An extra</param>
        public Disposable(T inner, IDisposable extra)
        {
            Inner = inner;
            _extra = extra;
        }

        /// <summary>
        /// Gets the inner dispoable object.
        /// </summary>
        public T Inner { get; }

        /// <summary>
        /// Implicit cast from disposable from inner object type.
        /// </summary>
        /// <param name="i"></param>
        public static implicit operator T(Disposable<T> i)
        {
            return i.Inner;
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            Inner.Dispose();
            _extra?.Dispose();
        }
    }
}
