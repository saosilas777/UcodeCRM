using System.Security.Cryptography;
using System.Text;

namespace CRM.Services
{
	public static class LoginServices
	{
		
		public static string SetPwdHash(string password)
		{
			return HashGeneration(password);
		}

		public static string PasswordGeneration(string password)
		{
			string newPwd = Guid.NewGuid().ToString().Substring(0, 8);
			password = HashGeneration(newPwd);
			return password;
		}

		public static string HashGeneration(string value)
		{
			var hash = SHA1.Create();
			var encoding = new ASCIIEncoding();
			var array = encoding.GetBytes(value);

			array = hash.ComputeHash(array);

			var strHexa = new StringBuilder();

			foreach (var item in array)
			{
				strHexa.Append(item.ToString("x2"));

			}
			return strHexa.ToString();
		}
	}
}
