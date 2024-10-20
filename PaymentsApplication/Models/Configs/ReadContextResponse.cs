namespace PaymentsApplication.Models.Configs
{
	public class ReadContextResponse
	{
		public string Token { get; set; }
		public string Ip { get; set; }
		public int TerminalId { get; set; }
		public string UserName { get; set; }
	}
}
