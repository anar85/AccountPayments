#nullable disable
namespace PaymentsApplication.Models.Request.User
{
    public class UserAuthenticationRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
