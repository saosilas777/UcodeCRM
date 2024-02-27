using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class CustomersServedPerMonth
	{
		[Key]
		public Guid Id { get; set; }
		public int January { get; set; }
		public int February { get; set; }
		public int March { get; set; }
		public int April { get; set; }
		public int May { get; set; }
		public int June { get; set; }
		public int July { get; set; }
		public int August { get; set; }
		public int September { get; set; }
		public int Octuber { get; set; }
		public int November { get; set; }
		public int December { get; set; }
		public UserModel? User { get; set; }
	}
}
