using CRM.Interfaces;
using CRM.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CRM.Helpers
{
	public class Session : IUserSession
	{
		private IHttpContextAccessor _httpContext;

		public Session(IHttpContextAccessor httpContext)
		{
			_httpContext = httpContext;
		}

		public UserModel GetUserSection()
		{
			string? userSection = _httpContext.HttpContext.Session.GetString("User");
			if (userSection != null)
			{
				var user = JsonConvert.DeserializeObject<UserModel>(userSection);
				return user;
			}
			return null;
		}

		public void UserSectionCreate(UserModel user)
		{
			var _user = JsonConvert.SerializeObject(user);
			_httpContext.HttpContext.Session.SetString("User", _user);
		}

		public void UserSectionRemove()
		{
			_httpContext.HttpContext.Session.Remove("User");
		}
	}
}