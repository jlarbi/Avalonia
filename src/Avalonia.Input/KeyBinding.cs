using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="KeyBinding"/> class.
    /// </summary>
    public class KeyBinding : AvaloniaObject
    {
        /// <summary>
        /// The command property.
        /// </summary>
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<KeyBinding, ICommand>("Command");

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get { return GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// The command parameter property.
        /// </summary>
        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<KeyBinding, object>("CommandParameter");

        /// <summary>
        /// Gets or sets the command parameter(s).
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// The gesture property.
        /// </summary>
        public static readonly StyledProperty<KeyGesture> GestureProperty =
            AvaloniaProperty.Register<KeyBinding, KeyGesture>("Gesture");

        /// <summary>
        /// Gets or sets the gesture.
        /// </summary>
        public KeyGesture Gesture
        {
            get { return GetValue(GestureProperty); }
            set { SetValue(GestureProperty, value); }
        }

        /// <summary>
        /// Attempts to handle the attached command.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        public void TryHandle(KeyEventArgs args)
        {
            if (Gesture?.Matches(args) == true)
            {
                args.Handled = true;
                if (Command?.CanExecute(CommandParameter) == true)
                    Command.Execute(CommandParameter);
            }
        }
    }
}
