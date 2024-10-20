using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.AppServicesInterface;
using PaymentsApplication.Interfaces.Token;
using PaymentsApplication.Models.Configs;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.OperationInterfaces;

namespace PaymentsApplication.Services.AppServices
{
	public class AppServices : IAppServices
	{
		private readonly ITokenService _tokenService;
		private readonly ILogger<AppServices> _logger;
		private readonly IGetOperStatus _getOperStatus;
		public AppServices(ITokenService tokenService, ILogger<AppServices> logger, IGetOperStatus getOperStatus)
		{
			_tokenService = tokenService;
			_logger = logger;
			_getOperStatus = getOperStatus;
		}

		public string GenerateTransactionId(string serviceId)
		{
			string result;
			Random RandNum = new Random();
			int RandomNumber = 0;
			for (int i = 1; i < 100; i++)
			{
				RandomNumber = RandNum.Next(1000000, 99999999);
			}
			result = serviceId + RandomNumber.ToString();
			return result;
		}
		public string GetCardType(string cardNumber)
		{
			string cardType = Convert.ToInt32(cardNumber[..1]) switch
			{
				5 => "MASTERCARD",
				6 => "MAESTRO",
				_ => "VISA",
			};
			return cardType;
		}
		public async Task<ReadContextResponse> GetInfoFromContext(HttpContext context)
		{
			var ip = context.Connection.RemoteIpAddress.ToString();
			ip ??= "0.0.0.0";
			_logger.LogInformation("ip =>" + ip);
			var token = context.Request.Headers["Authorization"].ToString().Trim();
			_logger.LogInformation("token =>" + token);
			var response = _tokenService.Verify(token);
			_logger.LogInformation("response Verify token =>" + JsonConvert.SerializeObject(response));
			if (response == null)
				throw new BadRequestException(ExceptionCodes.InvalidCredentials, "Token yanlışdır!");
			var terminalId = response.TerminalId;
			_logger.LogInformation("teminalId =>" + terminalId);
			var username = response.UserName;
			_logger.LogInformation("username =>" + username);
			return new ReadContextResponse
			{
				Token = token,
				Ip = ip,
				TerminalId = terminalId,
				UserName = username
			};

		}
		public async Task<string> GetOperationStatus(string transactionId, HttpContext context)
		{
			_logger.LogInformation("transactionId =>" + transactionId);
			var request = await GetInfoFromContext(context);
			var terminalId = request.TerminalId;
			_logger.LogInformation("teminalId =>" + terminalId);
			string decryptedTransactionId = TextEncryption.Decrypt(transactionId);
			_logger.LogInformation("decryptedTransactionId =>" + decryptedTransactionId);
			var response = await _getOperStatus.GetOperationStatus(decryptedTransactionId, terminalId);
			if (response == 0)
				throw new NotFoundException(ExceptionCodes.NotFound, "Not found.");
			return "Success";


		}
	}
}
