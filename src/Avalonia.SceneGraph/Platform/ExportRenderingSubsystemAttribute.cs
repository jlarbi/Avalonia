using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="ExportRenderingSubsystemAttribute"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ExportRenderingSubsystemAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportRenderingSubsystemAttribute"/> class.
        /// </summary>
        /// <param name="requiredOS"></param>
        /// <param name="priority"></param>
        /// <param name="name"></param>
        /// <param name="initializationType"></param>
        /// <param name="initializationMethod"></param>
        public ExportRenderingSubsystemAttribute(OperatingSystemType requiredOS, int priority, string name, Type initializationType, string initializationMethod)
        {
            Name = name;
            InitializationType = initializationType;
            InitializationMethod = initializationMethod;
            RequiredOS = requiredOS;
            Priority = priority;
        }

        /// <summary>
        /// Gets the initialization method.
        /// </summary>
        public string InitializationMethod { get; private set; }

        /// <summary>
        /// Gets the initialization type.
        /// </summary>
        public Type InitializationType { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// Gets the operating system type.
        /// </summary>
        public OperatingSystemType RequiredOS { get; private set; }

        /// <summary>
        /// TODO: What is that for??
        /// </summary>
        public string RequiresWindowingSubsystem { get; set; }
    }
}
