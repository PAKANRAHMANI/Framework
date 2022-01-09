using System;
using System.Collections.Generic;

namespace Framework.Authorization
{
    public class Permission
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public Permission ParentPermission { get; private set; }

        public Permission(string name, string displayName, Permission parent = null)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.ParentPermission = parent;
        }
    }
}
