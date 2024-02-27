namespace CRM.Models
{
	public class ContactRecords
	{
		public Guid Id { get; set; }
		public string Anotation { get; set; } = string.Empty;
		public DateTime RegistrationDate { get; set; }
		public CustomerModel Customer { get; set; }
	}
}
