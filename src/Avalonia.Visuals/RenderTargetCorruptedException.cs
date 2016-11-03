using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia
{
    /// <summary>
    /// Definition of the <see cref="RenderTargetCorruptedException"/> class.
    /// </summary>
    public class RenderTargetCorruptedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetCorruptedException"/> class.
        /// </summary>
        public RenderTargetCorruptedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetCorruptedException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public RenderTargetCorruptedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetCorruptedException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public RenderTargetCorruptedException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RenderTargetCorruptedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
