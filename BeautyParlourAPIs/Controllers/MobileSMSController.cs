using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautyParlourAPIs.Controllers
{
	[Route("api/mobile/[controller]")]
	[ApiController]
	public class MobileSMSController : ControllerBase
	{
		private readonly IMobileOtpService _otpService;
		public MobileSMSController(IMobileOtpService otpService) => _otpService = otpService;

		[HttpPost("send-otp")]
		public async Task<IActionResult> SendOtp([FromBody] string mobile)
		{
			if (string.IsNullOrWhiteSpace(mobile)) return BadRequest("Mobile is required.");

			try
			{
				await _otpService.SendOtpAsync(mobile);
				return Ok(new { Message = "OTP sent successfully." });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "Failed to send OTP: " + ex.Message });
			}
		}

		[HttpPost("verify-otp")]
		public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Mobile) || string.IsNullOrWhiteSpace(request.Otp))
				return BadRequest("Mobile and OTP are required.");

			var verified = await _otpService.VerifyOtpAsync(request.Mobile, request.Otp);
			if (!verified) return BadRequest(new { Message = "Invalid or expired OTP." });

			return Ok(new { Message = "Mobile verified successfully." });
		}
	}
}
