// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Avalonia.Styling
{
    /// <summary>
    /// Definition of the <see cref="ActivatorMode"/> enumeration.
    /// </summary>
    public enum ActivatorMode
    {
        /// <summary>
        /// And
        /// </summary>
        And,

        /// <summary>
        /// Or.
        /// </summary>
        Or,
    }

    /// <summary>
    /// Definition of the <see cref="StyleActivator"/> class.
    /// </summary>
    public static class StyleActivator
    {
        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static IObservable<bool> And(IList<IObservable<bool>> inputs)
        {
            if (inputs.Count == 0)
            {
                throw new ArgumentException("StyleActivator.And inputs may not be empty.");
            }
            else if (inputs.Count == 1)
            {
                return inputs[0];
            }
            else
            {
                return inputs.CombineLatest()
                    .Select(values => values.All(x => x))
                    .DistinctUntilChanged();
            }
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static IObservable<bool> Or(IList<IObservable<bool>> inputs)
        {
            if (inputs.Count == 0)
            {
                throw new ArgumentException("StyleActivator.Or inputs may not be empty.");
            }
            else if (inputs.Count == 1)
            {
                return inputs[0];
            }
            else
            {
                return inputs.CombineLatest()
                .Select(values => values.Any(x => x))
                .DistinctUntilChanged();
            }
        }
    }
}
