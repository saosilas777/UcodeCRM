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
				List<CustomerPurchases> purchases = new();

				return View(purchases);
			}
			TempData["ErrorMessage"] = "É necessário efetuar seu login!";
			return RedirectToAction("Login", "Login");
		}

		[HttpPost]
		public IActionResult SalesSeller(string initialDate, string finalDate)
		{
			var user = _session.GetUserSection();

			var customers = _customer.BuscarTodos(user.Id);


			var purchases = _purchase.GetPurchases().Where(x => x.UserId == user.Id
			&& x.PurchaseDate >= DateTime.Parse(initialDate)
			&& x.PurchaseDate <= DateTime.Parse(finalDate)).ToList();

			for (int i = 0; i < purchases.Count(); i++)
			{
				for (int j = 0; j < customers.Count(); j++)
				{
					if (purchases[i].CustomerCode.ToString() == customers[j].Codigo)
					{
						purchases[i].Customer = customers[j];

					}

				}

			}

			return View(purchases);
		}

		public IActionResult PurchaseDelete(Guid id)
		{
			_purchase.DeletePurchase(id);
			return RedirectToAction("SalesSeller","Seller");
		}

	}
}
