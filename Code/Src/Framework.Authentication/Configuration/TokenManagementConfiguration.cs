namespace Framework.Authentication.Configuration;

public class TokenManagementConfiguration
{
    public string StsBaseUrl { get; init; }
    public string ClientId { get; init; }
    public string SecretKey { get; init; }
    public string Scope { get; init; }
}