using PaymentsApplication.Models.Request.PayAccountsRequest;

namespace PaymentsApplication.Interfaces.YurikServicesİnterfaces
{
	public interface IPayLegalAccountsService
	{
		Task<string> PayAccByVoen(PayAccountsRequestModel payAccountsRequest, int serviceId);
		Task<string> PayLegalAccByAccountNumber(string transactionId, decimal amount, int serviceId);
		Task<string> PayAccByVoenWithPrimechanie(PayAccountsWithReferenceRequestModel payAccountsRequest, int serviceId);
		Task<string> PayAccByAccountNumberWithPrimechanie(string transactionId, decimal amount, int serviceId, string primechanie);
	}
}
