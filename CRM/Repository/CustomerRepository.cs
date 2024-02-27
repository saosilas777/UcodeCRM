using Microsoft.EntityFrameworkCore;
using CRM.Data;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;
using CRM.Services;

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
			var custumerDb = _context.Customers.Include(x => x.ContactRecords).Include(x => x.Emails).Include(x => x.Phones).FirstOrDefault(x => x.Id == id);

			string[] emails = new string[custumerDb.Emails.Count()];
			string[] phones = new string[custumerDb.Phones.Count()];
			string[] anotation = new string[custumerDb.ContactRecords.Count()];
			string[] recordsDate = new string[custumerDb.ContactRecords.Count()];

			for (int i = 0; i < custumerDb.Emails.Count(); i++)
			{
				emails[i] = custumerDb.Emails[i].Email;

			}
			for (int i = 0; i < custumerDb.Phones.Count(); i++)
			{
				phones[i] = custumerDb.Phones[i].Phone;

			}
			for (int i = 0; i < custumerDb.ContactRecords.Count(); i++)
			{
				anotation[i] = custumerDb.ContactRecords[i].Anotation;

			}
			for (int i = 0; i < custumerDb.ContactRecords.Count(); i++)
			{
				recordsDate[i] = custumerDb.ContactRecords[i].RegistrationDate.ToString();

			}


			CustomerEditViewModel _CustomerEditView = new()
			{
				Id = custumerDb.Id,
				Codigo = custumerDb.Codigo,
				Cnpj = custumerDb.Cnpj,
				RazaoSocial = custumerDb.RazaoSocial,
				Status = custumerDb.Status,
				Cidade = custumerDb.Cidade,
				Uf = custumerDb.Uf,
				Emails = emails,
				Phones = phones,
				Contact = custumerDb.Contact,
				//LastPurchaseDate = custumerDb.LastPurchaseDate,
				//LastPurchaseValue = custumerDb.LastPurchaseValue,
				ContactRecordsDate = recordsDate,
				ContactRecordsAnotation = anotation,
				NextContactDate = custumerDb.NextContactDate.ToShortDateString()

			};

			return (_CustomerEditView);

		}
		public CustomerModel BuscarPorCodigo(int codigo)
		{
			return _context.Customers.FirstOrDefault(x => x.Codigo == codigo.ToString());
		}
		public List<CustomerModel> BuscarTodos(Guid id)
		{
			return _context.Customers.Include(x => x.Emails).Include(x => x.ContactRecords).Include(x => x.CustomerPurchases).Include(x => x.Phones).Where(x => x.UserId == id).ToList();
		}
		public List<CustomerModel> ListarTodos()
		{
			return _context.Customers.ToList();
		}

		public bool Create(CustomerCreateViewModel customer)
		{
			var user = _session.GetUserSection();
			List<EmailModel> emails = new();
			EmailModel email = new();
			List<PhoneModel> phones = new();
			PhoneModel phone = new();



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
			foreach (var item in customer.Emails)
			{
				email.Email = item;
				email.Customer = customerModel;
				email.RegistrationDate = DateTime.Now;
				emails.Add(email);

			}
			foreach (var item in customer.Phones)
			{
				phone.Phone = item;
				phone.Customer = customerModel;
				phone.RegistrationDate = DateTime.Now;
				phones.Add(phone);

			}
			customerModel.Emails = emails;
			customerModel.Phones = phones;



			_context.Add(customerModel);
			_context.SaveChanges();
			return true;
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

		/*public bool Editar(_CustomerModel customer)
		{
			_CustomerModel customerDb = BuscarPorId(customer.Id);

			customerDb.Codigo = customer.Codigo;
			customerDb.RazaoSocial = customer.RazaoSocial;
			customerDb.Contact = customer.Contact;
			customerDb.Status = customer.Status;
			customerDb.LastPurchaseDate = customer.LastPurchaseDate;
			customerDb.LastPurchaseValue = customer.LastPurchaseValue;
			customerDb.ContactRecords = customer.ContactRecords;
			customerDb.NextContactDate = customer.NextContactDate;

			_context.Update(customerDb);
			_context.SaveChanges();
			return true;

		}*/
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
		
		public CustomerModel Atualizar(CustomerEditViewModel _customer)
		{
			var _customerDb = _context.Customers.FirstOrDefault(x => x.Id == _customer.Id);
			var _phonesDb = _context.Phones.Where(x => x.Customer == _customerDb).ToList();
			var _emailsDb = _context.Emails.Where(x => x.Customer == _customerDb).ToList();


			for (int i = 0; i < _customer.Phones.Length; i++)
			{
				_phonesDb[i].Phone = _customer.Phones[i];
			}

			
			for (int i = 0; i < _customer.Emails.Length; i++)
			{
				_emailsDb[i].Email = _customer.Emails[i];
			}

			
			_customerDb.Codigo = _customer.Codigo;
			_customerDb.Cnpj = _customer.Cnpj;
			_customerDb.RazaoSocial = _customer.RazaoSocial;
			_customerDb.Status = _customer.Status;
			_customerDb.Cidade = _customer.Cidade;
			_customerDb.Uf = _customer.Uf;
			_customerDb.Contact = _customer.Contact;
			//_customerDb.LastPurchaseDate = _customer.LastPurchaseDate;
			//_customerDb.LastPurchaseValue = _customer.LastPurchaseValue;
			_customerDb.NextContactDate = DateTime.Parse(_customer.NextContactDate);


			_context.Customers.Update(_customerDb);
			_context.SaveChanges();
			return _customerDb;
		}
		public List<CustomerModel> AdicionarTodos(List<CustomerModel> customers)
		{

			_context.Customers.AddRange(customers);
			_context.SaveChanges();
			return customers;
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

	}
}
