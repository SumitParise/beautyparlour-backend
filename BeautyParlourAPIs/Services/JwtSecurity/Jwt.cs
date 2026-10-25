using BeautyParlourAPIs.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.Services.JwtSecurity
{
	public class Jwt
	{
		private readonly IConfiguration _config;

		public Jwt(IConfiguration config)
		{
			_config = config;
		}


		public string GenerateJwtToken(Admin admin)
		{
			var jwtSettings = _config.GetSection("JwtSettings");
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]));

			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
			new Claim("AdminId", admin.AdminId.ToString()),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
