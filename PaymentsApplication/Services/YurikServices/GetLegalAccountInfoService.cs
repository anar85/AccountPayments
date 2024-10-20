using Microsoft.AspNetCore.Http;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.AppServicesInterface;
using PaymentsApplication.Interfaces.YurikServicesİnterfaces;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Response.AccountPayments;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.FizikInterfaces.Accounts;
using PaymentsDataLayer.Interface.YurikInterfaces;
using PaymentsDataLayer.Models.FizikModels.AccountsModels;

namespace PaymentsApplication.Services.YurikServices
{
	public class GetLegalAccountInfoService : IGetLegalAccountInfoService
	{
		private readonly IGetLegalAccountInfoDb _getLegalAccountInfoDb;
		private readonly IAppServices _appService;
		private readonly ISaveAccountInfo _saveAccountInfo;
		public GetLegalAccountInfoService(IGetLegalAccountInfoDb getLegalAccountInfoDb, IAppServices appService, ISaveAccountInfo saveAccountInfo)
		{
			_getLegalAccountInfoDb = getLegalAccountInfoDb;
			_appService = appService;
			_saveAccountInfo = saveAccountInfo;
		}
		public async Task<LicschHolderInfoResponseModel> GetLicschInfoByVoen(string voen, string payerPincode, DateTime payerBirthDate, HttpContext context)
		{
			if (string.IsNullOrEmpty(payerPincode) || payerPincode.Length != 7 || payerPincode.ToUpper().Contains('O'))
				throw new BadRequestException(ExceptionCodes.NotFound, "Finkod düzgün daxil edilməyib.");
			if (string.IsNullOrEmpty(voen) || voen.Length != 10 || voen.Substring(9, 1) != "1")
				throw new BadRequestException(ExceptionCodes.NotFound, "VÖEN düzgün daxil edilməyib.");
			var resultAccInfo = await _getLegalAccountInfoDb.GetAccInfoByVoen(voen);
			if (!resultAccInfo.Any())
			{
				throw new BadRequestException(ExceptionCodes.NotFound, "VÖEN üzrə hesab məlumatları tapılmadı.");
			}
			var trId = _appService.GenerateTransactionId("LP");
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
		public async Task<LicschHolderInfoResponseModel> GetLicschInfoByAccountNumber(string accountNumber, string payerPincode, DateTime payerBirthDate, HttpContext context)
		{
			if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length != 28 && accountNumber.Length != 20)
				throw new BadRequestException(ExceptionCodes.NotFound, "Hesab düzgün formatda daxil edilməyib.");
			if (accountNumber.Length == 28)
				accountNumber = accountNumber.Substring(8, 20);
			var responseContext = await _appService.GetInfoFromContext(context) ?? throw new BadRequestException(ExceptionCodes.ValidationError, "Sorğuda səhv var!");
			var resultAccInfo = await _getLegalAccountInfoDb.GetLegalAccInfoByAccount(accountNumber) ?? throw new BadRequestException(ExceptionCodes.NotFound, "Hesab üzrə  məlumat tapılmadı.");
			var trId = _appService.GenerateTransactionId("LP");
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
