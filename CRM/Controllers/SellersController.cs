using CRM.Interfaces;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
	public class SellersController : Controller
	{
		private readonly IUserSession _session;
		private readonly ICustomerPurchasesRepository _purchase;
		private readonly ICustomerRepository _customer;

		public SellersController(IUserSession session, ICustomerPurchasesRepository purchase, ICustomerRepository customer)
		{
			_session = session;
			_purchase = purchase;
			_customer = customer;
		}
		public IActionResult SalesSeller()
		{
			var user = _session.GetUserSection();
			if (user != null)
			{
				List<PurchaseModel> purchases = new();

				return View(purchases);
			}
			/*TempData["ErrorMessage"] = "É necessário efetuar seu login!";*/
			return RedirectToAction("Login", "Login");
		}

		[HttpPost]
		public IActionResult SalesSeller(string initialDate, string finalDate)
		{
			var user = _session.GetUserSection();
			var customers = _customer.BuscarTodos();
			var purchases = _purchase.GetPurchases().Where(x => x.PurchaseDate.Date >= DateTime.Parse(initialDate) && x.PurchaseDate <= DateTime.Parse(finalDate)).ToList();

			for (int i = 0; i < purchases.Count(); i++)
			{
				purchases[i].Customer = customers.Find(c => c.Id == purchases[i].CustomerId);
			}

			return View(purchases);
		}
		public PurchaseModel PurchaseDelete(string id)
		{
			var purchase = _purchase.GetPurchaseById(Guid.Parse(id));
			Guid _id = Guid.Parse(id);
			_purchase.DeletePurchase(_id);
			return new PurchaseModel();
		}

	}
}
