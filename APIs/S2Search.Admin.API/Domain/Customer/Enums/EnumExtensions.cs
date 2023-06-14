using System;
using System.ComponentModel;
using System.Reflection;

namespace Domain.Customer.Enums
{
    public static class EnumExtensions
    {
        /// <summary>
        /// If the Enum value has been decorated with the following: [Description("MyEnum")], this method will return the string value to allow output of friendly names. 
        /// Otherwise it will return null.
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
