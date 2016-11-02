// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace XamlTestApplication.ViewModels
{
    /// <summary>
    /// Definition of the <see cref="TestItem"/> class.
    /// </summary>
    public class TestItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestItem"/> class.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="subheader"></param>
        public TestItem(string header, string subheader)
        {
            Header = header;
            SubHeader = subheader;
        }

        /// <summary>
        /// Gets the item header.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// Gets the item sub header.
        /// </summary>
        public string SubHeader { get; }
    }
}
