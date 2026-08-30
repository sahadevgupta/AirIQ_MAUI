using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AirIQ.Extensions
{
    public static class EnumExtension
    {
        public static T GetValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
            typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null)!;
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null)!;
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());



            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];



            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }



            return value.ToString();
        }
    }
}