using Microsoft.AspNetCore.Http;
using PaymentsApplication.Models.Configs;

namespace PaymentsApplication.Interfaces.AppServicesInterface
{
	public interface IAppServices
	{
		string GenerateTransactionId(string serviceId);
		string GetCardType(string cardNumber);
		Task<ReadContextResponse> GetInfoFromContext(HttpContext context);
		Task<string> GetOperationStatus(string transactionId, HttpContext context);
	}
}
