namespace CRM.Models
{
	public class EmailModel
	{
		public Guid Id { get; set; }
		public string? Email { get; set; } = string.Empty;
		public DateTime? RegistrationDate { get; set; }
		public CustomerModel Customer { get; set; }

	}
}
