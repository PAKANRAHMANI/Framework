using System;

namespace Framework.Config.Loaders
{
    public class FrameworkModuleBuilder : IIocModuleBuilder, IModuleBuilder
    {
        private FrameworkModuleBuilder() { }
        public static IIocModuleBuilder Setup()
        {
            return new FrameworkModuleBuilder();
        }
        public IModuleBuilder WithModule(IFrameworkModule module)
        {
            FrameworkModuleRegistry.Install(module);
            return this;
        }

        public IModuleBuilder WithModule<T>() where T : IFrameworkModule, new()
        {
            FrameworkModuleRegistry.Install<T>();
            return this;
        }

        public IModuleBuilder WithIocModule(IFrameworkIocModule module, Action<FrameworkConfiguration> frameworkConfiguration = null)
        {
            var configuration = new FrameworkConfiguration();

            if (frameworkConfiguration is not null)
                frameworkConfiguration.Invoke(configuration);

            FrameworkModuleRegistry.Install(module);

            var coreModule = new CoreModule(configuration);

            FrameworkModuleRegistry.Install(coreModule);

            return this;
        }
    }
}
