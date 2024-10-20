using Microsoft.AspNetCore.Http;
using PaymentsApplication.Models.Response.CardPayments;

namespace PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces
{
	public interface ICardInfoService
	{
		Task<CardHolderResponseModel> GetCardHolderName(string cardnumber, HttpContext context);
	}
}
