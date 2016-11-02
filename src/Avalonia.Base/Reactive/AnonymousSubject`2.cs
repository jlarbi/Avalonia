// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Reactive.Subjects;

namespace Avalonia.Reactive
{
    /// <summary>
    /// Definition of the <see cref="AnonymousSubject{T, U}"/> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class AnonymousSubject<T, U> : ISubject<T, U>
    {
        private readonly IObserver<T> _observer;
        private readonly IObservable<U> _observable;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousSubject{T, U}"/> class.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="observable"></param>
        public AnonymousSubject(IObserver<T> observer, IObservable<U> observable)
        {
            _observer = observer;
            _observable = observable;
        }

        /// <summary>
        /// Called on completion.
        /// </summary>
        public void OnCompleted()
        {
            _observer.OnCompleted();
        }

        /// <summary>
        /// Called on errors.
        /// </summary>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            _observer.OnError(error);
        }

        /// <summary>
        /// Called on next.
        /// </summary>
        public void OnNext(T value)
        {
            _observer.OnNext(value);
        }

        /// <summary>
        /// Subscribes a new observer on the observable.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<U> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            //
            // [OK] Use of unsafe Subscribe: non-pretentious wrapping of an observable sequence.
            //
            return _observable.Subscribe/*Unsafe*/(observer);
        }
    }
}
