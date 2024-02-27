using System.ComponentModel.DataAnnotations;
using CRM.Enums;

namespace CRM.Models
{
	public class UserModel
	{
		public Guid Id { get; set; } = new Guid();
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public Perfil Perfil { get; set; }
		public DateTime ResgiterData { get; set; }
		public DateTime LastUpdate { get; set; }

	}
}
