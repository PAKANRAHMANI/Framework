namespace Framework.Authentication.Configuration;

public class AuthenticationConfig
{
    public string Authority { get; set; }
    public string Audience { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public int ClockSkew { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
}