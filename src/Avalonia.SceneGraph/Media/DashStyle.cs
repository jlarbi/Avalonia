namespace Avalonia.Media
{
    using Avalonia.Animation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Definition of the <see cref="DashStyle"/> class.
    /// </summary>
    public class DashStyle : Animatable
    {
        /// <summary>
        /// Stores the simple dashed dash style.
        /// </summary>
        private static DashStyle dash;

        /// <summary>
        /// Gets the simple dashed dash style which is 2 on, 2 off
        /// </summary>
        public static DashStyle Dash
        {
            get
            {
                if (dashDotDot == null)
                {
                    dash = new DashStyle(new double[] { 2, 2 }, 1);
                }

                return dash;
            }
        }

        /// <summary>
        /// Stores the dotted dash style.
        /// </summary>
        private static DashStyle dot;

        /// <summary>
        /// Gets the dotted dash style. which is 0 on, 2 off
        /// </summary>
        public static DashStyle Dot
        {
            get { return dot ?? (dot = new DashStyle(new double[] {0, 2}, 0)); }
        }

        /// <summary>
        /// Stores the dashed/dotted dash style.
        /// </summary>
        private static DashStyle dashDot;

        /// <summary>
        /// Gets the dashed/dotted dash style which is 2 on, 2 off, 0 on, 2 off
        /// </summary>
        public static DashStyle DashDot
        {
            get
            {
                if (dashDot == null)
                {
                    dashDot = new DashStyle(new double[] { 2, 2, 0, 2 }, 1);
                }

                return dashDot;
            }
        }

        /// <summary>
        /// Stores the dashed/dotted/dotted dash style.
        /// </summary>
        private static DashStyle dashDotDot;

        /// <summary>
        /// Gets the dashed/dotted/dotted dash style which is 2 on, 2 off, 0 on, 2 off, 0 on, 2 off
        /// </summary>
        public static DashStyle DashDotDot
        {
            get
            {
                if (dashDotDot == null)
                {
                    dashDotDot = new DashStyle(new double[] { 2, 2, 0, 2, 0, 2 }, 1);
                }

                return dashDotDot;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DashStyle"/> class.
        /// </summary>
        /// <param name="pDashes">The dashes template.</param>
        /// <param name="pOffset">The dash style offset.</param>
        public DashStyle(IReadOnlyList<double> pDashes = null, double pOffset = 0.0)
        {
            this.Dashes = pDashes;
            this.Offset = pOffset;
        }

        /// <summary>
        /// Gets and sets the length of alternating dashes and gaps.
        /// </summary>
        public IReadOnlyList<double> Dashes { get; }

        /// <summary>
        /// Gets the dash style offset.
        /// </summary>
        public double Offset { get; }
    }
}
