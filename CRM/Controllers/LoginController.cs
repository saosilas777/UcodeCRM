using Microsoft.AspNetCore.Mvc;
using CRM.Data;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;
using CRM.Repository;
using CRM.Services;
using CRM.ViewComponents;
using System.ComponentModel;

namespace CRM.Controllers
{

	public class LoginController : Controller
	{
		#region Dependencies
		private readonly ILoginRepository _loginRepository;
		private readonly IUserRepository _userRepository;
		private readonly Interfaces.IUserSession _session;

		private readonly ISendEmail _sendEmail;

		public LoginController(ILoginRepository loginRepository,
							  IUserRepository userRepository,
                              Interfaces.IUserSession section,
							  ISendEmail sendEmail)
		{
			_loginRepository = loginRepository;
			_userRepository = userRepository;
			_session = section;
			_sendEmail = sendEmail;

		}

		#endregion

		public IActionResult Login()
		{
			var user = _session.GetUserSection();

			if (user != null) return RedirectToAction("Index", "Home");
			return View();
		}

		public IActionResult Logout()
		{
			_session.UserSectionRemove();
			return RedirectToAction("Login");
		}
		public IActionResult SignUp()
		{
			return View();

		}

		[HttpPost]
		public IActionResult Create(LoginModel loginModel)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					TempData["ErrorMessage"] = "Não foi possível criar sua conta, verifique os dados informados.";
					return View("SignUp");

				}
				var register = _loginRepository.Create(loginModel);
				if (register == false)
				{
					TempData["ErrorMessage"] = "Login informado já existente, por favor tente novamente!";
					return View("SignUp");
				};
				TempData["SuccessMessage"] = "Cadastro realizado com sucesso, faça seu login!";
				return RedirectToAction("Login", "Login");


			}
			catch (Exception e)
			{

				TempData["ErrorMessage"] = $"Não foi possível criar sua conta, verifique os dados informados. {e.Message}";
				return View("SignUp");
			}

		}

		[HttpPost]
		public IActionResult Entrar(LoginViewModel loginViewModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var loginDb = _loginRepository.BuscarPorLogin(loginViewModel.Login);
					if (loginDb != null && loginDb.Password == LoginServices.HashGeneration(loginViewModel.Password))
					{
						var user = _userRepository.BuscarPorLogin(loginDb.Login);
						
						if (user != null)
						{
							_session.UserSectionCreate(user);
							return RedirectToAction("Index", "Home");
						}

					}
					TempData["ErrorMessage"] = $"Usuário ou senha inválidos, tente novamente!.";
					return View("Login", loginViewModel);

				}

				TempData["ErrorMessage"] = $"Para acessar é necessario informar seu login e senha!";
				return View("Login", loginViewModel);

			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}

		}

		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult SendMail(string email)
		{

			try
			{
				if (email == null)
				{
					TempData["ErrorMessage"] = $"Informe um email válido para recuperar sua senha";
					return View("ForgotPassword", email);
				}

				var emailDb = _loginRepository.BuscarPorEmail(email);
				if (emailDb == false)
				{
					TempData["ErrorMessage"] = $"Email informado não cadastrado";
					return View("ForgotPassword");

				}
				string senhaProvisoria = Guid.NewGuid().ToString();

				if (_loginRepository.BuscarPorEmail(email))
				{
					_sendEmail.SendEmail(email, "Recuperação de senha", senhaProvisoria);
				}
				senhaProvisoria = LoginServices.HashGeneration(senhaProvisoria);
				_loginRepository.PasswordUpdate(senhaProvisoria, email);
				TempData["SuccessMessage"] = $"Nova senha enviada com sucesso!";
				return View("Login");

			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}

		}
	}
}
