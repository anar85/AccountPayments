using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces;

namespace AccountPaymentsAPI.Controllers
{
    #region Test etmek ucun
    [Route("api/[controller]")]
	[ApiController]
	public class PayWithCardNumberController : ControllerBase
	{
		private readonly IPayToCardNumberService _payToCard;

		public PayWithCardNumberController(IPayToCardNumberService payToCard)
		{
			_payToCard = payToCard;
		}
		/// <summary>
		/// Pay to customer account by Card number.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		[HttpPut]
		[NonAction]
		[Route("PayWithCardNumber")]
		[Authorize]
		public async Task<IActionResult> PayToCard(string transactionId, decimal amount)
		{
			return Ok(await _payToCard.PayCard(transactionId, amount));
		}
	}
    #endregion
}
