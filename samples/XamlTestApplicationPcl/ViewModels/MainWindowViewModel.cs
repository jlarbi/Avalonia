// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using ReactiveUI;
using Avalonia.Controls;

namespace XamlTestApplication.ViewModels
{
    /// <summary>
    /// Definition of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            Items = new List<TestItem>();

            for (int i = 0; i < 10; ++i)
            {
                Items.Add(new TestItem($"Item {i}", $"Item {i} Value"));
            }

            Nodes = new List<TestNode>
            {
                new TestNode
                {
                    Header = "Root",
                    SubHeader = "Root Item",
                    IsExpanded = true,
                    Children = new[]
                    {
                        new TestNode
                        {
                            Header = "Child 1",
                            SubHeader = "Child 1 Value",
                        },
                        new TestNode
                        {
                            Header = "Child 2",
                            SubHeader = "Child 2 Value",
                            IsExpanded = false,
                            Children = new[]
                            {
                                new TestNode
                                {
                                    Header = "Grandchild",
                                    SubHeader = "Grandchild Value",
                                },
                                new TestNode
                                {
                                    Header = "Grandmaster Flash",
                                    SubHeader = "White Lines",
                                },
                            }
                        },
                    }
                }
            };

            
        


        CollapseNodesCommand = ReactiveCommand.Create();
            CollapseNodesCommand.Subscribe(_ => ExpandNodes(false));
            ExpandNodesCommand = ReactiveCommand.Create();
            ExpandNodesCommand.Subscribe(_ => ExpandNodes(true));

            OpenFileCommand = ReactiveCommand.Create();
            OpenFileCommand.Subscribe(_ =>
            {
                var ofd = new OpenFileDialog();

                ofd.ShowAsync();
            });

            OpenFolderCommand = ReactiveCommand.Create();
            OpenFolderCommand.Subscribe(_ =>
            {
                var ofd = new OpenFolderDialog();

                ofd.ShowAsync();
            });

            shell = ShellViewModel.Instance;
        }

        /// <summary>
        /// Stores the shell view model.
        /// </summary>
        private ShellViewModel shell;

        /// <summary>
        /// Gets pr sets the shell view model.
        /// </summary>
        public ShellViewModel Shell
        {
            get { return shell; }
            set { shell = value; }
        }

        /// <summary>
        /// Gets the view model items.
        /// </summary>
        public List<TestItem> Items { get; }

        /// <summary>
        /// Gets the view model nodes.
        /// </summary>
        public List<TestNode> Nodes { get; }

        /// <summary>
        /// Gets the collapsed nodes command.
        /// </summary>
        public ReactiveCommand<object> CollapseNodesCommand { get; }

        /// <summary>
        /// Gets the expand nodes command.
        /// </summary>
        public ReactiveCommand<object> ExpandNodesCommand { get; }

        /// <summary>
        /// Gets the open file command.
        /// </summary>
        public ReactiveCommand<object> OpenFileCommand { get; }

        /// <summary>
        /// Gets the open folder command.
        /// </summary>
        public ReactiveCommand<object> OpenFolderCommand { get; }

        /// <summary>
        /// Expands nodes.
        /// </summary>
        /// <param name="expanded">The flag indicating whether nodes are expanded or collapsed.</param>
        public void ExpandNodes(bool expanded)
        {
            foreach (var node in Nodes)
            {
                ExpandNodes(node, expanded);
            }
        }

        /// <summary>
        /// Expands nodes. (recursive)
        /// </summary>
        /// <param name="node">The node to expand or collaspe.</param>
        /// <param name="expanded">The flag indicating whether nodes are expanded or collapsed.</param>
        private void ExpandNodes(TestNode node, bool expanded)
        {
            node.IsExpanded = expanded;

            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    ExpandNodes(child, expanded);
                }
            }
        }
    }
}
