// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Data
{
    /// <summary>
    /// Definition of the <see cref="IndexerBinding"/> class.
    /// </summary>
    public class IndexerBinding : IBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexerBinding"/> class.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="mode"></param>
        public IndexerBinding(
            IAvaloniaObject source,
            AvaloniaProperty property,
            BindingMode mode)
        {
            Source = source;
            Property = property;
            Mode = mode;
        }

        /// <summary>
        /// Gets the binding source.
        /// </summary>
        private IAvaloniaObject Source { get; }

        /// <summary>
        /// Gets the property involved in the binding.
        /// </summary>
        public AvaloniaProperty Property { get; }

        /// <summary>
        /// Gets the binding mode.
        /// </summary>
        private BindingMode Mode { get; }

        /// <summary>
        /// Initializes a new <see cref="InstancedBinding"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetProperty"></param>
        /// <param name="anchor"></param>
        /// <param name="enableDataValidation"></param>
        /// <returns></returns>
        public InstancedBinding Initiate(
            IAvaloniaObject target,
            AvaloniaProperty targetProperty,
            object anchor = null,
            bool enableDataValidation = false)
        {
            var mode = Mode == BindingMode.Default ?
                targetProperty.GetMetadata(target.GetType()).DefaultBindingMode :
                Mode;

            if (mode == BindingMode.TwoWay)
            {
                return new InstancedBinding(Source.GetSubject(Property), mode);
            }
            else
            {
                return new InstancedBinding(Source.GetObservable(Property), mode);
            }
        }
    }
}
