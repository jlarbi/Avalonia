// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Xunit;

namespace Avalonia.Interactivity.UnitTests
{
    /// <summary>
    /// Definition of the <see cref="InteractiveTests"/> class.
    /// </summary>
    public class InteractiveTests
    {
        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Direct_Event_Should_Go_Straight_To_Source()
        {
            var ev = new RoutedEvent("test", RoutingStrategies.Direct, typeof(RoutedEventArgs), typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);
            var target = CreateTree(ev, handler, RoutingStrategies.Direct);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "2b" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Direct_Event_Should_Have_Route_Set_To_Direct()
        {
            var ev = new RoutedEvent("test", RoutingStrategies.Direct, typeof(RoutedEventArgs), typeof(TestInteractive));
            bool called = false;

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                Assert.Equal(RoutingStrategies.Direct, e.Route);
                called = true;
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Direct);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.True(called);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Bubbling_Event_Should_Bubble_Up()
        {
            var ev = new RoutedEvent("test", RoutingStrategies.Bubble, typeof(RoutedEventArgs), typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);
            var target = CreateTree(ev, handler, RoutingStrategies.Bubble | RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "2b", "1" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Tunneling_Event_Should_Tunnel()
        {
            var ev = new RoutedEvent("test", RoutingStrategies.Tunnel, typeof(RoutedEventArgs), typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);
            var target = CreateTree(ev, handler, RoutingStrategies.Bubble | RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "1", "2b" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Tunneling_Bubbling_Event_Should_Tunnel_Then_Bubble_Up()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);
            var target = CreateTree(ev, handler, RoutingStrategies.Bubble | RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "1", "2b", "2b", "1" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Events_Should_Have_Route_Set()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var invoked = new List<RoutingStrategies>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(e.Route);
            var target = CreateTree(ev, handler, RoutingStrategies.Bubble | RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[]
            {
                RoutingStrategies.Tunnel,
                RoutingStrategies.Tunnel,
                RoutingStrategies.Bubble,
                RoutingStrategies.Bubble,
            },
            invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Handled_Bubbled_Event_Should_Not_Propogate_Further()
        {
            var ev = new RoutedEvent("test", RoutingStrategies.Bubble, typeof(RoutedEventArgs), typeof(TestInteractive));
            var invoked = new List<string>();

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                var t = (TestInteractive)s;
                invoked.Add(t.Name);
                e.Handled = t.Name == "2b";
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Bubble);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "2b" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Handled_Tunnelled_Event_Should_Not_Propogate_Further()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel, 
                typeof(RoutedEventArgs), 
                typeof(TestInteractive));
            var invoked = new List<string>();

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                var t = (TestInteractive)s;
                invoked.Add(t.Name);
                e.Handled = t.Name == "2b";
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Bubble | RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "1", "2b" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Direct_Subscription_Should_Not_Catch_Tunneling_Or_Bubbling()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var count = 0;

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                ++count;
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Direct);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(0, count);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Bubbling_Subscription_Should_Not_Catch_Tunneling()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var count = 0;

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                Assert.Equal(RoutingStrategies.Bubble, e.Route);
                ++count;
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Bubble);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(2, count);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Tunneling_Subscription_Should_Not_Catch_Bubbling()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var count = 0;

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                Assert.Equal(RoutingStrategies.Tunnel, e.Route);
                ++count;
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(2, count);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Event_Should_Should_Keep_Propogating_To_HandedEventsToo_Handlers()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var invoked = new List<string>();

            EventHandler<RoutedEventArgs> handler = (s, e) =>
            {
                invoked.Add(((TestInteractive)s).Name);
                e.Handled = true;
            };

            var target = CreateTree(ev, handler, RoutingStrategies.Bubble | RoutingStrategies.Tunnel, true);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "1", "2b", "2b", "1" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Direct_Class_Handlers_Should_Be_Called()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Direct,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);

            var target = CreateTree(ev, null, 0);

            ev.AddClassHandler(typeof(TestInteractive), handler, RoutingStrategies.Direct);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "2b" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Tunneling_Class_Handlers_Should_Be_Called()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);

            var target = CreateTree(ev, null, 0);

            ev.AddClassHandler(typeof(TestInteractive), handler, RoutingStrategies.Tunnel);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "1", "2b" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Bubbling_Class_Handlers_Should_Be_Called()
        {
            var ev = new RoutedEvent(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(RoutedEventArgs),
                typeof(TestInteractive));
            var invoked = new List<string>();
            EventHandler<RoutedEventArgs> handler = (s, e) => invoked.Add(((TestInteractive)s).Name);

            var target = CreateTree(ev, null, 0);

            ev.AddClassHandler(typeof(TestInteractive), handler, RoutingStrategies.Bubble);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.Equal(new[] { "2b", "1" }, invoked);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Typed_Class_Handlers_Should_Be_Called()
        {
            var ev = new RoutedEvent<RoutedEventArgs>(
                "test",
                RoutingStrategies.Bubble | RoutingStrategies.Tunnel,
                typeof(TestInteractive));

            var target = CreateTree(ev, null, 0);

            ev.AddClassHandler<TestInteractive>(x => x.ClassHandler, RoutingStrategies.Bubble);

            var args = new RoutedEventArgs(ev, target);
            target.RaiseEvent(args);

            Assert.True(target.ClassHandlerInvoked);
            Assert.True(target.GetVisualParent<TestInteractive>().ClassHandlerInvoked);
        }

        /// <summary>
        /// Creates a tree for the test.
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="handler"></param>
        /// <param name="handlerRoutes"></param>
        /// <param name="handledEventsToo"></param>
        /// <returns></returns>
        private TestInteractive CreateTree(
            RoutedEvent ev,
            EventHandler<RoutedEventArgs> handler,
            RoutingStrategies handlerRoutes,
            bool handledEventsToo = false)
        {
            TestInteractive target;

            var tree = new TestInteractive
            {
                Name = "1",
                Children = new[]
                {
                    new TestInteractive
                    {
                        Name = "2a",
                    },
                    (target = new TestInteractive
                    {
                        Name = "2b",
                        Children = new[]
                        {
                            new TestInteractive
                            {
                                Name = "3",
                            },
                        },
                    }),
                }
            };

            if (handler != null)
            {
                foreach (var i in tree.GetSelfAndVisualDescendents().Cast<Interactive>())
                {
                    i.AddHandler(ev, handler, handlerRoutes, handledEventsToo);
                }
            }

            return target;
        }

        /// <summary>
        /// Definition of the <see cref="TestInteractive"/> class.
        /// </summary>
        private class TestInteractive : Interactive
        {
            public bool ClassHandlerInvoked { get; private set; }
            public string Name { get; set; }

            public IEnumerable<IVisual> Children
            {
                get
                {
                    return ((IVisual)this).VisualChildren.AsEnumerable();
                }

                set
                {
                    VisualChildren.AddRange(value.Cast<Visual>());
                }
            }

            public void ClassHandler(RoutedEventArgs e)
            {
                ClassHandlerInvoked = true;
            }
        }
    }
}
