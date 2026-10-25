using BCrypt.Net;
using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;
using BeautyParlourAPIs.Services.JwtSecurity;
using Microsoft.Win32;

namespace BeautyParlourAPIs.Services
{
	public class AuthService:IAuthService
	{

		private readonly IAuthRepository _authRepository;	
		private readonly Jwt _jwt;	

		public AuthService(IAuthRepository authRepository,Jwt jwt)
		{
			_authRepository = authRepository;
			_jwt= jwt;

		}

		public async Task<AuthResponseDto> Login(LoginAdminDto login)
		{
			var dto = new Admin
			{
				Email = login.Email,
				PasswordHash = login.Password
			};



			var admin = await _authRepository.Login(dto);
			if (admin == null || !BCrypt.Net.BCrypt.Verify(login.Password, admin.PasswordHash))
				throw new ArgumentException("Invalid username or password");

			var token = _jwt.GenerateJwtToken(admin);

			return new AuthResponseDto
			{
				Token = token,
				Email = admin.Email,
				FullName = admin.FullName
			};
		}

		public async Task<bool> Register(RegisterDto register)
		{
			try
			{
				var admin = new Admin
				{
					FullName = register.FullName,
					Email = register.Email,
					PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password)
				};

				return await _authRepository.Register(admin);

			}catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public async Task<bool> ResetPassword(ForgetPasswordDto reset)
		{
			if(reset.Email == null)
				throw new ArgumentException("Email is required");

			if(reset.Password != reset.ConfirmPassword)
				throw new ArgumentException("Passwords do not match");

			var admin = new Admin
			{
				Email = reset.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(reset.Password)
			};

			return await _authRepository.ResetPassword(admin);

		}
	}
}
