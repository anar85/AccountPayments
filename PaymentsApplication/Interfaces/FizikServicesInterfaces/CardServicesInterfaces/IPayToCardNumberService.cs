using System.Threading.Tasks;

namespace PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces
{
	public interface IPayToCardNumberService
	{
		Task<string> PayCard(string transactionId, decimal amount);
	}
}
