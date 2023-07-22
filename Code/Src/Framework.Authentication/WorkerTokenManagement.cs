using Framework.Authentication.Configuration;
using Framework.Core.Logging;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.Authentication;

public class WorkerTokenManagement : ITokenManagement
{
    private readonly TokenManagementConfiguration _tokenManagementConfiguration;
    private readonly HttpClient _client;
    private readonly ILogger _logger;
    public WorkerTokenManagement(TokenManagementConfiguration tokenManagementConfiguration, IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _tokenManagementConfiguration = tokenManagementConfiguration;
        _logger = logger;
        _client = httpClientFactory.CreateClient(Constants.ClientName);
    }

    public async Task<TokenResponse> GetCredentialsToken()
    {
        try
        {
            WriteMessage("going to get userToken from idp", "GetCredentialsToken");

            var discoveryDocument = await GetDiscoveryDocumentResponse();

            var tokenRequest = CreteClientCredentialsTokenRequest(discoveryDocument);

            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(tokenRequest);

            if (tokenResponse.IsError == false) return tokenResponse;

            WriteMessage($"can't get CredentialsToken(, message is:{tokenResponse.Error}, description:{tokenResponse.ErrorDescription}", "GetCredentialsToken");

            throw new Exception("can't get CredentialsToken");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private ClientCredentialsTokenRequest CreteClientCredentialsTokenRequest(DiscoveryDocumentResponse discoveryDocument)
    {
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = _tokenManagementConfiguration.ClientId,
            ClientSecret = _tokenManagementConfiguration.SecretKey,
            Scope = _tokenManagementConfiguration.Scope,
        };
        return tokenRequest;
    }

    private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentResponse()
    {
        var discoveryDocument = await _client.GetDiscoveryDocumentAsync();

        if (discoveryDocument.IsError == false) return discoveryDocument;

        WriteMessage($"can't get DiscoveryDocument, message is:{discoveryDocument.Error}", "GetDiscoveryDocumentResponse");

        throw new Exception("can't get DiscoveryDocument");

    }
    private void WriteMessage(string message, string methodName)
    {
        _logger.Write(
            "class: {className} | method: {methodName} | log-event: {logEvent},message: {@message}.",
            LogLevel.Information,
            nameof(TokenManagement), methodName, "Getting Token", message);
    }
}