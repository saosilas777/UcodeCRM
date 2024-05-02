using System.ComponentModel.DataAnnotations;

namespace CRM.Models.ViewModels
{
	public class CustomerEditViewModel
	{
		[Key]
		public Guid Id { get; set; }
		public string Codigo { get; set; } = string.Empty;
		public string Cnpj { get; set; } = string.Empty;
		public string RazaoSocial { get; set; } = string.Empty;
		public bool Status { get; set; }
		public string Cidade { get; set; } = string.Empty;
		public string Uf { get; set; } = string.Empty;
		public List<EmailModel> Emails { get; set; } = new();
		public List<PhoneModel> Phones { get; set; } = new();
		public string Contact { get; set; }
		public DateTime LastPurchaseDate { get; set; } = DateTime.Now;
		public double LastPurchaseValue { get; set; } = 0;
		public List<ContactRecords> ContactRecords { get; set; } = new();
		public string NextContactDate { get; set; } = string.Empty;
		public Guid UserId { get; set; }
	}
}
