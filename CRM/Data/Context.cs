using Microsoft.EntityFrameworkCore;
using CRM.Models;

namespace CRM.Data
{
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options)
		{
		}
		public DbSet<UserModel> Users { get; set; }
		public DbSet<LoginModel> Login { get; set; }
		public DbSet<CustomerModel> Customers { get; set; }
		public DbSet<EmailModel> Emails { get; set; }
		public DbSet<PhoneModel> Phones { get; set; }
		public DbSet<CustomerPurchases> CustomerPurchases { get; set; }
		public DbSet<ContactRecords> ContactRecords { get; set; }
		public DbSet<SendFileImageModel> ImageUrl { get; set; }
		





	}
}