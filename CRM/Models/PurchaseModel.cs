using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models
{
	public class PurchaseModel
	{
		[Key]
		public Guid Id { get; set; }
		public int CustomerCode { get; set; }
		public DateTime PurchaseDate { get; set; }
		public double PurchaseValue { get; set; }
		public Guid UserId { get; set; }

		[ForeignKey("CustomerModel")]
		public Guid CustomerId { get; set; }
		public virtual CustomerModel Customer { get; set; }
	}
}
