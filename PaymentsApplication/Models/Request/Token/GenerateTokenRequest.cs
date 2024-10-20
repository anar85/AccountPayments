namespace PaymentsApplication.Models.Request.Token
#nullable disable
{
    public class GenerateTokenRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TerminalId { get; set; }
    }
}
