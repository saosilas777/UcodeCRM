using CRM.Models;

namespace CRM.Interfaces
{
	public interface IUserSession
	{
		void UserSectionCreate(UserModel user);
		void UserSectionRemove();
		UserModel GetUserSection();
	}
}
