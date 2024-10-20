using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.YurikServicesİnterfaces;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Request.PayAccountsRequest;
using PaymentsApplication.Services.Encryption;
using PaymentsApplication.Services.FizikServices.AccountServices;
using PaymentsDataLayer.Interface.FizikInterfaces.Accounts;
using PaymentsDataLayer.Interface.Payments;
using PaymentsDataLayer.Models.FizikModels.AccountsModels;

namespace PaymentsApplication.Services.YurikServices
{
	public class PayLegalAccountsService : IPayLegalAccountsService
	{
		private readonly ISaveAccountInfo _saveAccountInfo;
		private readonly ILogger<PayPhsicalAccountsService> _logger;
		private readonly IPayAccounts _payAccounts;

		public PayLegalAccountsService(ISaveAccountInfo saveAccountInfo, ILogger<PayPhsicalAccountsService> logger, IPayAccounts payAccounts)
		{
			_saveAccountInfo = saveAccountInfo;
			_logger = logger;
			_payAccounts = payAccounts;
		}
		public async Task<string> PayAccByVoen(PayAccountsRequestModel payAccountsRequest, int serviceId)
		{
			_logger.LogInformation("Call PayAccByVoen method payAccountsRequest =>" + JsonConvert.SerializeObject(payAccountsRequest));
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
			string result = await _payAccounts.PayByAccountNumber(decryptedTransactionId, payAccountsRequest.Amount, serviceId, null);
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəmiş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}
		public async Task<string> PayLegalAccByAccountNumber(string transactionId, decimal amount, int serviceId)
		{
			_logger.LogInformation("Call PayCard method transactionId =>" + transactionId + " ; amount = > " + amount.ToString());
			string decryptedTransactionId = TextEncryption.Decrypt(transactionId);
			_logger.LogInformation("decryptedTransactionId =>" + decryptedTransactionId);
			string result = await _payAccounts.PayByAccountNumber(decryptedTransactionId, amount, serviceId, null);
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəniş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}
		public async Task<string> PayAccByVoenWithPrimechanie(PayAccountsWithReferenceRequestModel payAccountsRequest, int serviceId)
		{
			_logger.LogInformation("Call PayAccByVoen method payAccountsRequest =>" + JsonConvert.SerializeObject(payAccountsRequest));
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
			string result = await _payAccounts.PayByAccountNumber(decryptedTransactionId, payAccountsRequest.Amount, serviceId, payAccountsRequest.Primechanie);
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəmiş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}
		public async Task<string> PayAccByAccountNumberWithPrimechanie(string transactionId, decimal amount, int serviceId, string primechanie)
		{
			_logger.LogInformation("Call PayCard method transactionId =>" + transactionId + " ; amount = > " + amount.ToString());
			string decryptedTransactionId = TextEncryption.Decrypt(transactionId);
			_logger.LogInformation("decryptedTransactionId =>" + decryptedTransactionId);
			string result = await _payAccounts.PayByAccountNumber(decryptedTransactionId, amount, serviceId, primechanie);
			_logger.LogInformation("result from PayWithTransactionIdAmount method =>" + result + "\n-------------------------------------------");
			if (result != "SUCCESS")
				throw new BadRequestException(ExceptionCodes.InternalServerError, "Ödəniş Uğursuz oldu,Texniki Dəstək ilə əlaqə saxlayın!");
			return result;
		}
	}
}
