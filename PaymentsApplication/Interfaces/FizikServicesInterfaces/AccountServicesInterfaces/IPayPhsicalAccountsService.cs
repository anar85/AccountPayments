using PaymentsApplication.Models.Request.PayAccountsRequest;

namespace PaymentsApplication.Interfaces.FizikServicesInterfaces.AccountServicesInterfaces
{
	public interface IPayPhsicalAccountsService
	{
		Task<string> PayAccByPin(PayAccountsRequestModel payAccountsRequest);
		Task<string> PayAccByAccount(PayRequestModel request);
	}
}
