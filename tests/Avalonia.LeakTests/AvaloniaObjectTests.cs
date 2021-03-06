﻿using System;
using System.Reactive.Subjects;
using JetBrains.dotMemoryUnit;
using Xunit;
using Xunit.Abstractions;

namespace Avalonia.LeakTests
{
    /// <summary>
    /// Definition of the <see cref="AvaloniaObjectTests"/> class.
    /// </summary>
    public class AvaloniaObjectTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvaloniaObjectTests"/> class.
        /// </summary>
        /// <param name="atr"></param>
        public AvaloniaObjectTests(ITestOutputHelper atr)
        {
            DotMemoryUnitTestOutput.SetOutputMethod(atr.WriteLine);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Binding_To_Direct_Property_Does_Not_Get_Collected()
        {
            var target = new Class1();

            Func<WeakReference> setupBinding = () =>
            {
                var source = new Subject<string>();
                var sub = target.Bind((AvaloniaProperty)Class1.FooProperty, source);
                source.OnNext("foo");
                return new WeakReference(source);
            };

            var weakSource = setupBinding();

            GC.Collect();

            Assert.Equal("foo", target.Foo);
            Assert.True(weakSource.IsAlive);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Binding_To_Direct_Property_Gets_Collected_When_Completed()
        {
            var target = new Class1();

            Func<WeakReference> setupBinding = () =>
            {
                var source = new Subject<string>();
                var sub = target.Bind((AvaloniaProperty)Class1.FooProperty, source);
                return new WeakReference(source);
            };

            var weakSource = setupBinding();

            Action completeSource = () =>
            {
                ((ISubject<string>)weakSource.Target).OnCompleted();
            };

            completeSource();
            GC.Collect();

            Assert.False(weakSource.IsAlive);
        }

        /// <summary>
        /// Definition of the <see cref="Class1"/> class.
        /// </summary>
        private class Class1 : AvaloniaObject
        {
            public static readonly DirectProperty<Class1, string> FooProperty =
                AvaloniaProperty.RegisterDirect<Class1, string>(
                    "Foo",
                    o => o.Foo,
                    (o, v) => o.Foo = v,
                    unsetValue: "unset");

            private string _foo = "initial2";

            static Class1()
            {
            }

            public string Foo
            {
                get { return _foo; }
                set { SetAndRaise(FooProperty, ref _foo, value); }
            }
        }
    }
}
