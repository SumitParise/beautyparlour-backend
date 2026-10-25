using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautyParlourAPIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService auth)
		{
			_authService = auth;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginAdminDto login)
		{
			try
			{
				var result = await _authService.Login(login);
				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
			}
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto register)
		{
			try
			{
				var result = await _authService.Register(register);
				if (result)
					return Ok(new { message = "Registration successful" });
				else
					return BadRequest(new { message = "Registration failed" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("forget-password")]

		public async Task<IActionResult> ResetPassword(ForgetPasswordDto reset)
		{
			try
			{
				var result = await _authService.ResetPassword(reset);
				if (result)
					return Ok(new { message = "Password reset successful" });
				else
					return BadRequest(new { message = "Password reset failed" });

			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);

			}

		}
	}
}