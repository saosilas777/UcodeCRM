using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class TotalAnnualSales
	{
		[Key]
		public Guid Id { get; set; }
		public double January { get; set; } = 0;
		public double February { get; set; } = 0;
		public double March { get; set; } = 0;
		public double April { get; set; } = 0;
		public double May { get; set; } = 0;
		public double June { get; set; } = 0;
		public double July { get; set; } = 0;
		public double August { get; set; } = 0;
		public double September { get; set; } = 0;
		public double Octuber { get; set; } = 0;
		public double November { get; set; } = 0;
		public double December { get; set; } = 0;
		public UserModel? User { get; set; }
	}
}
