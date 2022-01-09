using Framework.Config;

namespace Framework.Build
{
    public interface IIocModuleBuilder
    {
        IModuleBuilder WithIocModule(IFrameworkIocModule module);
    }
}