using Microsoft.AspNetCore.Http;
using PaymentsApplication.Models.Response.AccountPayments;

namespace PaymentsApplication.Interfaces.YurikServicesİnterfaces
{
	public interface IGetLegalAccountInfoService
	{
		Task<LicschHolderInfoResponseModel> GetLicschInfoByVoen(string voen, string payerPincode, DateTime payerBirthDate, HttpContext context);
		Task<LicschHolderInfoResponseModel> GetLicschInfoByAccountNumber(string accountNumber, string payerPincode, DateTime payerBirthDate, HttpContext context);
	}
}
