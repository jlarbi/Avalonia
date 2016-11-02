// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Reactive.Subjects;

namespace Avalonia.Reactive
{
    /// <summary>
    /// Definition of the <see cref="AnonymousSubject{T}"/> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AnonymousSubject<T> : AnonymousSubject<T, T>, ISubject<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousSubject{T}"/> class.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="observable"></param>
        public AnonymousSubject(IObserver<T> observer, IObservable<T> observable)
            : base(observer, observable)
        {
        }
    }
}
