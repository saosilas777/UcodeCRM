using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class CustomerPurchases
	{
		[Key]
		public Guid Id { get; set; }
		public int CustomerCode { get; set; }
		public DateTime PurchaseDate { get; set; }
		public double PurchaseValue { get; set; }
		public CustomerModel Customer { get; set; }
		public Guid UserId { get; set; }
	}
}
