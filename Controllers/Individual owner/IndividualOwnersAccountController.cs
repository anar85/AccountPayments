using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.OwnerServicesInterfaces;
using PaymentsApplication.Interfaces.YurikServicesİnterfaces;
using PaymentsApplication.Models.Request.PayAccountsRequest;

namespace AccountPaymentsAPI.Controllers.Payments.Individual_owner
{
	[Route("api/[controller]")]
	[ApiController]
	public class IndividualOwnersAccountController : ControllerBase
	{
		private readonly IPayLegalAccountsService _payService;
		private readonly IGetOwnerAccountService _getOwnerAccountService;

		public IndividualOwnersAccountController(IPayLegalAccountsService payService, IGetOwnerAccountService getOwnerAccountService)
		{
			_payService = payService;
			_getOwnerAccountService = getOwnerAccountService;
		}
		/// <summary>
		/// Get Information about  Accounts of Individual Owner by VOEN.
		/// </summary>
		/// <param name="voen"></param>
		/// <param name="payerPincode"></param>
		/// <param name="payerBirthdate"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetOwnerAccountInfoByVoen")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetOwnerAccountInfoByVoen(string voen, string payerPincode, DateTime payerBirthdate)
		{
			return Ok(await _getOwnerAccountService.GetOwnerLicschInfoByVoen(voen, payerPincode, payerBirthdate, HttpContext));
		}
		//serviceId = 7 CARİ HESABA MƏDAXİL FƏRDİ SAHİBKAR(VÖEN)
		/// <summary>
		/// Pay to Individual Owner  Accounts  by Voen.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayOwnerAccountByVoen")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayAcccountByVoen([FromBody] PayAccountsRequestModel request)
		{
			var result = await _payService.PayAccByVoen(request, 7);
			return Ok(result);
		}
		/// <summary>
		/// Get Information about Accounts of Individual Owner by Account number.
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="payerPincode"></param>
		/// <param name="payerBirthdate"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetOwnerAccountInfoByAccountNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetOwnerAcccountInfoByAccountNumber(string accountNumber, string payerPincode, DateTime payerBirthdate)
		{
			var result = await _getOwnerAccountService.GetOwnerLicschInfoByAccountNumber(accountNumber, payerPincode, payerBirthdate, HttpContext);
			return Ok(result);
		}
		//serviceId = 8 CARİ HESABA MƏDAXİL FƏRDİ SAHİBKAR(HESAB)
		/// <summary>
		/// Pay to Individual Owner Persons Accounts  by Account Number.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayOwnerAccountByAccountNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayOwnerAcccountByAccount(string transactionId, decimal amount)
		{
			var result = await _payService.PayLegalAccByAccountNumber(transactionId, amount, 8);
			return Ok(result);
		}
	}

}
