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

		public List<CustomerPurchases> GetPurchases()
		{
			return _context.CustomerPurchases.ToList();
		}

		public bool SavePurchases(List<CustomerPurchases> purchases)
		{
			_context.CustomerPurchases.AddRange(purchases);
			_context.SaveChanges();
			return true;
			
		}
	}
}
