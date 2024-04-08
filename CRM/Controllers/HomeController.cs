using Microsoft.AspNetCore.Mvc;
using CRM.Interfaces;
using CRM.Models;
using CRM.Services;
using System.Reflection.PortableExecutable;
using CRM.Models.ViewModels;

namespace CRM.Controllers
{
	public class HomeController : Controller
	{
		#region Dependencies
		private readonly IUserSession _session;
		private readonly ICustomerRepository _customers;
		public HomeController(IUserSession section, ICustomerRepository customers)
		{
			_session = section;
			_customers = customers;

		}

		#endregion

		public IActionResult Index()
		{
			var user = _session.GetUserSection();
			var customers = _customers.BuscarTodos(user.Id);

			HomeViewModels homeModels = new();
			homeModels.Customers = customers;
			homeModels.User = user;

			if (_session.GetUserSection() == null){
				TempData["ErrorMessage"] = "É necessário efetuar seu login!";
				return RedirectToAction("Login", "Login");

			}

			return View(homeModels);
			
		}
	}
}
