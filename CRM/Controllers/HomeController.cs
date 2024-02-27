using Microsoft.AspNetCore.Mvc;
using CRM.Interfaces;
using CRM.Models;
using CRM.Services;
using System.Reflection.PortableExecutable;

namespace CRM.Controllers
{
	public class HomeController : Controller
	{
		#region Dependencies
		private readonly IUserSession _session;
		public HomeController(IUserSession section)
		{
			_session = section;

		}

		#endregion

		public IActionResult Index(UserModel user)
		{
			if (_session.GetUserSection() == null){
				TempData["ErrorMessage"] = "É necessário efetuar seu login!";
				return RedirectToAction("Login", "Login");

			}

			return View(user);
			
		}
	}
}
