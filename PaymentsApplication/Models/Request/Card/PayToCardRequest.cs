namespace PaymentsApplication.Models.Request.Card
{
	public class PayToCardRequest
	{
		public string TransactionId { get; set; }
		public decimal Amount { get; set; }
	}
}
