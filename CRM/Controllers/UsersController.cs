using Microsoft.AspNetCore.Mvc;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;
using CRM.Services;

namespace CRM.Controllers
{
	public class UsersController : Controller
	{
		private readonly ILoginRepository _loginRepository;
		private readonly IUserSession _session;

		public UsersController(ILoginRepository loginRepository, IUserSession section)
		{
			_loginRepository = loginRepository;
			_session = section;
		}
	
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ChangePassword()
		{
			var user = _session.GetUserSection();
			ChangePasswordViewModel viewModel = new();
			viewModel.UserId = user.Id;
			return View(viewModel);
		}
		public IActionResult NonUserPage()
		{
			return View();
		}
		[HttpPost]
		public IActionResult ChangePassword(ChangePasswordViewModel viewModel)
		{
			try
			{
				if (viewModel.NewPassword != viewModel.ConfirmPassword)
				{
					TempData["ErrorMessage"] = "As novas senhas digitadas não conferem";
					return View("ChangePassword", viewModel);
				}
				if (!ModelState.IsValid)
				{
					TempData["ErrorMessage"] = "Os dados não conferem";
					return View("ChangePassword", viewModel);
				}
				LoginModel login = _loginRepository.BuscarPorId(viewModel.UserId);
				viewModel.NewPassword = LoginServices.HashGeneration(viewModel.NewPassword);
				viewModel.OldPassword = LoginServices.HashGeneration(viewModel.OldPassword);
				if (viewModel.OldPassword != login.Password)
				{
					TempData["ErrorMessage"] = "A senha atual não confere";
					return View("ChangePassword", viewModel);
				}
				if (viewModel.NewPassword == login.Password)
				{
					TempData["ErrorMessage"] = "A nova senha não pode ser igual a senha atual";
					return View("ChangePassword", viewModel);
				}
				
				login.Password = viewModel.NewPassword;
				_loginRepository.ChangePassword(login);
				TempData["SuccessMessage"] = "Senha Atualizada com sucesso!";
				return View();
				


			}
			catch (Exception)
			{

				throw;
			}
		}


	}
}
