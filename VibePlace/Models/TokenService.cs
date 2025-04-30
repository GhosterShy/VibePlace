using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VibePlace.Data;

namespace VibePlace.Models
{
	public class TokenService
	{
		private readonly IConfiguration _configuration;
		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<string> GenerateAccessToken(AppUser user)
		{
			
			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


			var token = new JwtSecurityToken(

				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(
					Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
				signingCredentials: creds
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
