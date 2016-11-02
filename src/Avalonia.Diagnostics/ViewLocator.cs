// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Avalonia.Diagnostics
{
    /// <summary>
    /// Definition of the <see cref="ViewLocator{TViewModel}"/> class.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class ViewLocator<TViewModel> : IDataTemplate
    {
        /// <summary>
        /// Gets the flag indicating whether the view locator supports recycling or not.
        /// </summary>
        public bool SupportsRecycling => false;

        /// <summary>
        /// Builds the corresponding view element for the given object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The view control.</returns>
        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = name };
            }
        }

        /// <summary>
        /// Checks whether the view locator supports the given object type.
        /// </summary>
        /// <param name="data">The object to check for support.</param>
        /// <returns>True if of the supported type, false otherwise.</returns>
        public bool Match(object data)
        {
            return data is TViewModel;
        }
    }
}