using System;
using System.Linq;
using System.Reflection;

namespace Framework.DataAccess.Mongo
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
