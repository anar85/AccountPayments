using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentsApplication.Interfaces.Token;
using PaymentsApplication.Models.Configs;
using PaymentsApplication.Models.Request.Token;
using PaymentsApplication.Models.Response.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaymentsApplication.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _appSettings;


        public TokenService(IOptions<TokenSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<TokenResponse> GenerateToken(GenerateTokenRequest request)
        {

            var ttlToken = _appSettings.TtlAccessToken;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.UserName),
                     new Claim("TerminalId", request.TerminalId.ToString() ),
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(ttlToken)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor))
            };
        }
       
        public ResponseVerifyToken Verify(string token)
        {
            try
            {
                var Key = Encoding.ASCII.GetBytes(_appSettings.Secret); 

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // on production make it true
                    ValidateAudience = false, // on production make it true
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token.Replace("Bearer ", "").Replace("bearer ", ""), tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return  new ResponseVerifyToken
                {
                    UserName = principal.Identity.Name.ToString(),
                    TerminalId = Convert.ToInt32(principal.FindFirst(claim => claim.Type == "TerminalId").Value)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
