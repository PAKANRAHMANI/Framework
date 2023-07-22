using Framework.Authentication.Configuration;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.Core.Logging;
using static IdentityModel.OidcConstants;

namespace Framework.Authentication
{
    public class TokenManagement : ITokenManagement
    {
        private readonly TokenManagementConfiguration _tokenManagementConfiguration;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        public TokenManagement(TokenManagementConfiguration tokenManagementConfiguration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            _tokenManagementConfiguration = tokenManagementConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _client = httpClientFactory.CreateClient(Constants.ClientName);
        }

        public async Task<IdentityModel.Client.TokenResponse> ExchangeToken()
        {
            try
            {
                var userToken = await GetUserToken();

                var actorToken = await GetCredentialsToken();

                var tokenResponse = await ExchangeForDelegation(userToken, actorToken);

                if (tokenResponse.IsError == false) return tokenResponse;

                WriteMessage($"can't exchange, message is:{tokenResponse.Error}, description:{tokenResponse.ErrorDescription}", "ExchangeToken");

                throw new Exception("can't exchange Token");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IdentityModel.Client.TokenResponse> GetCredentialsToken()
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

        public async Task<string> GetUserToken()
        {
            var header = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];

            var token = header.ToString().Split(" ")[1];

            return await Task.FromResult(token);
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

        private async Task<IdentityModel.Client.TokenResponse> ExchangeForDelegation(string subjectToken, IdentityModel.Client.TokenResponse actorToken)
        {
            var disco = await GetDiscoveryDocumentResponse();

            var tokenRequest = new TokenExchangeTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _tokenManagementConfiguration.ClientId,
                ClientSecret = _tokenManagementConfiguration.SecretKey,
                SubjectToken = subjectToken,
                SubjectTokenType = TokenTypeIdentifiers.AccessToken,
                Scope = _tokenManagementConfiguration.Scope,
                ActorToken = actorToken.AccessToken,
                ActorTokenType = TokenTypeIdentifiers.AccessToken,
            };

            var tokenResponse = await _client.RequestTokenExchangeTokenAsync(tokenRequest);

            return tokenResponse;
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
}