using Microsoft.EntityFrameworkCore;
using CRM.Data;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;
using CRM.Services;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Repository
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly IUserSession _session;

		private readonly Context _context;
		public CustomerRepository(Context context,
								IUserSession section)
		{
			_context = context;
			_session = section;
		}

		public void AddNewContact(List<CustomerModel> customers)
		{
			foreach (var item in customers)
			{
				for (int i = 0; i < 2; i++)
				{
					PhoneModel phone = new()
					{
						Customer = item,
						Phone = "",
						RegistrationDate = DateTime.Now

					};
					item.Phones.Add(phone);

				}
				for (int i = 0; i < 2; i++)
				{
					EmailModel mail = new()
					{
						Customer = item,
						Email = "",
						RegistrationDate = DateTime.Now

					};
					item.Emails.Add(mail);

				}
				_context.Customers.Update(item);
				_context.SaveChanges();


			}
		}

		public CustomerEditViewModel BuscarPorId(Guid id)
		{
			var custumerDb = _context.Customers.Include(x => x.ContactRecords).Include(x => x.Emails).Include(x => x.Phones).Include(x => x.CustomerPurchases).FirstOrDefault(x => x.Id == id);
			DateTime today = DateTime.Now;

			CustomerEditViewModel _CustomerEditView = new()
			{
				Id = custumerDb.Id,
				Codigo = custumerDb.Codigo,
				Cnpj = custumerDb.Cnpj.ToString(),
				RazaoSocial = custumerDb.RazaoSocial,
				Status = custumerDb.Status,
				Cidade = custumerDb.Cidade,
				Uf = custumerDb.Uf,
				Emails = custumerDb.Emails,
				Phones = custumerDb.Phones,
				Contact = custumerDb.Contact,
				ContactRecords = custumerDb.ContactRecords,
				NextContactDate = custumerDb.NextContactDate,
				UserId = custumerDb.UserId

			};

			if (custumerDb.CustomerPurchases.Count() > 0)
			{
				_CustomerEditView.LastPurchaseDate = custumerDb.CustomerPurchases.OrderBy(p => p.PurchaseDate).LastOrDefault().PurchaseDate;
				_CustomerEditView.LastPurchaseValue = custumerDb.CustomerPurchases.OrderBy(p => p.PurchaseDate).LastOrDefault().PurchaseValue;
				TimeSpan daysInactive = today - _CustomerEditView.LastPurchaseDate;
				_CustomerEditView.InactiveDays = daysInactive.Days;
			}




			return (_CustomerEditView);

		}
		public CustomerModel BuscarPorCodigo(int codigo)
		{
			return _context.Customers.FirstOrDefault(x => x.Codigo == codigo.ToString());
		}
		public List<CustomerModel> BuscarTodos(Guid id)
		{
			return _context.Customers.Include(x => x.Emails).Include(x => x.ContactRecords).Include(x => x.CustomerPurchases.OrderBy(x => x.PurchaseDate)).Include(x => x.Phones).Where(x => x.UserId == id).OrderBy(x => x.NextContactDate).AsNoTracking().ToList();
		}
		public List<CustomerModel> ListarTodos()
		{
			return _context.Customers.ToList();
		}

		public CustomerModel Create(CustomerCreateViewModel customer)
		{
			var hasCustomerDb = _context.Customers.FirstOrDefault(x => x.Codigo == customer.Codigo || x.Cnpj == customer.Cnpj);

			if (hasCustomerDb != null) return null;

			var user = _session.GetUserSection();

			List<EmailModel> emails = new List<EmailModel>();
			List<PhoneModel> phones = new List<PhoneModel>();

			string formatedCnpj = "";
			if (customer.Cnpj.Length < 18)
			{
				formatedCnpj = Convert.ToUInt64(customer.Cnpj).ToString(@"00\.000\.000\/0000\-00");
			}
			else
			{
				formatedCnpj = customer.Cnpj;
			}

			CustomerModel customerModel = new()
			{
				UserId = user.Id,
				Codigo = customer.Codigo,
				Cnpj = formatedCnpj,
				RazaoSocial = customer.RazaoSocial,
				Contact = customer.Contato,
				Cidade = customer.Cidade,
				Uf = customer.Uf,
				Priority = Enums.Priority.green,
				NextContactDate = DateTime.Now



			};
			for (int i = 0; i < customer.Emails.Count(); i++)
			{
				EmailModel email = new()
				{
					Email = customer.Emails[i],
					Customer = customerModel,
					RegistrationDate = DateTime.Now

				};

				emails.Add(email);

			}
			for (int i = 0; i < customer.Phones.Count(); i++)
			{
				PhoneModel phone = new()
				{
					Phone = customer.Phones[i],
					Customer = customerModel,
					RegistrationDate = DateTime.Now

				};

				phones.Add(phone);

			}
			customerModel.Emails = emails;
			customerModel.Phones = phones;



			_context.Add(customerModel);
			_context.SaveChanges();
			return customerModel;
		}
		public bool CreateAll(List<CustomerModel> customers)
		{

			_context.AddRange(customers);
			_context.SaveChanges();
			return true;
		}

		public bool Deletar(CustomerModel customer)
		{
			var dbCustomer = _context.Customers.Find(customer.Id);
			if (dbCustomer != null)
			{
				dbCustomer.UserId = new Guid("00000000-0000-0000-0000-000000000000");
				_context.Customers.Update(dbCustomer);
				_context.SaveChanges();
				return true;
			}
			return false;

		}

		public List<CustomerModel> CreateAt()
		{
			List<CustomerModel> _customers = new List<CustomerModel>();

			for (int i = 0; i < 20; i++)
			{
				CustomerModel customer = new CustomerModel
				{
					/*IdSense = new Guid().ToString().Substring(0, 6),
					IdStarford = new Guid().ToString().Substring(0, 6),
					Cnpj = "01.001.000/0001-01",
					RazaoSocial = $"Mercado do seu zé {i}",
					Status = true,
					Loja = $"Mercado do seu zé {i}",
					Cliente = $"Mercado do seu zé {i}",
					ProdutoFiscal = "MRR de E-Commerce",
					Fr = "MRR",
					ValorMensal = 599.99*/


				};
				var user = _session.GetUserSection();

				customer.UserId = user.Id;
				_customers.Add(customer);
			}



			CreateAll(_customers);
			return _customers;
		}

		public List<CustomerModel> AtualizarTodos(List<CustomerModel> customers)
		{
			_context.Customers.UpdateRange(customers);
			_context.SaveChanges();
			return customers;
		}

		public bool Atualizar(CustomerEditViewModel _customer)
		{
			try
			{
				var date = DateTime.Now;
				List<PhoneModel> phones = new();

				var user = _session.GetUserSection();

				var customerDb = _context.Customers.Include(x => x.CustomerPurchases).Include(x => x.Phones).Include(x => x.Emails).FirstOrDefault(x => x.Id == _customer.Id);

				customerDb.Codigo = _customer.Codigo;
				customerDb.Cnpj = _customer.Cnpj;
				customerDb.RazaoSocial = _customer.RazaoSocial;
				customerDb.Contact = _customer.Contact;
				customerDb.Status = _customer.Status;
				customerDb.Cidade = _customer.Cidade;
				customerDb.Uf = _customer.Uf;

				for (int i = 0; i < customerDb.Phones.Count(); i++)
				{
					customerDb.Phones[i].Phone = _customer.Phones[i].Phone;

				}
				for (int i = 0; i < customerDb.Emails.Count(); i++)
				{
					customerDb.Emails[i].Email = _customer.Emails[i].Email;

				}

				var purchase = customerDb.CustomerPurchases.FirstOrDefault(x => x.PurchaseValue == _customer.LastPurchaseValue && x.PurchaseDate == _customer.LastPurchaseDate);
				if (_customer.LastPurchaseValue > 0 && purchase == null)
				{
					PurchaseModel _purchase = new();
					customerDb.Status = true;
					_purchase.PurchaseValue = _customer.LastPurchaseValue;
					_purchase.PurchaseDate = _customer.LastPurchaseDate;
					_purchase.PurchaseValue = _customer.LastPurchaseValue;
					_purchase.CustomerCode = int.Parse(_customer.Codigo);
					_purchase.Customer = customerDb;
					_purchase.UserId = user.Id;
					_context.Purchases.Add(_purchase);
				}
				customerDb.NextContactDate = _customer.NextContactDate;

				_context.Customers.Update(customerDb);
				_context.SaveChanges();
				return true;

			}
			catch (Exception)
			{

				return false;
			}
		}
		public List<CustomerModel> AdicionarTodos(List<CustomerModel> customers)
		{
			_context.Customers.AddRange(customers);
			_context.SaveChanges();
			return customers;
		}
		public void ChangeAllNextContactsDates(string nextContactDate)
		{
			var user = _session.GetUserSection();
			var customers = _context.Customers.Where(x => x.UserId == user.Id).ToList();
			for (int i = 0; i < customers.Count(); i++)
			{
				if (customers[i].NextContactDate < DateTime.Parse(nextContactDate))
				{
					customers[i].NextContactDate = DateTime.Parse(nextContactDate);
				}
			}
			_context.UpdateRange(customers);
			_context.SaveChanges();

		}
		public CustomerModel BuscarCustomerPorId(Guid id)
		{
			var customer = _context.Customers.FirstOrDefault(x => x.Id == id);
			return customer;
		}
		public bool RegistrationContact(string anotation, string date, Guid id)
		{
			var _customer = BuscarCustomerPorId(id);
			ContactRecords contacts = new();
			contacts.Customer = _customer;
			contacts.Anotation = anotation;
			contacts.RegistrationDate = DateTime.Now;

			_customer.NextContactDate = DateTime.Parse(date);

			_context.Customers.Update(_customer);
			_context.ContactRecords.Add(contacts);
			_context.SaveChanges();

			return true;

		}
		public void NextContactRecord(string date, Guid id)
		{
			var customer = BuscarCustomerPorId(id);
			customer.NextContactDate = DateTime.Parse(date);

			_context.Customers.Update(customer);
			_context.SaveChanges();


		}

		public void ContactDateEdit(DateTime date, Guid id)
		{
			var customer = _context.Customers.Find(id);
			if (customer != null)
			{
				customer.NextContactDate = date;
				_context.SaveChanges();

			}
		}

		public List<CustomerModel> BuscarClientesDaAgendaAtual(Guid id)
		{
			var date = DateTime.Now;
			return _context.Customers.Include(x => x.Emails).Include(x => x.ContactRecords)
				.Include(x => x.CustomerPurchases).Include(x => x.Phones)
				.Where(x => x.UserId == id && x.NextContactDate <= date).OrderBy(x => x.NextContactDate).AsNoTracking().ToList();
		}

		public void ChangePriority(string priority, string priorityId)
		{
			var customer = BuscarCustomerPorId(Guid.Parse(priorityId));

			switch (priority)
			{
				case "green":
					customer.Priority = Enums.Priority.green;
					break;
				case "red":
					customer.Priority = Enums.Priority.red;
					break;
			}
			_context.Update(customer);
			_context.SaveChanges();
		}

		public void UpdateNextContactDates(List<UpdateNextContactDateViewModel> updates)
		{
			foreach (var item in updates)
			{
				var customer = BuscarCustomerPorId(Guid.Parse(item.CustomerId));
				if (customer != null)
				{
					customer.NextContactDate = DateTime.Parse(item.NextContactDate);
					_context.Update(customer);
					_context.SaveChanges();
				}

			}

		}

	}
}
