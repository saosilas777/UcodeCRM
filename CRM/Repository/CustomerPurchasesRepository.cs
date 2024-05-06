using CRM.Data;
using CRM.Interfaces;
using CRM.Models;

namespace CRM.Repository
{
	public class CustomerPurchasesRepository : ICustomerPurchasesRepository
	{

		private readonly Context _context;

		public CustomerPurchasesRepository(Context context)
		{
			_context = context;
		}

		public List<CustomerPurchasesModel> GetPurchases()
		{
			return _context.CustomerPurchases.ToList();
		}


		public void AddPurchase(CustomerPurchasesModel purchase)
		{
			_context.CustomerPurchases.Update(purchase);
			_context.SaveChanges();
		}
		public bool SavePurchases(List<CustomerPurchasesModel> purchases)
		{
			_context.CustomerPurchases.AddRange(purchases);
			_context.SaveChanges();
			return true;
			
		}
		
		public bool DeletePurchase(Guid id)
		{
			var purchases = _context.CustomerPurchases.FirstOrDefault(x => x.Id == id);
			_context.CustomerPurchases.Remove(purchases);
			_context.SaveChanges();
			return true;
			
		}


	}
}
