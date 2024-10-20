namespace PaymentsApplication.Models.Response.AccountPayments
{
	public class LicschHolderInfoResponseModel
	{
		public LicschHolderInfoResponseModel()
		{
			this.AccountsInfo = new List<HoldersLicschsInfo>();
		}
		public string TransactionId { get; set; }
		public string FullName { get; set; }
		public string FinCode { get; set; }
		public string Passport { get; set; }
		public decimal FeePercent { get; set; } = 1.18M;
		public List<HoldersLicschsInfo> AccountsInfo { get; set; }
	}
	public class HoldersLicschsInfo
	{
		public string Licsch { get; set; }
		public string Currency { get; set; }
		public int? FilialNumber { get; set; }
		public string FilialName { get; set; }
	}
}
