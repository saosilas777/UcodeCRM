using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;
using CRM.Repository;
using CRM.Services;
using Newtonsoft.Json;

namespace CRM.Controllers
{
	public class AnalyticsController : Controller
	{
		private readonly AnalyticsServices _analyticsServices;
		private readonly IUserSession _session;

		public AnalyticsController(AnalyticsServices analyticsServices, IUserSession section)
		{
			_analyticsServices = analyticsServices;
			_session = section;
		}

		public IActionResult Index()
		{
			try
			{
				var user = _session.GetUserSection();
				
				if (user != null)
				{
					var analytics = _analyticsServices.AnalyticsBuilder();
					return View(analytics);
				}
				return RedirectToAction("Login", "Login");
				

			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}

		}

	}
}