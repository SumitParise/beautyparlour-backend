using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;
using System.Collections.Concurrent;

namespace BeautyParlourAPIs.Services
{
	public class OTPService : IOtpService
	{
		private static ConcurrentDictionary<string, (string otp, DateTime expiry)> _otpStore = new();
		private readonly IEmailService _emailService;
		private readonly IEmailTemplateService _emailTemplateService;
		public OTPService(IEmailService emailService, IEmailTemplateService emailTemplateService)
		{
			_emailService = emailService;
			_emailTemplateService = emailTemplateService;
		}

		public async Task SendOTPAsync(string email)
		{
			var otp = new Random().Next(100000, 999999).ToString();
			var expiry = DateTime.Now.AddMinutes(5);
			_otpStore[email] = (otp, expiry);
			var placeholders = new Dictionary<string, string>
			{
				{ "OTP", otp }
			};

			var htmlBody = await _emailTemplateService.GetTemplateAsync("OtpTemplate", placeholders);

			await _emailService.SendEmailAsync(
				email,
				"🌸 Your Verification OTP Code 🌸",
				htmlBody
			);
		}

		public bool VerifyOTP(string email, string otp)
		{
			if (_otpStore.TryGetValue(email, out var entry))
			{
				if (entry.otp == otp && entry.expiry > DateTime.Now)
				{
					_otpStore.TryRemove(email, out _);
					return true;
				}
			}
			return false;
		}
	}
}
