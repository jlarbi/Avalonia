// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Markup.Xaml;
using XamlTestApplication.ViewModels;

namespace XamlTestApplication.Views
{
    /// <summary>
    /// Definition of the <see cref="MainWindow"/> class.
    /// </summary>
    public class MainWindow : Window
    {
        /// <summary>
        /// Stores the exit menu item.
        /// </summary>
        private MenuItem _exitMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            DevTools.Attach(this);
        }

        /// <summary>
        /// Initializes the main window.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _exitMenu = this.FindControl<MenuItem>("exitMenu");
            _exitMenu.Click += (s, e) => Application.Current.Exit();
        }
    }
}