namespace PaymentsApplication.Models.Response.CardPayments
{
	public class CardHolderResponseModel
	{
		public string TransactionId { get; set; }
		public string MaskNameCardHolder { get; set; }
		public string Currency { get; set; }
		public string CardType { get; set; }
		public decimal FeePercent { get; set; }
	}
}
