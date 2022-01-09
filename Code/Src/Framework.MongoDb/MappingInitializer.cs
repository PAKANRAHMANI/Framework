using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework.MongoDb
{
    public static class MappingInitializer
    {
        public static void Initial(Assembly assembly)
        {
            if (assembly == null) return;
            var mapperTypes = assembly.GetTypes().Where(a => typeof(IBsonMapping).IsAssignableFrom(a)).ToList();
            foreach (var instanceOfMapper in mapperTypes.Select(mapper => (IBsonMapping)Activator.CreateInstance(mapper)))
            {
                instanceOfMapper.Register();
            }
        }
    }
}
