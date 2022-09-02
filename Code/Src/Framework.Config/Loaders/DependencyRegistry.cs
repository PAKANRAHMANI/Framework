namespace Framework.Config.Loaders
{
    internal static class DependencyRegistry
    {
        public static IDependencyRegister Current { get; private set; }
        public static void SetCurrent(IDependencyRegister registry)
        {
            Current = registry;
        }
    }
}