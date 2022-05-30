using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MicroCore.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr =
                        Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                        return attr.Description;
                    return value.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value from string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="str">The string.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentException">T must be an enumerated type</exception>
        public static T GetValueFromString<T>(this Enum value, string str) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            Enum.TryParse(value.GetType(), str, out var result);
            return (T)result;
        }
    }
}
