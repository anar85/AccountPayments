using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces;
using PaymentsApplication.Models.Request.Card;

namespace AccountPaymentsAPI.Controllers.Payments.Fizik.Card_operations
{
	[Route("api/[controller]")]
	[ApiController]
	public class CardController : ControllerBase
	{
		private readonly ICardInfoService _cardInfoService;
		private readonly IPayToCardNumberService _payToCard;

		public CardController(ICardInfoService cardInfoService, IPayToCardNumberService payToCard)
		{
			_cardInfoService = cardInfoService;
			_payToCard = payToCard;
		}
		/// <summary>
		/// Get Information about Cardholder by Card Number.
		/// </summary>
		/// <param name="cardNumber"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getcardinfo")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetCard(string cardNumber)
		{
			var response = await _cardInfoService.GetCardHolderName(cardNumber, HttpContext);
			return Ok(response);
		}
		/// <summary>
		/// Pay to customer account by Card number.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayWithCardNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayToCard([FromBody] PayToCardRequest request)
		{
			return Ok(await _payToCard.PayCard(request.TransactionId, request.Amount));
		}
	}
}
