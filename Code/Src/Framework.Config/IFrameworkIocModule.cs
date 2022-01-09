namespace Framework.Config
{
    public interface IFrameworkIocModule : IFrameworkModule
    {
        IDependencyRegister CreateServiceRegistry();
    }
}
