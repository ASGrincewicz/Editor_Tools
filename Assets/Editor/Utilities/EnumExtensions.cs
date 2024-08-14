using System;
using System.Reflection;

namespace Editor.Utilities
{
    public static class EnumExtensions {
        public static string GetDescription(this Enum value) {
            FieldInfo field = value.GetType().GetField(value.ToString());
            object[] attributes = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description : value.ToString();
        }
    }
}