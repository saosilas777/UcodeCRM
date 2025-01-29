using OfficeOpenXml;
using System.ComponentModel;
using CRM.Interfaces;
using CRM.Models;
using LicenseContext = System.ComponentModel.LicenseContext;
using System.Collections.Generic;
using CRM.Models.ViewModels;
using CRM.Repository;
using CRM.Data;

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
			List<PurchaseModel> purchases = new List<PurchaseModel>();
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
						PurchaseModel purchase = new PurchaseModel();
						if (worksheet.Cells[row, 1].Value.ToString() != "")
						{
							var cnpj = worksheet.Cells[row, 3].Value.ToString();
							var customer = customers.Find(x => x.Cnpj == cnpj);
							purchase.Customer = customer;
							if (purchase.Customer != null)
							{
								purchase.CustomerCode = int.Parse(customer.Codigo);
								purchase.PurchaseDate = DateTime.Parse(worksheet.Cells[row, 1].Value.ToString());
								purchase.PurchaseValue = double.Parse(worksheet.Cells[row, 4].Value.ToString());
								purchase.UserId = user.Id;
								purchase.CustomerId = customer.Id;

								purchases.Add(purchase);
							}
							continue;
						}
						else
						{
							return "Documento contém células vazias";
						}

					};

					if (VerifyDuplicatedPurchases(purchases))
					{
						return "Valores atualizados com sucesso!";
					}
					return "Valores não foram atualizados, tente novamente!";


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
					_customerRepository.AdicionarTodos(customers);
					return "Valores atualizados com sucesso!";
				}

			}
		}

		public bool VerifyDuplicatedPurchases(List<PurchaseModel> purchases)
		{
			var purchasesDb = _purchasesRepository.GetPurchases();

			for (int i = 0; i < purchasesDb.Count(); i++)
			{
				for (int j = 0; j < purchases.Count(); j++)
				{
					if (purchasesDb[i].PurchaseDate == purchases[j].PurchaseDate
						&& purchasesDb[i].PurchaseValue == purchases[j].PurchaseValue)
					{
						purchases.Remove(purchases[j]);
					}
				}

			}
			
			if (purchases.Count() != 0)
			{
				_purchasesRepository.SavePurchases(purchases);
				return true;
			}
			return false;





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

		public MemoryStream ReadStrem(IFormFile file)
		{
			using var stream = new MemoryStream();

			file?.CopyTo(stream);
			var byteArray = stream.ToArray();
			return new MemoryStream(byteArray);
		}

	}
}


