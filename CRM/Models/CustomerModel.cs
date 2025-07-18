﻿using CRM.Enums;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class CustomerModel
	{
		[Key]
		public Guid Id { get; set; }
		public string Codigo { get; set; } = string.Empty;
		public string Cnpj { get; set; } = string.Empty;
		public string RazaoSocial { get; set; } = string.Empty;
		public bool Status { get; set; }
		public string Cidade { get; set; } = string.Empty;
		public string Uf { get; set; } = string.Empty;
		public List<EmailModel> Emails { get; set; }
		public List<PhoneModel> Phones { get; set; }
		public string Contact { get; set; } = string.Empty;
		public Priority Priority { get; set; }
		public List<PurchaseModel>? CustomerPurchases { get; set; }
		public List<ContactRecords>? ContactRecords { get; set; }
		public DateTime NextContactDate { get; set; }
		public Guid UserId { get; set; }
		public DateTime LastUpdated { get; set; }

	}
}
