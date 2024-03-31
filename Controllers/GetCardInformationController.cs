using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces;

namespace AccountPaymentsAPI.Controllers
{
    #region Test etmek ucun 
    [Route("api/[controller]")]
	[ApiController]
	public class GetCardInformationController : ControllerBase
	{
		private readonly ICardInfoService _cardInfoService;

		public GetCardInformationController(ICardInfoService cardInfoService)
		{
			_cardInfoService = cardInfoService;
		}
		/// <summary>
		/// Get Information about Cardholder by Card Number.
		/// </summary>
		/// <param name="cardNumber"></param>
		/// <returns></returns>
		[HttpGet]
		[NonAction]
		[Route("getcardinfo")]
		[Authorize]
		public async Task<IActionResult> GetCard(string cardNumber)
		{
			var response = await _cardInfoService.GetCardHolderName(cardNumber, HttpContext);
			return Ok(response);
		}
	}
    #endregion
}
