using CRM.Models;

namespace CRM.Interfaces
{
	public interface ICustomerPurchasesRepository
	{
		bool SavePurchases(List<CustomerPurchasesModel> purchases);
		void AddPurchase(CustomerPurchasesModel purchase);
		List<CustomerPurchasesModel> GetPurchases();
		bool DeletePurchase(Guid id);
	}
}
