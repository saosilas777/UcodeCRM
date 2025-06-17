using CRM.Interfaces;
using CRM.Models;
using CRM.Repository;

namespace CRM.Services
{
	public class CustomerStatusVerify
	{
		private readonly ICustomerRepository _customerRepository;


		public CustomerStatusVerify(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}
		public void StatusVerify(List<CustomerModel> customers)
		{
			var today = DateTime.Today;

			List<CustomerModel> _customers = customers;
			foreach (var item in _customers)
			{
				var purchase = item.CustomerPurchases.LastOrDefault();
				if (purchase != null)
				{
					var inactiveDays = purchase.PurchaseDate - today;
					item.LastUpdated = today.Date;
					var newDate = $"{item.NextContactDate.ToString("yyyy-MM-dd")} 09:00:00";
					item.NextContactDate = DateTime.Parse(newDate);
					if (inactiveDays.Days > -90)
					{
						item.Status = true;
					}
					else
					{
						item.Status = false;
					}
					

				}


			}
			_customerRepository.AtualizarTodos(_customers);
		}
		


	}
}
