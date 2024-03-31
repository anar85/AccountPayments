using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.AccountServicesInterfaces;
using PaymentsApplication.Models.Request.PayAccountsRequest;

namespace AccountPaymentsAPI.Controllers.Payments.Fizik.Account_operations
{
	[Route("api/[controller]")]
	[ApiController]
	public class PhysicalPersonsAccountController : ControllerBase
	{
		private readonly IGetAccountInfoService _service;
		private readonly IPayPhsicalAccountsService _payService;


		public PhysicalPersonsAccountController(IGetAccountInfoService service, IPayPhsicalAccountsService payService)
		{
			_service = service;
			_payService = payService;
		}
		/// <summary>
		/// Get Information about Accounts of physical person by FinCode.
		/// </summary>
		/// <param pincode="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetPhysicalAccountInfoByPin")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetAccountInfoByPin(string pincode)
		{
			return Ok(await _service.GetLicschInfoByFin(pincode, HttpContext));
		}
		/// <summary>
		/// Pay to Physical Persons Accounts  by FinCode.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayPhysicalPersonsAccountByPin")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayAcccountByPin([FromBody] PayAccountsRequestModel request)
		{
			var result = await _payService.PayAccByPin(request);
			return Ok(result);
		}
		/// <summary>
		/// Get Information about Accounts of physical person by Account number.
		/// </summary>
		/// <param accountNumber="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetPhysicalAccountInfoByAccountNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> GetAccountInfoByAccountNumber(string accountNumber)
		{
			return Ok(await _service.GetLicschInfoByAccountNumber(accountNumber, HttpContext));
		}
		/// <summary>
		/// Pay to Physical Persons Accounts  by Account Number.
		/// </summary>
		/// <param transactionId="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PayPhysicalPersonsAccountByAccountNumber")]
		[Authorize]
		//[NonAction]
		public async Task<IActionResult> PayAcccountByAccount(PayRequestModel request)
		{
			var result = await _payService.PayAccByAccount(request);
			return Ok(result);
		}
	}
}
