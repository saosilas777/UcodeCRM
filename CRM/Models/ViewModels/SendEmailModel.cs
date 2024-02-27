using System.ComponentModel.DataAnnotations;

namespace CRM.Models.ViewModels
{
	public class SendEmailModel
	{
		[Required(ErrorMessage = "Informe um email válido")]
		public string Email { get; set; } = string.Empty;
	}
}