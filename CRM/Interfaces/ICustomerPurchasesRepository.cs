using CRM.Models;

namespace CRM.Interfaces
{
	public interface ICustomerPurchasesRepository
	{
		bool SavePurchases(List<CustomerPurchases> purchases);
		List<CustomerPurchases> GetPurchases();
	}
}
