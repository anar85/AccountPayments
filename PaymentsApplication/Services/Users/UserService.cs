using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.Token;
using PaymentsApplication.Interfaces.Users;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Request.Token;
using PaymentsApplication.Models.Response.Token;
using PaymentsDataLayer.Interface.Users;
#nullable disable
namespace PaymentsApplication.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<TokenResponse> GetAccessToken(GenerateTokenRequest tokenRequest)
        {
            _logger.LogInformation("Call GetAccessToken method = > " + JsonConvert.SerializeObject(tokenRequest));
            var item = _userRepository.CheckUser(tokenRequest.UserName);
            _logger.LogInformation("items from CheckUser = > " + JsonConvert.SerializeObject(item));
            if (item == null )
                throw new BadRequestException(ExceptionCodes.InvalidCredentials, "Username or password incorrect!");
            var passswordCheck = BCrypt.Net.BCrypt.Verify(tokenRequest.Password, item.PASSWORD);
            _logger.LogInformation("Check password equals passswordCheck = > " + passswordCheck);
            if (!passswordCheck)
                throw new BadRequestException(ExceptionCodes.InvalidCredentials, "Username or password incorrect!");
            tokenRequest.TerminalId = item.TERMINAL_ID;
            var token = await _tokenService.GenerateToken(tokenRequest);
            _logger.LogInformation("Get token from GenerateToken method  = > " + token + "\n------------------------------");
            return token;

        }
    }
}
