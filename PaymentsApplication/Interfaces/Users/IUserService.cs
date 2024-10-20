using PaymentsApplication.Models.Request.Token;
using PaymentsApplication.Models.Response.Token;

namespace PaymentsApplication.Interfaces.Users
{
    public interface IUserService
    {
        Task<TokenResponse> GetAccessToken(GenerateTokenRequest tokenRequest);
    }
}
