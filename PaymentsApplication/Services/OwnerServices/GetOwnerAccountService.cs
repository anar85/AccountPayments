using Microsoft.AspNetCore.Http;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.AppServicesInterface;
using PaymentsApplication.Interfaces.OwnerServicesInterfaces;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Response.AccountPayments;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.FizikInterfaces.Accounts;
using PaymentsDataLayer.Interface.OwnerInterfaces;
using PaymentsDataLayer.Models.FizikModels.AccountsModels;

namespace PaymentsApplication.Services.OwnerServices
{
	public class GetOwnerAccountService : IGetOwnerAccountService
	{
		private readonly IGetOwnerAccountInfoDb _getOwnerAccountInfoDb;
		private readonly IAppServices _appService;
		private readonly ISaveAccountInfo _saveAccountInfo;
		public GetOwnerAccountService(IGetOwnerAccountInfoDb getLegalAccountInfoDb, IAppServices appService, ISaveAccountInfo saveAccountInfo)
		{
			_getOwnerAccountInfoDb = getLegalAccountInfoDb;
			_appService = appService;
			_saveAccountInfo = saveAccountInfo;
		}
		public async Task<LicschHolderInfoResponseModel> GetOwnerLicschInfoByVoen(string voen, string payerPincode, DateTime payerBirthDate, HttpContext context)
		{
			if (string.IsNullOrEmpty(payerPincode) || payerPincode.Length != 7 || payerPincode.ToUpper().Contains('O'))
				throw new BadRequestException(ExceptionCodes.NotFound, "Finkod düzgün daxil edilməyib.");
			if (string.IsNullOrEmpty(voen) || voen.Length != 10 || voen.Substring(9, 1) != "2")
				throw new BadRequestException(ExceptionCodes.NotFound, "VÖEN düzgün daxil edilməyib.");
			var resultAccInfo = await _getOwnerAccountInfoDb.GetOwnerAccInfoByVoen(voen);
			if (!resultAccInfo.Any())
			{
				throw new BadRequestException(ExceptionCodes.NotFound, "VÖEN üzrə hesab məlumatları tapılmadı.");
			}
			var trId = _appService.GenerateTransactionId("OP");
			var response = resultAccInfo.Select(x => new LicschHolderInfoResponseModel
			{
				FullName = x.FullName,
				FinCode = x.PinCode,
				Passport = x.Passport,
				TransactionId = TextEncryption.Encrypt(trId)

			}).FirstOrDefault();

			foreach (var item in resultAccInfo)
			{
				var model = new HoldersLicschsInfo
				{
					Licsch = item.Licsch,
					Currency = item.Currency,
					FilialNumber = item.FilialNumber,
					FilialName = item.FilialName

				};
				response.AccountsInfo.Add(model);
			}
			var responseContext = await _appService.GetInfoFromContext(context) ?? throw new BadRequestException(ExceptionCodes.ValidationError, "Sorğuda səhv var!");
			var accInfo = new AccountInfoSaveRequestModel
			{
				TransactionId = trId,
				EncryptedTransactionId = TextEncryption.Encrypt(trId),
				FullName = response.FullName,
				FinCode = response.FinCode,
				Passport = response.Passport,
				TerminalId = responseContext.TerminalId,
				IpAddress = responseContext.Ip,
				PayerFincode = payerPincode.ToUpper(),
				PayerBirthdate = payerBirthDate.ToString("d"),
				Voen = voen
			};
			await _saveAccountInfo.SaveAccInfo(accInfo);

			return response;
		}
		public async Task<LicschHolderInfoResponseModel> GetOwnerLicschInfoByAccountNumber(string accountNumber, string payerPincode, DateTime payerBirthDate, HttpContext context)
		{
			if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length != 28 && accountNumber.Length != 20)
				throw new BadRequestException(ExceptionCodes.NotFound, "Hesab düzgün formatda daxil edilməyib.");
			if (accountNumber.Length == 28)
				accountNumber = accountNumber.Substring(8, 20);
			var responseContext = await _appService.GetInfoFromContext(context) ?? throw new BadRequestException(ExceptionCodes.ValidationError, "Sorğuda səhv var!");
			var resultAccInfo = await _getOwnerAccountInfoDb.GetOwnerAccInfoByAccount(accountNumber) ?? throw new BadRequestException(ExceptionCodes.NotFound, "Hesab üzrə  məlumat tapılmadı.");
			var trId = _appService.GenerateTransactionId("OP");
			var response = new LicschHolderInfoResponseModel
			{
				FullName = resultAccInfo.FullName,
				FinCode = resultAccInfo.PinCode,
				Passport = resultAccInfo.Passport,
				TransactionId = TextEncryption.Encrypt(trId),
				AccountsInfo = new List<HoldersLicschsInfo>
				{
					new HoldersLicschsInfo
					{
						Currency = resultAccInfo.Currency,
						FilialNumber = resultAccInfo.FilialNumber,
						Licsch = resultAccInfo.Licsch,
						FilialName = resultAccInfo.FilialName
					}
				}
			};
			var accInfo = new AccountInfoSaveRequestModel
			{
				TransactionId = trId,
				EncryptedTransactionId = TextEncryption.Encrypt(trId),
				FullName = response.FullName,
				FinCode = response.FinCode,
				Passport = response.Passport,
				Currency = response.AccountsInfo.Select(x => x.Currency).FirstOrDefault(),
				BranchCode = Convert.ToInt32(response.AccountsInfo.Select(x => x.FilialNumber).FirstOrDefault()),
				CurrentAccount = response.AccountsInfo.Select(x => x.Licsch).FirstOrDefault(),
				FeePercent = 1.18M,
				TerminalId = responseContext.TerminalId,
				IpAddress = responseContext.Ip,
				PayerFincode = payerPincode.ToUpper(),
				PayerBirthdate = payerBirthDate.ToString("d")
			};
			await _saveAccountInfo.SaveAccInfo(accInfo);
			return response;
		}
	}
}
