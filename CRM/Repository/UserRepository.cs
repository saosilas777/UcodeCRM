using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CRM.Data;
using CRM.Interfaces;
using CRM.Models;
using CRM.Models.ViewModels;

namespace CRM.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly Context _context;

		public UserRepository(Context context)
		{
			_context = context;
		}

		public bool Create(LoginModel login)
		{
			
				LoginModel? LoginsDB = _context.Login.FirstOrDefault(x => x.Login == login.Login);
				if(LoginsDB == null)
				{
					login.User.Perfil = Enums.Perfil.padrao;
					login.User.ResgiterData = DateTime.Now;
					login.User.LastUpdate = DateTime.Now;

					_context.Login.Add(login);
					_context.SaveChanges();
					return true;
				}
			return false;
			
		}

		

		public UserModel BuscarPorEmail(string email)
		{
			var user = _context.Users.FirstOrDefault(x => x.Email == email);
			return user;
		}

		public UserModel BuscarPorId(Guid id)
		{
			return _context.Users.FirstOrDefault(x => x.Id == id);
		}

		public UserModel BuscarPorLogin(string login)
		{
			LoginModel? loginDB = _context.Login.FirstOrDefault(x => x.Login == login);
			if(loginDB != null)
			{
				UserModel? user = _context.Users.FirstOrDefault(x => x.Id == loginDB.UserId);
				return user;
			}

			return null;


		}

		public List<UserModel> BuscarTodos()
		{
			throw new NotImplementedException();
		}

		

		public bool Deletar(Guid id)
		{
			throw new NotImplementedException();
		}

		public UserModel Editar(UserModel contato)
		{
			throw new NotImplementedException();
		}

		
	}
}
