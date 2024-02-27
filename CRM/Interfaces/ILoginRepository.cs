using CRM.Models;
using CRM.Models.ViewModels;

namespace CRM.Interfaces
{
	public interface ILoginRepository
	{
		bool Create(LoginModel login);
		LoginModel BuscarPorLogin(string login);
		bool BuscarPorEmail(string email);
		LoginModel BuscarPorId(Guid id);
		void ChangePassword(LoginModel login);
		bool PasswordUpdate(string password, string email);

	}
}
