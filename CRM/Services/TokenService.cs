using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CRM.Models;
using CRM.Repository;
using CRM.Interfaces;

namespace CRM.Services
{

	public static class TokenService
	{

		public static string GenerateToken(UserModel user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(SettingsToken.Secret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.FirstName.ToString()),
					new Claim(ClaimTypes.Email, user.Email.ToString()),
					new Claim(ClaimTypes.Role, user.Perfil.ToString()),
				}),

				Expires = DateTime.UtcNow.AddHours(2),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),

				SecurityAlgorithms.HmacSha256Signature),

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);

		}
		public static string Authenticate(UserModel user)
		{
			var token = GenerateToken(user);

			return token;
		}
		public static bool TokenIsValid(string token)
		{
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SettingsToken.Secret));

			var handler = new JwtSecurityTokenHandler();

			try
			{
				var tokenValid = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					IssuerSigningKey = key

				};

				SecurityToken validateToken;

				handler.ValidateToken(token, tokenValid, out validateToken);
				
				return true;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}


		}

		public static UserModel GetDataInToken(string token)
		{
			/*if (!TokenIsValid(token))
				throw new Exception("Token is not valid.");*/

			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			try
			{
				if (securityToken != null)
				{
					Guid id = Guid.Parse(securityToken.Claims.First(x => x.Type == "nameid").Value);
					string role = securityToken.Claims.First(x => x.Type == "role").Value;
					string name = securityToken.Claims.First(x => x.Type == "unique_name").Value;
					string email = securityToken.Claims.First(x => x.Type == "email").Value;
					Enums.Perfil _role;
					if (role == "admin")
					{
						_role = Enums.Perfil.admin;
					}
					else
					{
						_role = Enums.Perfil.padrao;
					}


					UserModel tokenLinkConfigurationModel = new UserModel
					{
						Id = id,
						FirstName = name,
						Email = email,
						Perfil = _role

					};


					return tokenLinkConfigurationModel;
				}
				UserModel user = new();
				user.FirstName = "Teste";
				user.LastName = "Teste";
				user.Email = "teste@gmail.com";
				user.Perfil = Enums.Perfil.admin;
				user.Id = Guid.NewGuid();
				user.ResgiterData = DateTime.Now;
				user.LastUpdate = DateTime.Now;

				return user;
			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}
		}
	}
}
