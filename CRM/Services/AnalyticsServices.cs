using CRM.Data;
using CRM.Interfaces;
using CRM.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace CRM.Services
{
	public class AnalyticsServices
	{
		private readonly Context _context;
		private readonly IUserSession _session;

		public AnalyticsServices(Context context, IUserSession section)
		{
			_context = context;
			_session = section;
		}
		public AnalyticsModel AnalyticsBuilder(int month)
		{
			var user = _session.GetUserSection();
			if (user == null) return new AnalyticsModel();

			DateTime date = DateTime.Now;
			string initialDate = "";
			string finalDate = "";

			if (month == 0)
			{
				if (date.Month == 12 && date.Day > 20)
				{
					initialDate = $"{date.Year}/{date.Month}/21";
					finalDate = $"{date.Year + 1}/01/20";

				}
				else if (date.Month == 12 && date.Day < 21)
				{
					initialDate = $"{date.Year}/{date.Month - 1}/21";
					finalDate = $"{date.Year}/{date.Month}/20";

				}
				else if (date.Month == 1 && date.Day < 21)
				{
					initialDate = $"{date.Year - 1}/12/21";
					finalDate = $"{date.Year}/01/20";

				}
				else if (date.Day < 21)
				{
					initialDate = $"{date.Year}/{date.Month - 1}/21";
					finalDate = $"{date.Year}/{date.Month}/20";

				}
				else
				{
					initialDate = $"{date.Year}/{date.Month}/21";
					finalDate = $"{date.Year}/{date.Month + 1}/20";

				}
			}
			else
			{
				if (month == 12)
				{
					initialDate = $"{date.Year}/{month}/21";
					finalDate = $"{date.Year + 1}/01/20";

				}
				else if (month == 1)
				{
					initialDate = $"{date.Year - 1}/12/21";
					finalDate = $"{date.Year}/01/20";

				}
				else
				{
					initialDate = $"{date.Year}/{month - 1}/21";
					finalDate = $"{date.Year}/{month}/20";

				}
			}

			List<CustomerModel> customer = _context.Customers.Include(x => x.CustomerPurchases).Where(x => x.UserId == user.Id).ToList();

			double total = 0;
			int active = 0;
			int inactive = 0;

			for (int i = 0; i < customer.Count(); i++)
			{
				if (customer[i].CustomerPurchases.Count() > 0)
				{
					for (int j = 0; j < customer[i].CustomerPurchases.Count(); j++)
					{
						if (customer[i].CustomerPurchases[j].PurchaseDate >= DateTime.Parse(initialDate) && customer[i].CustomerPurchases[j].PurchaseDate <= DateTime.Parse(finalDate))
						{

							total += customer[i].CustomerPurchases[j].PurchaseValue;
							customer[i].Status = true;
						}


					}

				}

				if (customer[i].Status == true)
				{
					active++;
				}
				else
				{
					inactive++;
				}

			}

			double commission = 0;
			switch (total)
			{
				case <= 39999:
					commission = total * 0.010;
					break;
				case <= 49999:
					commission = total * 0.011;
					break;
				case <= 59999:
					commission = total * 0.012;
					break;
				case <= 69999:
					commission = total * 0.013;
					break;
				case <= 79999:
					commission = total * 0.014;
					break;
				case <= 89999:
					commission = total * 0.015;
					break;
				case <= 99999:
					commission = total * 0.016;
					break;
				case <= 109999:
					commission = total * 0.017;
					break;
				case <= 119999:
					commission = total * 0.018;
					break;
				case <= 129999:
					commission = total * 0.019;
					break;
				case > 129999:
					commission = total * 0.020;
					break;
			}

			DateTime currentDate = new DateTime(date.Year, date.Month, 1);

			int currentMonth = currentDate.Month;
			int workDays = 0;
			int daysInTheMonth = 0;
			int sundaysOrHollyday = 0;
			

			while (currentDate.Month == currentMonth)
			{
				daysInTheMonth++;
				if (IsHollyday(currentDate))
				{
					sundaysOrHollyday++;
				}
				if (currentDate.DayOfWeek.ToString() == "Sunday")
				{
					sundaysOrHollyday++;
				}
				
				currentDate = currentDate.AddDays(1);
			}
			workDays = daysInTheMonth - sundaysOrHollyday;
			double pwr = (commission / workDays) * sundaysOrHollyday;
			AnalyticsModel analytics = new();

			analytics.TotalSalesMonth = total;
			analytics.TotalCustomers = customer.Count();
			analytics.ActiveCustomers = active;
			analytics.InactiveCustomers = inactive;

			analytics.BaseSalary = 2155;
			analytics.Commission = double.Parse(commission.ToString("F2"));
			analytics.PaidWeeklyRest = double.Parse(pwr.ToString("F2"));
			analytics.TotalPayment = 2155 + commission + pwr;
			if (month == 0)
			{
				if (date.Day < 21) month = date.Month;
				if (date.Day > 20) month = date.Month + 1;
			}
			analytics.CurrentMonth = month;

			TotalAnnualSales totalAnnualSales = new TotalAnnualSales();

			List<PurchaseModel> purchases = new();

			for (int i = 0; i < customer.Count(); i++)
			{
				for (int j = 0; j < customer[i].CustomerPurchases.Count(); j++)
				{
					purchases.Add(customer[i].CustomerPurchases[j]);
				}
			}

			#region Canvas
			var year = date.Year;
			List<CustomerModel> _customers = new();
			CustomersServedPerMonth customersServed = new();

			foreach (var item in purchases)
			{
				if (item.PurchaseDate >= DateTime.Parse($"{year - 1}-12-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-01-20"))
				{
					totalAnnualSales.January += item.PurchaseValue;

					var _customer = customer.Find(x => x.Id == item.Id);
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.January = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-01-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-02-20"))
				{
					totalAnnualSales.February += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.February = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-02-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-03-20"))
				{
					totalAnnualSales.March += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.March = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-03-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-04-20"))
				{
					totalAnnualSales.April += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.April = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-04-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-05-20"))
				{
					totalAnnualSales.May += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.May = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-05-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-06-20"))
				{
					totalAnnualSales.June += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.June = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-06-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-07-20"))
				{
					totalAnnualSales.July += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.July = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-07-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-08-20"))
				{
					totalAnnualSales.August += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.August = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-08-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-09-20"))
				{
					totalAnnualSales.September += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.September = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-09-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-10-20"))
				{
					totalAnnualSales.Octuber += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.Octuber = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-10-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-11-20"))
				{
					totalAnnualSales.November += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.November = _customers.Count();
					}
				}

			}
			_customers.Clear();
			foreach (var item in purchases)
			{

				if (item.PurchaseDate >= DateTime.Parse($"{year}-11-21")
				&& item.PurchaseDate <= DateTime.Parse($"{year}-12-20"))
				{
					totalAnnualSales.December += item.PurchaseValue;
					if (!_customers.Contains(item.Customer))
					{
						_customers.Add(item.Customer);
						customersServed.December = _customers.Count();
					}
				}

			}
			#endregion
			analytics.TotalAnnualSales = totalAnnualSales;
			analytics.CustomersServedPerMonth = customersServed;

			return analytics;
		}

		public bool IsHollyday(DateTime day)
		{
			day = DateTime.Parse(day.ToShortDateString());
			#region dateArray
			string[] hollydays = new string[12];
			hollydays[0] = "01/01/2025";
			hollydays[1] = "18/04/2025";
			hollydays[2] = "21/04/2025";
			hollydays[3] = "01/05/2025";
			hollydays[4] = "19/06/2025";
			hollydays[5] = "09/07/2025";
			hollydays[6] = "07/09/2025";
			hollydays[7] = "12/10/2025";
			hollydays[8] = "02/11/2025";
			hollydays[9] = "15/11/2025";
			hollydays[10] = "20/11/2025";
			hollydays[11] = "25/12/2025";


			#endregion

			for (int i = 0; i < hollydays.Length;i++)
			{
				if (DateTime.Parse(hollydays[i]) == day)
				{
					return true;
				}

			}
			return false;
		}

	}

	
	
}
