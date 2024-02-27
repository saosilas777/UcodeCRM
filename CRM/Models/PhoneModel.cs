namespace CRM.Models
{
	public class PhoneModel
	{
		public Guid Id { get; set; }
		public string? Phone { get; set; } = string.Empty;
		public DateTime? RegistrationDate { get; set; }
		public CustomerModel Customer { get; set; }
	}
}
