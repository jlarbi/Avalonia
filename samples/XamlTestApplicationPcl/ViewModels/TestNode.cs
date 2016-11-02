// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;
using ReactiveUI;

namespace XamlTestApplication.ViewModels
{
    /// <summary>
    /// Definition of the <see cref="TestNode"/> class.
    /// </summary>
    public class TestNode : ReactiveObject
    {
        /// <summary>
        /// Stores the flag indicating whether the node is exanded or not.
        /// </summary>
        private bool _isExpanded;

        /// <summary>
        /// Gets or sets the node header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the node sub header.
        /// </summary>
        public string SubHeader { get; set; }

        /// <summary>
        /// Gets or sets the node's children.
        /// </summary>
        public IEnumerable<TestNode> Children { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the node is exanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.RaiseAndSetIfChanged(ref this._isExpanded, value); }
        }
    }
}