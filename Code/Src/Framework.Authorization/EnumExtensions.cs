using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Authorization
{
    internal static class EnumExtensions
    {
        public static Permission GetPermission(this Enum value)
        {
            var name = value.ToString().Trim();
            var detailAttribute = value.GetAttribute<PermissionDetailAttribute>();
            var description = detailAttribute.Description?.Trim();
            var parent = detailAttribute.ParentPermission;
            return new Permission(name, description, parent);
        }
        private static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            if (attributes.Any())
                return (T)attributes[0];
            return null;
        }
    }
}
