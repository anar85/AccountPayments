namespace PaymentsApplication.Models.Request.Token
#nullable disable
{
    public class ResponseVerifyToken
    {
        public int TerminalId { get; set; }
        public string UserName { get; set; }
    }
}