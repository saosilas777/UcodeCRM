using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class LoginModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		[Required(ErrorMessage = "É preciso informar um login válido")]
		public string Login { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public UserModel User { get; set; } = new UserModel();

	}
}
