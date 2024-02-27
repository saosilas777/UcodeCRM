using Microsoft.AspNetCore.Mvc;
using CRM.Models;
using CRM.Repository;
using CRM.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;
using CRM.Interfaces;

namespace CRM.ViewComponents
{
	public class Header : ViewComponent
	{
		private readonly SendFileImageRepository _sendFileImage;
		private readonly IUserSession _session;
		
		public Header(SendFileImageRepository sendFileImage,  
					  IUserSession section)
		{
			_sendFileImage = sendFileImage;
			_session = section;
		}
		

		public  async Task<IViewComponentResult> InvokeAsync()
		{
			try
			{
				var user = _session.GetUserSection();
				SendFileImageModel imageModel = _sendFileImage.GetById(user.Id);
				UserViewModel userView = new UserViewModel();

				if (imageModel == null)
				{
					userView.User = user;
					userView.Image = new SendFileImageModel()
					{
						Id = user.Id,
						Url = "",
						UserId = user.Id,
					};
					return View(userView);
				}
				userView.User = user;

				userView.Image = imageModel;


				return View(userView);
			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}

		}


	}
}
