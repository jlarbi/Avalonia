// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Media;
using Avalonia.Platform;
using SharpDX.Direct2D1;
using SweepDirection = SharpDX.Direct2D1.SweepDirection;
using D2D = SharpDX.Direct2D1;
using Avalonia.Logging;
using System;

namespace Avalonia.Direct2D1.Media
{
    /// <summary>
    /// Definition of the <see cref="StreamGeometryContextImpl"/> class.
    /// </summary>
    public class StreamGeometryContextImpl : IStreamGeometryContextImpl
    {
        private readonly GeometrySink _sink;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamGeometryContextImpl"/> class.
        /// </summary>
        /// <param name="sink"></param>
        public StreamGeometryContextImpl(GeometrySink sink)
        {
            _sink = sink;
        }

        /// <summary>
        /// Creates a new arc to the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <param name="rotationAngle"></param>
        /// <param name="isLargeArc"></param>
        /// <param name="sweepDirection"></param>
        public void ArcTo(
            Point point,
            Size size,
            double rotationAngle,
            bool isLargeArc,
            Avalonia.Media.SweepDirection sweepDirection)
        {
            _sink.AddArc(new D2D.ArcSegment
            {
                Point = point.ToSharpDX(),
                Size = size.ToSharpDX(),
                RotationAngle = (float)rotationAngle,
                ArcSize = isLargeArc ? ArcSize.Large : ArcSize.Small,
                SweepDirection = (SweepDirection)sweepDirection,
            });
        }

        /// <summary>
        /// Starts a new figure at the given point.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="isFilled"></param>
        public void BeginFigure(Point startPoint, bool isFilled)
        {
            _sink.BeginFigure(startPoint.ToSharpDX(), isFilled ? FigureBegin.Filled : FigureBegin.Hollow);
        }

        /// <summary>
        /// Creates a cubic bezier segment from the three given points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        public void CubicBezierTo(Point point1, Point point2, Point point3)
        {
            _sink.AddBezier(new D2D.BezierSegment
            {
                Point1 = point1.ToSharpDX(),
                Point2 = point2.ToSharpDX(),
                Point3 = point3.ToSharpDX(),
            });
        }

        /// <summary>
        /// Creates a quadratic bezier segment from two points.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="dest"></param>
        public void QuadraticBezierTo(Point control, Point dest)
        {
            _sink.AddQuadraticBezier(new D2D.QuadraticBezierSegment
            {
                Point1 = control.ToSharpDX(),
                Point2 = dest.ToSharpDX()
            });
        }

        /// <summary>
        /// Creates a new line from the current position to the given one.
        /// </summary>
        /// <param name="point"></param>
        public void LineTo(Point point)
        {
            _sink.AddLine(point.ToSharpDX());
        }

        /// <summary>
        /// Ends the current figure.
        /// </summary>
        /// <param name="isClosed"></param>
        public void EndFigure(bool isClosed)
        {
            _sink.EndFigure(isClosed ? FigureEnd.Closed : FigureEnd.Open);
        }

        /// <summary>
        /// Sets the mode indicating how to fill the drawn shape.
        /// </summary>
        /// <param name="fillRule"></param>
        public void SetFillRule(FillRule fillRule)
        {
            _sink.SetFillMode(fillRule == FillRule.EvenOdd ? FillMode.Alternate : FillMode.Winding);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            // Put a catch around sink.Close as it may throw if there were an error e.g. parsing a path.
            try
            {
                _sink.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(
                    LogArea.Visual,
                    this,
                    "GeometrySink.Close exception: {Exception}",
                    ex);
            }

            _sink.Dispose();
        }
    }
}
