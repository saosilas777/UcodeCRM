using CRM.Data;
using CRM.Interfaces;
using CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Repository
{
	public class CustomerPurchasesRepository : ICustomerPurchasesRepository
	{

		private readonly Context _context;

		public CustomerPurchasesRepository(Context context)
		{
			_context = context;
		}

		public List<PurchaseModel> GetPurchases()
		{
			return _context.Purchases.AsNoTracking().ToList();
		}


		public void Add(PurchaseModel purchase)
		{
			_context.Add(purchase);
			_context.SaveChanges();
		}
		public bool SavePurchases(List<PurchaseModel> _purchases)
		{
			_context.UpdateRange(_purchases);
			_context.SaveChanges();


			return true;

		}

		public bool DeletePurchase(Guid id)
		{
			var purchases = _context.Purchases.FirstOrDefault(x => x.Id == id);
			_context.Purchases.Remove(purchases);
			_context.SaveChanges();
			return true;

		}


	}
}
