// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Markup.Xaml;

namespace VirtualizationTest
{
    /// <summary>
    /// Definition of the <see cref="App"/> class.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Initializes the application.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
