namespace Framework.Authentication.Configuration;

public class TokenManagementConfiguration
{
    public string StsBaseUrl { get; set; }
    public string ClientId { get; set; }
    public string SecretKey { get; set; }
    public string Scope { get; set; }
}