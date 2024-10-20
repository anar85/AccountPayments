using Microsoft.Extensions.Logging;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.Payments;

namespace PaymentsApplication.Services.FizikServices.CardServices
{
	public class PayToCardNumberService : IPayToCardNumberService
	{
		private readonly IPayAccounts _payAccounts;
		private readonly ILogger<PayToCardNumberService> _logger;

		public PayToCardNumberService(IPayAccounts payAccounts, ILogger<PayToCardNumberService> logger)
		{
			_payAccounts = payAccounts;
			_logger = logger;
		}

		public async Task<string> PayCard(string transactionId, decimal amount)
		{
			_logger.LogInformation("Call PayCard method transactionId =>" + transactionId + " ; amount = > " + amount.ToString());
			string decryptedTransactionId = TextEncryption.Decrypt(transactionId);
			_logger.LogInformation("decryptedTransactionId =>" + decryptedTransactionId);
			string result = await _payAccounts.PayWithTransactionIdAmount(decryptedTransactionId, amount);
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəmiş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}

	}
}
