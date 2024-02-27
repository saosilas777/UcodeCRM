namespace CRM.Models
{
	public class AnalyticsModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public DateTime Registration { get; set; }
		public int TotalCustomers { get; set; }
		public int ActiveCustomers { get; set; }
		public int InactiveCustomers { get; set; }
		public double? TotalSalesMonth { get; set; }
		public double? BaseSalary { get; set; }
		public double? Commission { get; set; }
		public double? PaidWeeklyRest { get; set; }
		public double? TotalPayment { get; set; }
		public TotalAnnualSales? TotalAnnualSales { get; set; }
		public CustomersServedPerMonth? CustomersServedPerMonth { get; set; }

	}
}
