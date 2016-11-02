// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Controls;
using Xunit;

namespace Avalonia.Layout.UnitTests
{
    /// <summary>
    /// Definition of the <see cref="MeasureTests"/> class.
    /// </summary>
    public class MeasureTests
    {
        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Margin_Should_Be_Included_In_DesiredSize()
        {
            var decorator = new Decorator
            {
                Width = 100,
                Height = 100,
                Margin = new Thickness(8),
            };

            decorator.Measure(Size.Infinity);

            Assert.Equal(new Size(116, 116), decorator.DesiredSize);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Invalidating_Child_Should_Not_Invalidate_Parent()
        {
            var panel = new StackPanel();
            var child = new Border();
            panel.Children.Add(child);

            panel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            Assert.Equal(new Size(0, 0), panel.DesiredSize);

            child.Width = 100;
            child.Height = 100;

            Assert.True(panel.IsMeasureValid);
            Assert.False(child.IsMeasureValid);

            panel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Assert.Equal(new Size(0, 0), panel.DesiredSize);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Removing_From_Parent_Should_Invalidate_Measure_Of_Control_And_Descendents()
        {
            var panel = new StackPanel();
            var child2 = new Border();
            var child1 = new Border { Child = child2 };
            panel.Children.Add(child1);

            panel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Assert.True(child1.IsMeasureValid);
            Assert.True(child2.IsMeasureValid);

            panel.Children.Remove(child1);
            Assert.False(child1.IsMeasureValid);
            Assert.False(child2.IsMeasureValid);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Negative_Margin_Larger_Than_Constraint_Should_Request_Width_0()
        {
            Control target;

            var outer = new Decorator
            {
                Width = 100,
                Height = 100,
                Child = target = new Control
                {
                    Margin = new Thickness(-100, 0, 0, 0),
                }
            };

            outer.Measure(Size.Infinity);

            Assert.Equal(0, target.DesiredSize.Width);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        // TO DO: Comment...
        [Fact]
        public void Negative_Margin_Larger_Than_Constraint_Should_Request_Height_0()
        {
            Control target;

            var outer = new Decorator
            {
                Width = 100,
                Height = 100,
                Child = target = new Control
                {
                    Margin = new Thickness(0, -100, 0, 0),
                }
            };

            outer.Measure(Size.Infinity);

            Assert.Equal(0, target.DesiredSize.Height);
        }
    }
}
