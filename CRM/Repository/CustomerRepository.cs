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

		public CustomerEditViewModel BuscarPorId(Guid id)
		{
			var custumerDb = _context.Customers.Include(x => x.ContactRecords).Include(x => x.CustomerPurchases.OrderByDescending(x=>x.PurchaseDate)).Include(x => x.Emails).Include(x => x.Phones).FirstOrDefault(x => x.Id == id);

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
				NextContactDate = custumerDb.NextContactDate.ToShortDateString()

			};
			if (custumerDb.CustomerPurchases.Count() > 0)
			{
				_CustomerEditView.LastPurchaseDate = custumerDb.CustomerPurchases[0].PurchaseDate;
				_CustomerEditView.LastPurchaseValue = custumerDb.CustomerPurchases[0].PurchaseValue;

			}

			return (_CustomerEditView);

		}
		public CustomerModel BuscarPorCodigo(int codigo)
		{
			return _context.Customers.FirstOrDefault(x => x.Codigo == codigo.ToString());
		}
		public List<CustomerModel> BuscarTodos(Guid id)
		{
			return _context.Customers.Include(x => x.Emails).Include(x => x.ContactRecords).Include(x => x.CustomerPurchases.OrderBy(x => x.PurchaseDate)).Include(x => x.Phones).Where(x => x.UserId == id).OrderByDescending(x => x.NextContactDate).AsNoTracking().ToList();
		}
		public List<CustomerModel> ListarTodos()
		{
			return _context.Customers.ToList();
		}

		public string Create(CustomerCreateViewModel customer)
		{
			var hasCustomerDb = _context.Customers.FirstOrDefault(x => x.Codigo == customer.Codigo || x.Cnpj == customer.Cnpj);

			if (hasCustomerDb != null) return "Código ou CNPJ já existente!";

			var user = _session.GetUserSection();

			List<EmailModel> emails = new List<EmailModel>();
			List<PhoneModel> phones = new List<PhoneModel>();



			CustomerModel customerModel = new()
			{
				UserId = user.Id,
				Codigo = customer.Codigo,
				Cnpj = customer.Cnpj,
				RazaoSocial = customer.RazaoSocial,
				Contact = customer.Contato,
				Cidade = customer.Cidade,
				Uf = customer.Uf


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
			return "Cadastro realizado com sucesso!";
		}
		public bool CreateAll(List<CustomerModel> customers)
		{

			_context.AddRange(customers);
			_context.SaveChanges();
			return true;
		}

		public bool Deletar(CustomerModel customer)
		{
			_context.Remove(customer);
			_context.SaveChanges();
			return true;
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
				List<EmailModel> emails = new();
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
				//TODO
				//Ajustar campos de emails e telefones para varios inputs

				if(customerDb.Phones.Count() < _customer.Phones.Count())
				{
					foreach (var item in _customer.Phones)
					{
						PhoneModel phone = new()
						{
							Customer = customerDb,
							Phone = item.Phone,
							RegistrationDate = DateTime.Now
						};
						phones.Add(phone);
					}
					customerDb.Phones = phones;
				}

				if (customerDb.Emails.Count() < _customer.Emails.Count())
				{
					foreach (var item in _customer.Emails)
					{
						EmailModel mail = new()
						{
							Customer = customerDb,
							Email = item.Email,
							RegistrationDate = DateTime.Now
						};
						emails.Add(mail);
					}
					customerDb.Emails = emails;
				}
				
				

				var purchase = customerDb.CustomerPurchases.FirstOrDefault(x => x.PurchaseValue == _customer.LastPurchaseValue && x.PurchaseDate == _customer.LastPurchaseDate);
				if (_customer.LastPurchaseValue > 0 && purchase != null || _customer.LastPurchaseValue != 0)
				{
					PurchaseModel _purchase = new();
					_purchase.PurchaseValue = _customer.LastPurchaseValue;
					_purchase.PurchaseDate = _customer.LastPurchaseDate;
					_purchase.PurchaseValue = _customer.LastPurchaseValue;
					_purchase.CustomerCode = int.Parse(_customer.Codigo);
					_purchase.Customer = customerDb;
					_purchase.UserId = user.Id;
					_context.Purchases.Add(_purchase);
				}
				customerDb.NextContactDate = DateTime.Parse(_customer.NextContactDate);

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
			for(int i = 0; i < customers.Count();i++)
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
		public List<CustomerModel> GetByStatus(string status)
		{
			bool _status;

			if (status.ToUpper() == "ATIVO")
			{
				_status = true;
			}
			else
			{
				_status = false;
			}
			var customers = _context.Customers.Include(x => x.CustomerPurchases).Include(x => x.Emails).Include(x => x.Phones).Include(x => x.ContactRecords).Where(x => x.Status == _status).ToList();
			return customers;
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
				case "green": customer.Priority = Enums.Priority.green;
					break;
				case "yellow": customer.Priority = Enums.Priority.yellow;
					break;
				case "red": customer.Priority = Enums.Priority.red;
					break;
			}
			_context.Update(customer);
			_context.SaveChanges();
		}

	}
}
