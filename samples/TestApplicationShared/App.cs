// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Default;
using Avalonia.Diagnostics;
using Avalonia.Platform;
using Avalonia.Shared.PlatformSupport;
using Avalonia.Media;

namespace TestApplication
{
    /// <summary>
    /// Definition of the <see cref="App"/> class.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Runs the application.
        /// </summary>
        public void Run()
        {
            Styles.Add(new DefaultTheme());

            var loader = new AvaloniaXamlLoader();
            var baseLight = (IStyle)loader.Load(
                new Uri("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default"));
            Styles.Add(baseLight);

            Styles.Add(new SampleTabStyle());
            DataTemplates = new DataTemplates
            {
                new FuncTreeDataTemplate<Node>(
                    x => new TextBlock {Text = x.Name},
                    x => x.Children),
            };

            MainWindow.RootNamespace = "TestApplication";
            var wnd = MainWindow.Create(); 
            wnd.AttachDevTools();

            Run(wnd);
        }

        /// <summary>
        /// Creates the application's simple window.
        /// </summary>
        /// <returns>The window.</returns>
        // This provides a simple UI tree for testing input handling, drawing, etc
        public static Window CreateSimpleWindow()
        {
            Window window = new Window
            {
                Title = "Avalonia Test Application",
                Background = Brushes.Red,
                Content = new StackPanel
                {
                    Margin = new Thickness(30),
                    Background = Brushes.Yellow,
                    Children = new Controls
                    {
                        new TextBlock
                        {
                            Text = "TEXT BLOCK",
                            Width = 300,
                            Height = 40,
                            Background = Brushes.White,
                            Foreground = Brushes.Black
                        },

                        new Button
                        {
                            Content = "BUTTON",
                            Width = 150,
                            Height = 40,
                            Background = Brushes.LightGreen,
                            Foreground = Brushes.Black
                        }

                    }
                }
            };

            return window;
        }
    }
}
