using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Utilities
{
    /// <summary>
    /// Stores either a single key value pair or constructs a dictionary when more than one value is stored.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class SingleOrDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private KeyValuePair<TKey, TValue>? _singleValue;
        private Dictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Adds a new key/value pair.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (_singleValue != null)
            {
                dictionary = new Dictionary<TKey, TValue>();
                ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(_singleValue.Value);
                _singleValue = null;
            }

            if (dictionary != null)
            {
                dictionary.Add(key, value);
            }
            else
            {
                _singleValue = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        /// <summary>
        /// Attempts to retrieve a value from its key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The resulting value of found, null otherwise.</param>
        /// <returns>True if found, false otherwise.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (dictionary == null)
            {
                if (!_singleValue.HasValue || !_singleValue.Value.Key.Equals(key))
                {
                    value = default(TValue);
                    return false;
                }
                else
                {
                    value = _singleValue.Value.Value;
                    return true;
                }
            }
            else
            {
                return dictionary.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// Gets the container enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (dictionary == null)
            {
                if (_singleValue.HasValue)
                {
                    return new SingleEnumerator<KeyValuePair<TKey, TValue>>(_singleValue.Value);
                }
            }
            else
            {
                return dictionary.GetEnumerator();
            }
            return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
        }

        /// <summary>
        /// Gets the container enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets all values.
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                if(dictionary == null)
                {
                    if (_singleValue.HasValue)
                    {
                        return new[] { _singleValue.Value.Value };
                    }
                }
                else
                {
                    return dictionary.Values;
                }
                return Enumerable.Empty<TValue>();
            }
        }

        /// <summary>
        /// Definition of the <see cref="SingleEnumerator{T}"/> class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class SingleEnumerator<T> : IEnumerator<T>
        {
            private T value;
            private int index = -1;

            /// <summary>
            /// Initializes a new instance of the <see cref="SingleEnumerator{T}"/> class.
            /// </summary>
            /// <param name="value"></param>
            public SingleEnumerator(T value)
            {
                this.value = value;
            }

            /// <summary>
            /// Gets the current object.
            /// </summary>
            public T Current
            {
                get
                {
                    if (index == 0)
                    {
                        return value;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            /// <summary>
            /// Gets the current object.
            /// </summary>
            object IEnumerator.Current => Current;

            /// <summary>
            /// Releases resources.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Moves to the next object.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                index++;
                return index < 1;
            }

            /// <summary>
            /// Resets the iterator.
            /// </summary>
            public void Reset()
            {
                index = -1;
            }
        }

    }
}
