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

namespace CRM.Controllers
{
	public class CustomersController : Controller
	{
		#region Dependencies
		private readonly ICustomerRepository _customer;
		private readonly ICustomerPurchasesRepository _purchases;

		private readonly IUserSession _session;
		public CustomersController(ICustomerRepository customer,
								   IUserSession session, ICustomerPurchasesRepository purchasesRepository
									)
		{
			_customer = customer;
			_session = session;
			_purchases = purchasesRepository;
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
					customers = _customer.BuscarTodos(user.Id);
					return View(customers);
				}
				else
				{
					customers = _customer.BuscarTodos(user.Id).Where(x => x.NextContactDate >= DateTime.Parse(initialDate) && x.NextContactDate <= DateTime.Parse(finalDate)).ToList();
					return View(customers);

				}

			}
			TempData["ErrorMessage"] = "É necessário efetuar seu login!";
			return RedirectToAction("Login", "Login");
		}

		[HttpGet]
		public IActionResult ExportXls()
		{
			var user = _session.GetUserSection();
			var customers = _customer.BuscarTodos(user.Id);
			ExportXlsService.ExportXls(customers);

			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/clientes.xlsx");
			var memory = new MemoryStream();
			using(var stream = new FileStream(path, FileMode.Open))
			{
				stream.CopyTo(memory);
			}
			memory.Position = 0;
			var contentType = "APPLICATION/octet-stream";
			var fileName = Path.GetFileName(path);
			return File(memory,contentType,fileName);
		}
		public IActionResult GetByStatus()
		{
			return View();
		}
		[HttpPost]
		public IActionResult GetByStatus(string status)
		{
			if (status == null) return View("Index");
			var user = _session.GetUserSection();
			List<CustomerModel> customers = _customer.GetByStatus(status);
			return View(customers);
		}

		public IActionResult Editar(Guid id)
		{
			var user = _session.GetUserSection();
			if (user != null)
			{
				CustomerEditViewModel customerDb = _customer.BuscarPorId(id);
				return View(customerDb);
			}
			TempData["ErrorMessage"] = "É necessário efetuar seu login!";
			return RedirectToAction("Login", "Login");

		}

		public IActionResult Create()
		{
			var user = _session.GetUserSection();
			CustomerCreateViewModel customerCreate = new();
			if (user != null)
			{
				return View(customerCreate);
			}
			TempData["ErrorMessage"] = "É necessário efetuar seu login!";
			return RedirectToAction("Login", "Login");

		}

		[HttpPost]
		public IActionResult Create(CustomerCreateViewModel customer)
		{
			try
			{
				if (ModelState.IsValid)
				{
					string res = _customer.Create(customer);
					if (res == "Cadastro realizado com sucesso!")
					{
						TempData["SuccessMessage"] = res;
						return View(customer);
					}
					TempData["ErrorMessage"] = res;
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
			var update = _customer.Atualizar(customer);
			if (update)
			{
				TempData["SuccessMessage"] = "Atualização feita com sucesso";
				return RedirectToAction("Editar", "Customers", new { customer.Id });
			}
			TempData["ErrorMessage"] = "Ocorreu um erro ao tentar atualizar o cadastro";
			return RedirectToAction("Editar", "Customers", new { customer.Id });
		}
		[HttpPost]
		public IActionResult RegistrationContact(string anotation, string id, string date)
		{
			_customer.RegistrationContact(anotation, date, Guid.Parse(id));
			/*return RedirectToAction("Editar", "Customers", new { customer.Id });*/
			Guid _id = Guid.Parse(id);
			return RedirectToAction("Editar", "Customers", new { id = _id });
		}

		[HttpPost]
		public IActionResult Delete(CustomerModel customer)
		{
			_customer.Deletar(customer);
			return RedirectToAction("Index", "Customers");
		}

		[HttpPost]
		public IActionResult GetByZipCode(string cep)
		{
			var url = $"https://viacep.com.br/ws/{cep}/json";
			var response = GET(url);

			ZipCodeModel result = JsonConvert.DeserializeObject<ZipCodeModel>(response);


			CustomerCreateViewModel customerCreate = new();
			customerCreate.Cidade = result.localidade;
			customerCreate.Uf = result.uf;



			return View("Create", customerCreate);
		}

		[HttpPost]
		public IActionResult ContactDateEdit(string nextContactDate, string id)
		{
			_customer.ContactDateEdit(DateTime.Parse(nextContactDate), Guid.Parse(id));
			return RedirectToAction("Index","Customers");
			
		}


		public static dynamic GET(string url)
		{
			var client = new RestClient(url);
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response.Content;

		}




	}
}
