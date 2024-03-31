using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsApplication.Interfaces.AppServicesInterface;

namespace AccountPaymentsAPI.Controllers.Operations
{
	[Route("api/[controller]")]
	[ApiController]
	public class OperationsServiceController : ControllerBase
	{
		private readonly IAppServices _appServices;

		public OperationsServiceController(IAppServices appServices)
		{
			_appServices = appServices;
		}

		/// <summary>
		/// Get Status of operation by transaction id.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetOperationStatusByTransactionId")]
		[Authorize]
		public async Task<IActionResult> GetOwnerAccountInfoByVoen(string transactionId)
		{
			return Ok(await _appServices.GetOperationStatus(transactionId, HttpContext));
		}
	}
}
