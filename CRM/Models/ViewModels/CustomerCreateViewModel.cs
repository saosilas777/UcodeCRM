using System.ComponentModel.DataAnnotations;

namespace CRM.Models.ViewModels
{
    public class CustomerCreateViewModel
    {
		public string Codigo { get; set; } = string.Empty;
		public string Cnpj { get; set; } = string.Empty;
		public string RazaoSocial { get; set; } = string.Empty;
		public string Cidade { get; set; } = string.Empty;
		public string Contato { get; set; } = string.Empty;
		public string Cep { get; set; } = string.Empty;
		public string Uf { get; set; } = string.Empty;
		public string[] Emails { get; set; } = new string[3];
		public string[] Phones { get; set; } = new string[3];
	}
}
