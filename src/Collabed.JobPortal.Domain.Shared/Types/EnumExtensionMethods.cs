using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Collabed.JobPortal.Types
{
    public static class EnumExtensionMethods
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var attribute = enumType.GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>();
            var ignoredAttribute = enumType.GetMember(enumValue.ToString())
               .First()
               .GetCustomAttribute<IgnoreDataMemberAttribute>();

            if (ignoredAttribute != null)
            {
                return string.Empty;
            }

            if (attribute != null)
            {
                return attribute.Name;
            }

            return Enum.GetName(enumType, enumValue);
        }

        public static string GetName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var attribute = enumType.GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>();
            if (attribute != null)
            {
                return attribute.Name;
            }
            return Enum.GetName(enumType, enumValue);
        }
    }
}
