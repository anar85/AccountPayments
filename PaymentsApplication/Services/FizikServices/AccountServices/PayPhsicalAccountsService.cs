using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.AccountServicesInterfaces;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Request.PayAccountsRequest;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.FizikInterfaces.Accounts;
using PaymentsDataLayer.Interface.Payments;
using PaymentsDataLayer.Models.FizikModels.AccountsModels;

namespace PaymentsApplication.Services.FizikServices.AccountServices
{
	public class PayPhsicalAccountsService : IPayPhsicalAccountsService
	{
		private readonly ISaveAccountInfo _saveAccountInfo;
		private readonly ILogger<PayPhsicalAccountsService> _logger;
		private readonly IPayAccounts _payAccounts;
		public PayPhsicalAccountsService(ISaveAccountInfo saveAccountInfo, ILogger<PayPhsicalAccountsService> logger, IPayAccounts payAccounts)
		{
			_saveAccountInfo = saveAccountInfo;
			_logger = logger;
			_payAccounts = payAccounts;
		}
		public async Task<string> PayAccByPin(PayAccountsRequestModel payAccountsRequest)
		{
			_logger.LogInformation("Call PayAccByPin method payAccountsRequest =>" + JsonConvert.SerializeObject(payAccountsRequest));
			string decryptedTransactionId = TextEncryption.Decrypt(payAccountsRequest.TransactionId);
			_logger.LogInformation("decryptedTransactionId =>" + decryptedTransactionId);
			var info = new AccountInfoSaveRequestModel
			{
				TransactionId = decryptedTransactionId,
				CurrentAccount = payAccountsRequest.Account,
				Currency = payAccountsRequest.Currency,
				Amount = payAccountsRequest.Amount,
				BranchCode = payAccountsRequest.Filialnumber,
				FeePercent = 1.18M
			};
			bool updateQuery = await _saveAccountInfo.ChangeAccInfo(info);
			_logger.LogInformation("result from ChangeAccInfo method =>" + updateQuery + "\n-------------------------------------------");

			if (!updateQuery)
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəniş xətası,Texniki Dəstək ilə əlaqə saxlayın!");
			string result = await _payAccounts.PayByAccountNumber(decryptedTransactionId, payAccountsRequest.Amount, 1, "");
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəniş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}

		public async Task<string> PayAccByAccount(PayRequestModel request)
		{
			_logger.LogInformation("Call PayCard method transactionId =>" + request.TransactionId + " ; amount = > " + request.Amount.ToString());
			string decryptedTransactionId = TextEncryption.Decrypt(request.TransactionId);
			_logger.LogInformation("decryptedTransactionId =>" + decryptedTransactionId);
			string result = await _payAccounts.PayByAccountNumber(decryptedTransactionId, request.Amount, 2, null);
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəniş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}
	}
}
