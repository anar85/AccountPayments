namespace PaymentsApplication.Models.Request.PayAccountsRequest
{
	public class PayAccountsRequestModel
	{
		public string TransactionId { get; set; }
		public int Filialnumber { get; set; }
		public string Account { get; set; }
		public string Currency { get; set; }
		public decimal Amount { get; set; }
	}
	public class PayAccountsWithReferenceRequestModel
	{
		public string TransactionId { get; set; }
		public int Filialnumber { get; set; }
		public string Account { get; set; }
		public string Currency { get; set; }
		public decimal Amount { get; set; }
		public string Primechanie { get; set; }
	}
	public class PayRequestModel
	{
		public string TransactionId { get; set; }
		public decimal Amount { get; set; }
	}

}
