using PaymentsApplication.Models.Request.Token;
using PaymentsApplication.Models.Response.Token;

namespace PaymentsApplication.Interfaces.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateToken(GenerateTokenRequest request);
        ResponseVerifyToken Verify(string token);
    }
}
