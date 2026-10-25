using BeautyParlourAPIs.Models.DTOs;

namespace BeautyParlourAPIs.Services.Interface
{
	public interface IAuthService
	{
		Task<bool> Register(RegisterDto register);

		Task<AuthResponseDto> Login(LoginAdminDto login);
		Task<bool> ResetPassword(ForgetPasswordDto reset);
	}
}
