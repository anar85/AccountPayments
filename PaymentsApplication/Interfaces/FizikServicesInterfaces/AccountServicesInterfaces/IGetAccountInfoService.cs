using Microsoft.AspNetCore.Http;
using PaymentsApplication.Models.Response.AccountPayments;

namespace PaymentsApplication.Interfaces.FizikServicesInterfaces.AccountServicesInterfaces
{
	public interface IGetAccountInfoService
	{
		Task<LicschHolderInfoResponseModel> GetLicschInfoByFin(string pincode, HttpContext context);
		Task<LicschHolderInfoResponseModel> GetLicschInfoByAccountNumber(string accountNumber, HttpContext context);
	}
}
