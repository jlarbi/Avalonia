
using System.Runtime.CompilerServices;

namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="Design"/> class.
    /// </summary>
    public static class Design
    {
        /// <summary>
        /// Gets the flag indicating whether the application is in designer mode or not.
        /// </summary>
        public static bool IsDesignMode { get; internal set; }

        /// <summary>
        /// The height property
        /// </summary>
        public static readonly AttachedProperty<double> HeightProperty = AvaloniaProperty
            .RegisterAttached<Control, double>("Height", typeof (Design));

        /// <summary>
        /// Sets a new height value to the given control
        /// </summary>
        public static void SetHeight(Control control, double value)
        {
            control.SetValue(HeightProperty, value);
        }

        /// <summary>
        /// Gets a new height value to the given control
        /// </summary>
        public static double GetHeight(Control control)
        {
            return control.GetValue(HeightProperty);
        }

        /// <summary>
        /// The width property
        /// </summary>
        public static readonly AttachedProperty<double> WidthProperty = AvaloniaProperty
            .RegisterAttached<Control, double>("Width", typeof(Design));

        /// <summary>
        /// Sets a new width value to the given control
        /// </summary>
        public static void SetWidth(Control control, double value)
        {
            control.SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Gets a new width value to the given control
        /// </summary>
        public static double GetWidth(Control control)
        {
            return control.GetValue(WidthProperty);
        }

        /// <summary>
        /// The data context property
        /// </summary>
        public static readonly AttachedProperty<object> DataContextProperty = AvaloniaProperty
            .RegisterAttached<Control, object>("DataContext", typeof (Design));

        /// <summary>
        /// Sets a new data context value to the given control
        /// </summary>
        public static void SetDataContext(Control control, object value)
        {
            control.SetValue(DataContextProperty, value);
        }

        /// <summary>
        /// Gets a new data context value to the given control
        /// </summary>
        public static object GetDataContext(Control control)
        {
            return control.GetValue(DataContextProperty);
        }

        /// <summary>
        /// Stores the substitutes.
        /// </summary>
        static readonly ConditionalWeakTable<object, Control> Substitutes = new ConditionalWeakTable<object, Control>();

        /// <summary>
        /// The preview width property
        /// </summary>
        public static readonly AttachedProperty<Control> PreviewWithProperty = AvaloniaProperty
            .RegisterAttached<AvaloniaObject, Control>("PreviewWith", typeof (Design));

        /// <summary>
        /// Sets a new preview width value to the given control
        /// </summary>
        public static void SetPreviewWith(object target, Control control)
        {
            Substitutes.Remove(target);
            Substitutes.Add(target, control);
        }

        /// <summary>
        /// Gets a new preview width value to the given control
        /// </summary>
        public static Control GetPreviewWith(object target)
        {
            Control rv;
            Substitutes.TryGetValue(target, out rv);
            return rv;
        }

        /// <summary>
        /// Applies changed properties to the given target control using the provided source one.
        /// </summary>
        /// <param name="target">The target control.</param>
        /// <param name="source">The source control.</param>
        internal static void ApplyDesignerProperties(Control target, Control source)
        {
            if (source.IsSet(WidthProperty))
                target.Width = source.GetValue(WidthProperty);
            if (source.IsSet(HeightProperty))
                target.Height = source.GetValue(HeightProperty);
            if (source.IsSet(DataContextProperty))
                target.DataContext = source.GetValue(DataContextProperty);
        }
    }
}
