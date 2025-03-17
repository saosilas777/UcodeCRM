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
		public PurchaseModel GetPurchaseById(Guid id)
		{
			return _context.Purchases.Where(p => p.Id == id).FirstOrDefault();
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
			DateTime date = DateTime.Now;
			var purchases = _context.Purchases.FirstOrDefault(x => x.Id == id);
			var customer = _context.Customers.Where(c => c.Id == purchases.CustomerId).FirstOrDefault();
			var lastPurchase = _context.Purchases.Where(x => x.CustomerId == customer.Id).OrderByDescending(p => p.PurchaseDate).Where(p => p.PurchaseDate < purchases.PurchaseDate).FirstOrDefault();

			if(lastPurchase != null)
			{
				var differenceDays = lastPurchase.PurchaseDate - date;
				if (differenceDays.Days < -90)
				{
					customer.Status = false;
					_context.Customers.Update(customer);
					_context.SaveChanges();
				}

			}
			else
			{
				customer.Status = false;
				_context.Customers.Update(customer);
				_context.SaveChanges();
			}
			

			
			_context.Purchases.Remove(purchases);
			_context.SaveChanges();
			return true;

		}


	}
}
