using System;
using System.ComponentModel.DataAnnotations;

namespace Collabed.JobPortal.Attributes;

public static class DisplayAttributeRetriever
{
    public static T GetValueFromName<T>(this string name) where T : Enum
    {
        var type = typeof(T);

        foreach (var field in type.GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
            {
                if (attribute.Name.Equals(name))
                {
                    return (T)field.GetValue(null);
                }
            }

            if (field.Name.Equals(name))
            {
                return (T)field.GetValue(null);
            }
        }

        throw new ArgumentOutOfRangeException(nameof(name));
    }
}