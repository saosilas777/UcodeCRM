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
			var date = DateTime.Now;
			List<CustomerModel> _customers = customers;
			foreach (var item in _customers)
			{
				item.LastUpdated = date.Date;
				var purchase = item.CustomerPurchases.LastOrDefault();
				if (purchase != null)
				{
					TimeSpan inactiveDays = date - purchase.PurchaseDate;

					if (inactiveDays.Days < 90)
					{
						item.Status = true;
					}
					else
					{
						item.Status = false;
					}
				}
				else
				{
					item.Status = false;
				}
				if (item.NextContactDate < date)
				{
					item.NextContactDate = DateTime.Parse(date.ToShortDateString() + " " +  item.NextContactDate.ToShortTimeString() + ":00");
				}






			}
			_customerRepository.AtualizarTodos(_customers);
		}



	}
}
