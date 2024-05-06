using OfficeOpenXml;
using System.ComponentModel;
using CRM.Interfaces;
using CRM.Models;
using LicenseContext = System.ComponentModel.LicenseContext;
using System.Collections.Generic;
using CRM.Models.ViewModels;
using CRM.Repository;

namespace CRM.Services
{
	public class SendFileService
	{
		#region Dependencies
		private readonly IUserSession _session;
		private readonly ICustomerRepository _customerRepository;
		private readonly ICustomerPurchasesRepository _purchasesRepository;
		public SendFileService(IUserSession section,
							   ICustomerRepository customerRepository,
							   ICustomerPurchasesRepository puchasesRepository)
		{
			_session = section;
			_customerRepository = customerRepository;
			_purchasesRepository = puchasesRepository;
		}
		#endregion
		public string ReadXls(IFormFile uploadFile)
		{
			var streamFile = ReadStrem(uploadFile);

			var user = _session.GetUserSection();
			List<CustomerModel> customers = _customerRepository.BuscarTodos(user.Id);
			List<CustomerPurchasesModel> purchases = new();
			string result = string.Empty;
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

			using (ExcelPackage package = new((Stream)streamFile))
			{

				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
				int colCount = worksheet.Dimension.End.Column;
				int rowCount = worksheet.Dimension.End.Row;

				if (colCount <= 4)
				{
					for (int row = 2; row <= rowCount; row++)
					{
						CustomerPurchasesModel purchase = new CustomerPurchasesModel();

						var _customer = customers.Find(x => x.Codigo == worksheet.Cells[row, 1].Value.ToString());
						purchase.CustomerCode = int.Parse(worksheet.Cells[row, 1].Value.ToString());
						purchase.PurchaseDate = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString());
						purchase.PurchaseValue = double.Parse(worksheet.Cells[row, 4].Value.ToString());
						purchase.Customer = _customer;
						purchase.UserId = user.Id;



						purchases.Add(purchase);
					};
					result = VerifyDuplicatedPurchases(purchases);

					return result;
				}
				else
				{
					for (int row = 2; row <= rowCount; row++)
					{
						bool status;
						if (worksheet.Cells[row, 4].Value.ToString().ToUpper() == "ATIVO")
						{
							status = true;
						}
						else
						{
							status = false;
						}

						EmailModel _emailModel = new();
						_emailModel.Id = Guid.NewGuid();
						_emailModel.Email = worksheet.Cells[row, 5].Value.ToString();
						_emailModel.RegistrationDate = DateTime.Now;
						List<EmailModel> emails = new List<EmailModel>();
						emails.Add(_emailModel);

						PhoneModel _phoneModel = new();
						_phoneModel.Id = Guid.NewGuid();
						_phoneModel.Phone = worksheet.Cells[row, 6].Value.ToString();
						_phoneModel.RegistrationDate = DateTime.Now;
						List<PhoneModel> phones = new();
						phones.Add(_phoneModel);

						string cnpj = worksheet.Cells[row, 2].Value.ToString();

						CustomerModel customer = new CustomerModel()
						{

							Codigo = worksheet.Cells[row, 1].Value.ToString(),
							Cnpj = cnpj.PadLeft(13, '0'),
							RazaoSocial = worksheet.Cells[row, 3].Value.ToString(),
							Status = status,
							Emails = emails,
							Phones = phones,
							Contact = worksheet.Cells[row, 7].Value.ToString(),
							Cidade = worksheet.Cells[row, 8].Value.ToString(),
							Uf = worksheet.Cells[row, 9].Value.ToString(),
							NextContactDate = DateTime.Parse(worksheet.Cells[row, 13].Value.ToString()),
							UserId = user.Id,

						};

						customers.Add(customer);
					}

				}

				/*UpdateCustomers(customers);*/
				result = VerifyDuplicatedCustomers(customers);
				return result;
			}
		}

		public void UpdateCustomers(List<CustomerModel> customers)
		{
			var user = _session.GetUserSection();
			var customersDb = _customerRepository.BuscarTodos(user.Id);

			List<CustomerModel> _customers = new();

			foreach (var item in customersDb)
			{
				var customer = customers.Find(x => x.Codigo == item.Codigo);
				customer.Codigo = item.Codigo;
				customer.Cnpj = item.Cnpj;
				customer.RazaoSocial = item.RazaoSocial;
				_customers.Add(item);
			}
			_customerRepository.AtualizarTodos(_customers);
		}

		public string VerifyDuplicatedCustomers(List<CustomerModel> customer)
		{
			var user = _session.GetUserSection();
			List<CustomerModel> customerDb = _customerRepository.BuscarTodos(user.Id);
			int customerCount = customer.Count();
			if (customerDb.Count() > 0)
			{
				for (int i = 0; i < customerCount; i++)
				{
					for (int j = 0; j < customerDb.Count(); j++)
					{
						if (customerDb[j].Codigo == customer[i].Codigo
							&& customerDb[j].Cnpj == customer[i].Cnpj
							)

						{
							customer.Remove(customer[i]);
							break;
						}

					}
				}
			}
			if (customer.Count() > 0)
			{
				_customerRepository.AdicionarTodos(customer);
				return "Valores atualizados com sucesso!";
			}
			return "Nenhum valor a ser atualizado";
		}


		//TODO
		//implementar e verificar se não há duplicidade no envio das compras por cliente ()
		public string VerifyDuplicatedPurchases(List<CustomerPurchasesModel> purchases)
		{
			List<CustomerPurchasesModel> purchasesDb = _purchasesRepository.GetPurchases();

			Guid id = Guid.NewGuid();
			if (purchasesDb.Count() > 0)
			{
				for (int i = 0; i < purchases.Count(); i++)
				{
					for (int j = 0; j < purchasesDb.Count(); j++)
					{
						if (purchases[i].CustomerCode == purchasesDb[j].CustomerCode
							&& purchases[i].PurchaseDate == purchasesDb[j].PurchaseDate
							&& purchases[i].PurchaseValue == purchasesDb[j].PurchaseValue)

						{
							purchases[i].Id = id;
						}

					}
				}
				for (int i = 0; i < purchases.Count(); i++)
				{
					if (purchases[i].Id == id || purchases[i].Customer == null)
					{
						purchases.Remove(purchases[i]);
						i = -1;
					}
				}
			}


			if (purchases.Count() > 0)
			{
				_purchasesRepository.SavePurchases(purchases);
				return "Valores atualizados com sucesso";
			}
			return "Nenhum valor a ser atualizado";


		}
		public MemoryStream ReadStrem(IFormFile file)
		{
			using var stream = new MemoryStream();

			file?.CopyTo(stream);
			var byteArray = stream.ToArray();
			return new MemoryStream(byteArray);
		}

	}
}


