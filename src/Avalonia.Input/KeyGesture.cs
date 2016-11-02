using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.Input
{
    /// <summary>
    /// Definition of the <see cref="KeyGesture"/> class.
    /// </summary>
    public sealed class KeyGesture : IEquatable<KeyGesture>
    {
        /// <summary>
        /// Checks whether this gesture key is equal to another.
        /// </summary>
        /// <param name="other">The other gesture key.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public bool Equals(KeyGesture other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key == other.Key && Modifiers == other.Modifiers;
        }

        /// <summary>
        /// Checks whether this gesture key is equal to another.
        /// </summary>
        /// <param name="obj">The other gesture key.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is KeyGesture && Equals((KeyGesture) obj);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Key*397) ^ (int) Modifiers;
            }
        }

        /// <summary>
        /// Checks whether this gesture key is equal to another.
        /// </summary>
        /// <param name="left">The first gesture key.</param>
        /// <param name="right">The second gesture key.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public static bool operator ==(KeyGesture left, KeyGesture right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Checks whether this gesture key is different to another.
        /// </summary>
        /// <param name="left">The first gesture key.</param>
        /// <param name="right">The second gesture key.</param>
        /// <returns>True if different, false otherwise.</returns>
        public static bool operator !=(KeyGesture left, KeyGesture right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Gets or sets the keyboard key.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets the key modifiers.
        /// </summary>
        public InputModifiers Modifiers { get; set; }

        
        static readonly Dictionary<string, Key> KeySynonims = new Dictionary<string, Key>
        {
            {"+", Key.OemPlus },
            {"-", Key.OemMinus},
            {".", Key.OemPeriod }
        };

        //TODO: Move that to external key parser
        static Key ParseKey(string key)
        {
            Key rv;
            if (KeySynonims.TryGetValue(key.ToLower(), out rv))
                return rv;
            return (Key)Enum.Parse(typeof (Key), key, true);
        }

        static InputModifiers ParseModifier(string modifier)
        {
            if (modifier.Equals("ctrl", StringComparison.OrdinalIgnoreCase))
                return InputModifiers.Control;
            return (InputModifiers) Enum.Parse(typeof (InputModifiers), modifier, true);
        }

        /// <summary>
        /// Parses the given string into a Gesture key.
        /// </summary>
        /// <param name="gesture">The gesture key as string.</param>
        /// <returns></returns>
        public static KeyGesture Parse(string gesture)
        {
            //string.Split can't be used here because "Ctrl++" is a perfectly valid key gesture

            var parts = new List<string>();

            var cstart = 0;
            for (var c = 0; c <= gesture.Length; c++)
            {
                var ch = c == gesture.Length ? '\0' : gesture[c];
                if (c == gesture.Length || (ch == '+' && cstart != c))
                {
                    parts.Add(gesture.Substring(cstart, c - cstart));
                    cstart = c + 1;
                }
            }
            for (var c = 0; c < parts.Count; c++)
                parts[c] = parts[c].Trim();

            var rv = new KeyGesture();

            for (var c = 0; c < parts.Count; c++)
            {
                if (c == parts.Count - 1)
                    rv.Key = ParseKey(parts[c]);
                else
                    rv.Modifiers |= ParseModifier(parts[c]);
            }
            return rv;
        }

        /// <summary>
        /// Turns the gesture key into a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var parts = new List<string>();
            foreach (var flag in Enum.GetValues(typeof (InputModifiers)).Cast<InputModifiers>())
            {
                if (Modifiers.HasFlag(flag) && flag != InputModifiers.None)
                    parts.Add(flag.ToString());
            }
            parts.Add(Key.ToString());
            return string.Join(" + ", parts);
        }

        /// <summary>
        /// Checks whether the given key event arguments matches this gesture key.
        /// </summary>
        /// <param name="keyEvent"></param>
        /// <returns>True if matches, false otherwise.</returns>
        public bool Matches(KeyEventArgs keyEvent) => keyEvent.Key == Key && keyEvent.Modifiers == Modifiers;
    }
}
