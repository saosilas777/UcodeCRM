using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;
using CRM.Repository;
using CRM.Services;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using RestSharp;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Controllers
{
	public class CustomersController : Controller
	{
		#region Dependencies
		private readonly ICustomerRepository _customerRepository;
		private readonly ICustomerPurchasesRepository _purchases;
		private readonly CustomerStatusVerify _statusVerify;

		private readonly IUserSession _session;
		private object customerStatusVerify;

		public CustomersController(ICustomerRepository customer,
								   IUserSession session,
								   ICustomerPurchasesRepository purchasesRepository,
								   CustomerStatusVerify statusVerify
									)
		{
			_customerRepository = customer;
			_session = session;
			_purchases = purchasesRepository;
			_statusVerify = statusVerify;
		}
		#endregion


		public IActionResult Index(string initialDate, string finalDate)
		{
			var date = DateTime.Now;
			var user = _session.GetUserSection();
			if (user != null)
			{
				List<CustomerModel> customers = new();
				if (initialDate == null || finalDate == null)
				{
					customers = _customerRepository.BuscarTodos(user.Id);
					if (!customers.IsNullOrEmpty() && customers[0].LastUpdated.Date < date.Date)
					{
						_statusVerify.StatusVerify(customers);
					}

					return View(customers);
				}
				else
				{
					customers = _customerRepository.BuscarTodos(user.Id).Where(x => x.NextContactDate >= DateTime.Parse(initialDate) && x.NextContactDate <= DateTime.Parse(finalDate)).ToList();
					return View(customers);
				}

			}
			/*TempData["ErrorMessage"] = "É necessário efetuar seu login!";*/
			return RedirectToAction("Login", "Login");
		}

		[HttpGet]
		public IActionResult ExportXls()
		{
			var user = _session.GetUserSection();
			var customers = _customerRepository.BuscarTodos(user.Id);
			ExportXlsService.ExportXls(customers);

			return ExportFile();
		}
		public IActionResult Editar(Guid id)
		{
			var user = _session.GetUserSection();
			CustomerEditViewModel customerDb = _customerRepository.BuscarPorId(id);
			if(user == null)
			{
				/*TempData["ErrorMessage"] = "É necessário efetuar seu login!";*/
				return RedirectToAction("Login", "Login");

			}
			if (customerDb.UserId != user.Id)
			{
				TempData["ErrorMessage"] = "Cadastro não encontrado!";
				return RedirectToAction("Index", "Customers");
				
			}
			return View(customerDb);

		}
		public IActionResult Create()
		{
			var user = _session.GetUserSection();
			CustomerCreateViewModel customerCreate = new();
			if (user != null)
			{
				return View(customerCreate);
			}
			/*TempData["ErrorMessage"] = "É necessário efetuar seu login!";*/
			return RedirectToAction("Login", "Login");

		}

		[HttpPost]
		public IActionResult Create(CustomerCreateViewModel customer)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var _customer = _customerRepository.Create(customer);
					if (_customer != null)
					{
						//TempData["SuccessMessage"] = "";
						return RedirectToAction("Editar", new { id = _customer.Id });
					}
					TempData["ErrorMessage"] = "Não foi possível efeturar o cadastro";
					return View(customer);

				}

				TempData["ErrorMessage"] = "Todos os dados devem estar preenchidos, tente novamente!";
				return View(customer);

			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

		}

		[HttpPost]
		public IActionResult Edit(CustomerEditViewModel customer)
		{
			var update = _customerRepository.Atualizar(customer);
			if (update)
			{
				TempData["SuccessMessage"] = "Atualização feita com sucesso";
				return RedirectToAction("Editar", "Customers", new { customer.Id });
			}
			TempData["ErrorMessage"] = "Ocorreu um erro ao tentar atualizar o cadastro";
			return RedirectToAction("Editar", "Customers", new { customer.Id });
		}
		[HttpPost]
		public IActionResult RegistrationContact(string anotation, string id, string date, string index)
		{
			_customerRepository.RegistrationContact(anotation, date, Guid.Parse(id));
			Guid _id = Guid.Parse(id);
			if(index == null) return RedirectToAction("Editar", "Customers", new { id = _id });
			return RedirectToAction("Index", "Customers");
		}

		[HttpPost]
		public IActionResult Delete(CustomerModel customer)
		{
			_customerRepository.Deletar(customer);
			return RedirectToAction("Index", "Customers");
		}

		[HttpPost]
		public IActionResult ContactDateEdit(string nextContactDate, string id, string checkCalendary)
		{
			if (checkCalendary != "on")
			{
				_customerRepository.ContactDateEdit(DateTime.Parse(nextContactDate), Guid.Parse(id));
				return RedirectToAction("Index", "Customers");

			}
			_customerRepository.ChangeAllNextContactsDates(nextContactDate);
			return RedirectToAction("Index", "Customers");

		}
		[HttpPost]
		public IActionResult UpdateNextContactDate(string reciverStringfy)
		{
			UpdateCustomerContacts(reciverStringfy);
			TempData["SuccessMessage"] = "Atualização feita com sucesso";
			return RedirectToAction("Index");
		}
		public FileStreamResult ExportFile()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/clientes.xlsx");
			var memory = new MemoryStream();
			using (var stream = new FileStream(path, FileMode.Open))
			{
				stream.CopyTo(memory);
			}
			memory.Position = 0;
			var contentType = "APPLICATION/octet-stream";
			var fileName = Path.GetFileName(path);

			return File(memory, contentType, fileName);
		}


		public IActionResult ChangePriority(string priority, string priorityId)
		{
			_customerRepository.ChangePriority(priority, priorityId);
			return RedirectToAction("Index");
		}
		public IActionResult FindCustumer(string headerSearch)
		{
			var code = headerSearch.Split(" - ");
			var customerCode = code[0];
			var cus = _customerRepository.BuscarPorCodigo(int.Parse(customerCode));
			var i = 0;
			return RedirectToAction("Index");
		}

		public void UpdateCustomerContacts(string reciverStringfy)
		{
			var updates = JsonConvert.DeserializeObject<List<UpdateNextContactDateViewModel>>(reciverStringfy);

			_customerRepository.UpdateNextContactDates(updates);
		}

	}
}
