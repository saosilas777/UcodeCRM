using System.ComponentModel.DataAnnotations;

namespace CRM.Models.ViewModels
{
	public class ChangePasswordViewModel
	{
		public Guid UserId { get; set; }
		public string OldPassword { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
		[Compare(nameof(NewPassword), ErrorMessage = "As senhas acima não conferem")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
