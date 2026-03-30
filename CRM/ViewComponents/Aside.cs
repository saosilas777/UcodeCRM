using CRM.Interfaces;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRM.ViewComponents
{
	public class Aside : ViewComponent
	{

		private readonly IUserSession _session;

		public Aside(IUserSession section)
		{
			_session = section;
		}

		public IViewComponentResult Invoke()
		{
			try
			{
				UserModel user = _session.GetUserSection();

				return View(user);

			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}


		}
	}
}
