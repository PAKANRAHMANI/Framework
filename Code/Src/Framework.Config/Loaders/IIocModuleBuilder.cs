using System;

namespace Framework.Config.Loaders
{
    public interface IIocModuleBuilder
    {
        IModuleBuilder WithIocModule(IFrameworkIocModule module, Action<FrameworkConfiguration> frameworkConfiguration = null);
    }
}