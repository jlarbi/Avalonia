using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Platform
{
    /// <summary>
    /// Definition of the <see cref="ExportWindowingSubsystemAttribute"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ExportWindowingSubsystemAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportWindowingSubsystemAttribute"/> class.
        /// </summary>
        /// <param name="requiredRuntimePlatform"></param>
        /// <param name="priority"></param>
        /// <param name="name"></param>
        /// <param name="initializationType"></param>
        /// <param name="initializationMethod"></param>
        public ExportWindowingSubsystemAttribute(OperatingSystemType requiredRuntimePlatform, int priority, string name, Type initializationType, string initializationMethod)
        {
            Name = name;
            InitializationType = initializationType;
            InitializationMethod = initializationMethod;
            RequiredOS = requiredRuntimePlatform;
            Priority = priority;
        }

        /// <summary>
        /// Gets the initialization method name.
        /// </summary>
        public string InitializationMethod { get; private set; }

        /// <summary>
        /// Gets the initialization method provider type.
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
        /// Gets the required OS type.
        /// </summary>
        public OperatingSystemType RequiredOS { get; private set; }
    }
}
