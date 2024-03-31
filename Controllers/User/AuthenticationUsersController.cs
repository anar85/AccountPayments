using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.Users;
using PaymentsApplication.Models.Request.Token;
using PaymentsApplication.Models.Request.User;
#nullable disable
namespace AccountPaymentsAPI.Controllers.User
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationUsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Authentication Terminal Users to get JWT access token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate(UserAuthenticationRequestModel request)
        {
            var tokenRequest = new GenerateTokenRequest
            {
                UserName = request.UserName,
                Password = request.Password
            };
            var token = await _userService.GetAccessToken(tokenRequest);

            if (token == null)
            {
                return Unauthorized("Token not Valid");
            }

            return Ok(token);
        }
    }
}
