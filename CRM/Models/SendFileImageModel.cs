namespace CRM.Models
{
	public class SendFileImageModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string Url { get; set; } = string.Empty;
	}
}
