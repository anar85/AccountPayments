using Microsoft.AspNetCore.Http;
using PaymentsApplication.Models.Response.AccountPayments;

namespace PaymentsApplication.Interfaces.OwnerServicesInterfaces
{
	public interface IGetOwnerAccountService
	{
		Task<LicschHolderInfoResponseModel> GetOwnerLicschInfoByAccountNumber(string accountNumber, string payerPincode, DateTime payerBirthDate, HttpContext context);
		Task<LicschHolderInfoResponseModel> GetOwnerLicschInfoByVoen(string voen, string payerPincode, DateTime payerBirthDate, HttpContext context);
	}
}
