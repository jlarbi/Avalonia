// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using Avalonia.Platform;
using DWrite = SharpDX.DirectWrite;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="FormattedTextImpl"/> class.
    /// </summary>
    public class FormattedTextImpl : IFormattedTextImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedTextImpl"/> class.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontStyle"></param>
        /// <param name="textAlignment"></param>
        /// <param name="fontWeight"></param>
        /// <param name="wrapping"></param>
        public FormattedTextImpl(
            string text,
            string fontFamily,
            double fontSize,
            FontStyle fontStyle,
            TextAlignment textAlignment,
            FontWeight fontWeight,
            TextWrapping wrapping)
        {
            var factory = AvaloniaLocator.Current.GetService<DWrite.Factory>();

            using (var format = new DWrite.TextFormat(
                factory,
                fontFamily,
                (DWrite.FontWeight)fontWeight,
                (DWrite.FontStyle)fontStyle,
                (float)fontSize))
            {
                format.WordWrapping = wrapping == TextWrapping.Wrap ? 
                    DWrite.WordWrapping.Wrap : DWrite.WordWrapping.NoWrap;

                TextLayout = new DWrite.TextLayout(
                    factory,
                    text ?? string.Empty,
                    format,
                    float.MaxValue,
                    float.MaxValue);
            }

            TextLayout.TextAlignment = textAlignment.ToDirect2D();
        }

        /// <summary>
        /// Gets or sets the constraint size.
        /// </summary>
        public Size Constraint
        {
            get
            {
                return new Size(TextLayout.MaxWidth, TextLayout.MaxHeight);
            }

            set
            {
                TextLayout.MaxWidth = (float)value.Width;
                TextLayout.MaxHeight = (float)value.Height;
            }
        }

        /// <summary>
        /// Gets the text layout.
        /// </summary>
        public DWrite.TextLayout TextLayout { get; }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            TextLayout.Dispose();
        }

        /// <summary>
        /// Gets the formatted text lines.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FormattedTextLine> GetLines()
        {
            var result = TextLayout.GetLineMetrics();
            return from line in result select new FormattedTextLine(line.Length, line.Height);
        }

        /// <summary>
        /// Checks whether the point hit the formatted text element.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public TextHitTestResult HitTestPoint(Point point)
        {
            SharpDX.Mathematics.Interop.RawBool isTrailingHit;
            SharpDX.Mathematics.Interop.RawBool isInside;

            var result = TextLayout.HitTestPoint(
                (float)point.X,
                (float)point.Y,
                out isTrailingHit,
                out isInside);

            return new TextHitTestResult
            {
                IsInside = isInside,
                TextPosition = result.TextPosition,
                IsTrailing = isTrailingHit,
            };
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        // TO DO: Comment...
        public Rect HitTestTextPosition(int index)
        {
            float x;
            float y;

            var result = TextLayout.HitTestTextPosition(
                index,
                false,
                out x,
                out y);

            return new Rect(result.Left, result.Top, result.Width, result.Height);
        }

        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        // TO DO: Comment...
        public IEnumerable<Rect> HitTestTextRange(int index, int length)
        {
            var result = TextLayout.HitTestTextRange(index, length, 0, 0);
            return result.Select(x => new Rect(x.Left, x.Top, x.Width, x.Height));
        }

        /// <summary>
        /// Measures the formatted text element.
        /// </summary>
        /// <returns></returns>
        public Size Measure()
        {
            var metrics = TextLayout.Metrics;
            var width = metrics.WidthIncludingTrailingWhitespace;

            if (float.IsNaN(width))
            {
                width = metrics.Width;
            }

            return new Size(width, TextLayout.Metrics.Height);
        }

        /// <summary>
        /// Set the brush used to draw the foreground part of the text element.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public void SetForegroundBrush(IBrush brush, int startIndex, int count)
        {
            TextLayout.SetDrawingEffect(
                new BrushWrapper(brush),
                new DWrite.TextRange(startIndex, count));
        }
    }
}
