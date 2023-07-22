using System.Threading.Tasks;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace Framework.Authentication;

public interface ITokenManagement
{
    public Task<TokenResponse> GetCredentialsToken();
}