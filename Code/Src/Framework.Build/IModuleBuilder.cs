using Framework.Config;

namespace Framework.Build
{
    public interface IModuleBuilder
    {
        IModuleBuilder WithModule(IFrameworkModule module);
        IModuleBuilder WithModule<T>() where T : IFrameworkModule, new();
    }
}
