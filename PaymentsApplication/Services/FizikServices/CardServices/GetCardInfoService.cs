using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentsApplication.Exceptions;
using PaymentsApplication.Interfaces.AppServicesInterface;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces;
using PaymentsApplication.Interfaces.Token;
using PaymentsApplication.Models.Exceptions;
using PaymentsApplication.Models.Response.CardPayments;
using PaymentsApplication.Services.Encryption;
using PaymentsDataLayer.Interface.Fizik.Card;
using PaymentsDataLayer.Models.FizikModels.CardModels;
namespace PaymentsApplication.Services.FizikServices.CardServices
{
	public class GetCardInfoService : ICardInfoService

	{
		private readonly IGetCardInfo _getCardInfo;
		private readonly ISaveCardInfo _saveCardInfo;
		private readonly ITokenService _tokenService;
		private readonly IAppServices _appServices;
		private readonly ILogger<GetCardInfoService> _logger;

		public GetCardInfoService(IGetCardInfo getCardInfo, ISaveCardInfo saveCardInfo, ITokenService tokenService, IAppServices getTransactionId, ILogger<GetCardInfoService> logger)
		{
			_getCardInfo = getCardInfo;
			_saveCardInfo = saveCardInfo;
			_tokenService = tokenService;
			_appServices = getTransactionId;
			_logger = logger;
		}
		public async Task<CardHolderResponseModel> GetCardHolderName(string cardnumber, HttpContext context)
		{
			_logger.LogInformation("cardnumber =>" + cardnumber);
			if (!cardnumber.All(char.IsDigit) || cardnumber.Length < 16)
				throw new BadRequestException(ExceptionCodes.InvalidCredentials, "Kart nömrəsi düzgün daxil edilməyib!");
			var responseContext = await _appServices.GetInfoFromContext(context);
			if (responseContext == null)
				throw new BadRequestException(ExceptionCodes.ValidationError, "Sorğuda səhv var!");
			var item = await _getCardInfo.GetCardInfos(cardnumber);
			_logger.LogInformation("item from GetCardInfoService method  =>" + JsonConvert.SerializeObject(item));
			if (item == null)
				throw new BadRequestException(ExceptionCodes.InvalidCredentials, "Kart məlumatları tapılmadı!");
			var trId = _appServices.GenerateTransactionId("PC");
			var cardInfoRequest = new CardInfoRequestModel
			{
				TransactionId = trId,
				EncryptedTransactionId = TextEncryption.Encrypt(trId),
				FirstName = item.FIRSTNAME,
				LastName = item.LASTNAME,
				MiddleName = item.MIDDLENAME,
				CurrentAccount = item.CURRENTACCOUNT,
				CardNumber = item.CARDNUMBER,
				BranchCode = item.BRANCHCODE,
				Currency = item.CURRENCY,
				Passport = item.PASSPORT,
				FinCode = item.FINCODE,
				FeePercent = item.FEEPERCENT,
				TerminalId = responseContext.TerminalId,
				IpAddress = responseContext.Ip

			};
			_logger.LogInformation("cardInfoRequest for WriteInfo method  =>" + JsonConvert.SerializeObject(cardInfoRequest));
			_saveCardInfo.WriteInfo(cardInfoRequest);
			_logger.LogInformation(@"TransactionId =>" + TextEncryption.Encrypt(cardInfoRequest.TransactionId) + "\nMaskNameCardHolder = > " + cardInfoRequest.FullName +
				"\nFeePercent = > " + cardInfoRequest.FeePercent.ToString() + "\n-----------------------------------------");
			return new CardHolderResponseModel
			{
				TransactionId = TextEncryption.Encrypt(cardInfoRequest.TransactionId),
				MaskNameCardHolder = cardInfoRequest.FullName,
				Currency = cardInfoRequest.Currency,
				CardType = _appServices.GetCardType(cardnumber),
				FeePercent = cardInfoRequest.FeePercent
			};
		}

	}
}
