using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.YurikServicesİnterfaces;
using PaymentsApplication.Models.Request.PayAccountsRequest;

namespace AccountPaymentsAPI.Controllers.Yurik
{
	[Route("api/[controller]")]
	[ApiController]

	public class LegalPersonsAccountController : ControllerBase
	{
		private readonly IGetLegalAccountInfoService _getLegalAccountInfoService;
		private readonly IPayLegalAccountsService _payService;

		public LegalPersonsAccountController(IGetLegalAccountInfoService getLegalAccountInfoService, IPayLegalAccountsService payService)
		{
			_getLegalAccountInfoService = getLegalAccountInfoService;
			_payService = payService;
		}

		/// <summary>
		/// Get Information about  Accounts of Legal person by VOEN.
		/// </summary>
		/// <param name="voen"></param>
		/// <param name="payerPincode"></param>
		/// <param name="payerBirthdate"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetLegalAccountInfoByVoen")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetAccountInfoByVoen(string voen, string payerPincode, DateTime payerBirthdate)
		{
			return Ok(await _getLegalAccountInfoService.GetLicschInfoByVoen(voen, payerPincode, payerBirthdate, HttpContext));
		}
		//serviceId = 3 HESABIN MÖHKƏMLƏNDİRİLMƏSİ HÜQUQİ ŞƏXS(VÖEN)
		/// <summary>
		/// Pay to Legal Persons Accounts  by Voen.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayLegalPersonsAccountByVoen")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayAcccountByVoen([FromBody] PayAccountsRequestModel request)
		{
			var result = await _payService.PayAccByVoen(request, 3);
			return Ok(result);
		}
		/// <summary>
		/// Get Information about Accounts of Legal person by Account number.
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="payerPincode"></param>
		/// <param name="payerBirthdate"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetLegalAccountInfoByAccountNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetAcccountInfoByAccountNumber(string accountNumber, string payerPincode, DateTime payerBirthdate)
		{
			var result = await _getLegalAccountInfoService.GetLicschInfoByAccountNumber(accountNumber, payerPincode, payerBirthdate, HttpContext);
			return Ok(result);
		}
		//serviceId = 4 HESABIN MÖHKƏMLƏNDİRİLMƏSİ HÜQUQİ ŞƏXS(HESAB)
		/// <summary>
		/// Pay to Legal Persons Accounts  by Account Number.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayLegalPersonsAccountByAccountNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayLegalAcccountByAccount(string transactionId, decimal amount)
		{
			var result = await _payService.PayLegalAccByAccountNumber(transactionId, amount, 4);
			return Ok(result);
		}
		//serviceId = 5 TƏSİSÇİ TƏRƏFİNDƏN YARDIM HÜQUQİ ŞƏXS(VÖEN)
		/// <summary>
		/// Pay to Legal Persons Accounts  by Voen, Help from the founder.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayLegalPersonsAccountByVoenFounderHelp")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayAcccountByVoenFounder([FromBody] PayAccountsRequestModel request)
		{
			var result = await _payService.PayAccByVoen(request, 5);
			return Ok(result);
		}
		/// <summary>
		/// Pay to Legal Persons Accounts  by Account Number, Help from the founder.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayLegalPersonsAccountByAccountNumberFounderHelp")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayLegalAcccountByAccountFounder(string transactionId, decimal amount)
		{
			var result = await _payService.PayLegalAccByAccountNumber(transactionId, amount, 5);
			return Ok(result);
		}
		//serviceId = 6 DİGƏR DAXİLOLMALAR HÜQUQİ ŞƏXS
		/// <summary>
		/// Pay to Legal Persons Accounts  by Voen, Other Incomes.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayLegalPersonsAccountByVoenOtherIncome")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayAcccountByVoenOtherIncome([FromBody] PayAccountsWithReferenceRequestModel request)
		{
			var result = await _payService.PayAccByVoenWithPrimechanie(request, 6);
			return Ok(result);
		}
		/// <summary>
		/// Pay to Legal Persons Accounts  by Account Number, Other Incomes.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="amount"></param>
		/// <param name="primechanie"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayLegalPersonsAccountByAccountNumberOtherIncome")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayLegalAcccountByAccountFounder(string transactionId, decimal amount, string primechanie)
		{
			var result = await _payService.PayAccByAccountNumberWithPrimechanie(transactionId, amount, 5, primechanie);
			return Ok(result);
		}

	}
}
