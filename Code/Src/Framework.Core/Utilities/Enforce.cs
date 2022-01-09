using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Utilities
{
    public static class Enforce
    {
        public static class That
        {
            public static void CollectionHasBeenInitialized<T>(ref List<T> list)
            {
                list ??= new List<T>();
            }
        }
    }
}
