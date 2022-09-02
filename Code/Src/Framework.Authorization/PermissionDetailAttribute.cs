using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Authorization
{
    public class PermissionDetailAttribute : Attribute
    {
        public string Description { get; private set; }
        public Permission ParentPermission { get; private set; }

        public PermissionDetailAttribute(string description, Enum parent = null)
        {
            Description = description;
            if(parent is Enum parentEnum)
                ParentPermission = parentEnum.GetPermission();
        }
    }
}
