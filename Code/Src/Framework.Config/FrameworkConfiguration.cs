namespace Framework.Config;

public class FrameworkConfiguration
{
    public FrameworkConfiguration()
    {
        EnableLogInCommand=true;
        EnableLogInRequest = true;
        UseUnitOfWork = true;
    }
    public bool EnableLogInCommand { get; set; }
    public bool UseUnitOfWork { get; set; }
    public bool EnableLogInRequest { get; set; }
}