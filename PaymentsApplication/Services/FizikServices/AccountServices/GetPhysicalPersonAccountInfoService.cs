using Microsoft.AspNetCore.Http;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.AppServicesInterface;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.AccountServicesInterfaces;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Response.AccountPayments;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.FizikInterfaces.Accounts;
using PaymentsDataLayer.Models.FizikModels.AccountsModels;

namespace PaymentsApplication.Services.FizikServices.AccountServices
{
	public class GetPhysicalPersonAccountInfoService : IGetAccountInfoService
	{
		private readonly IGetAccInfo _getAccInfoService;
		private readonly IAppServices _appService;
		private readonly ISaveAccountInfo _saveAccountInfo;

		public GetPhysicalPersonAccountInfoService(IGetAccInfo getAccInfoByPinService, IAppServices appService, ISaveAccountInfo saveAccountInfo)
		{
			_getAccInfoService = getAccInfoByPinService;
			_appService = appService;
			_saveAccountInfo = saveAccountInfo;
		}
		public async Task<LicschHolderInfoResponseModel> GetLicschInfoByFin(string pincode, HttpContext context)
		{
			if (string.IsNullOrEmpty(pincode) || pincode.Length > 7)
				throw new BadRequestException(ExceptionCodes.NotFound, "Finkod düzgün daxil edilməyib.");
			var resultAccInfo = await _getAccInfoService.GetAccInfoByPin(pincode);
			if (!resultAccInfo.Any())
			{
				throw new BadRequestException(ExceptionCodes.NotFound, "Finkod üzrə hesab məlumatları tapılmadı.");
			}
			var trId = _appService.GenerateTransactionId("FP");
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
				IpAddress = responseContext.Ip
			};
			await _saveAccountInfo.SaveAccInfo(accInfo);

			return response;
		}
		public async Task<LicschHolderInfoResponseModel> GetLicschInfoByAccountNumber(string accountNumber, HttpContext context)
		{
			if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length != 28 && accountNumber.Length != 20)
				throw new BadRequestException(ExceptionCodes.NotFound, "Hesab düzgün formatda daxil edilməyib.");
			if (accountNumber.Length == 28)
				accountNumber = accountNumber.Substring(8, 20);
			var responseContext = await _appService.GetInfoFromContext(context) ?? throw new BadRequestException(ExceptionCodes.ValidationError, "Sorğuda səhv var!");
			var resultAccInfo = await _getAccInfoService.GetAccInfoByAccount(accountNumber) ?? throw new BadRequestException(ExceptionCodes.NotFound, "Hesab üzrə  məlumat tapılmadı.");
			var trId = _appService.GenerateTransactionId("FP");
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
				IpAddress = responseContext.Ip
			};
			await _saveAccountInfo.SaveAccInfo(accInfo);
			return response;
		}
	}
}
