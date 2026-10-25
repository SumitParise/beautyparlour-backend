using BeautyParlourAPIs.DAL;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BeautyParlourAPIs.Services
{
	public class MobileOtpService : IMobileOtpService
	{
		private readonly AppDbContext _db;
		private readonly string _accountSid;
		private readonly string _authToken;
		private readonly string _fromNumber;
		private readonly int _expiryMinutes;
		private readonly int _resendCooldownSeconds;
		private readonly int _maxAttempts;
		private readonly string _hashKey;

		public MobileOtpService(AppDbContext db, IConfiguration config)
		{
			_db = db;
			_accountSid = config["Twilio:AccountSid"];
			_authToken = config["Twilio:AuthToken"];
			_fromNumber = config["Twilio:PhoneNumber"];

			_expiryMinutes = int.Parse(config["Otp:ExpiryMinutes"] ?? "5");
			_resendCooldownSeconds = int.Parse(config["Otp:ResendCooldownSeconds"] ?? "60");
			_maxAttempts = int.Parse(config["Otp:MaxAttempts"] ?? "5");
			_hashKey = config["Otp:HashKey"] ?? throw new InvalidOperationException("OTP hash key missing");

			TwilioClient.Init(_accountSid, _authToken);
		}


		public async Task<bool> IsMobileVerifiedAsync(string mobile)
		{
			return await _db.mobileOtps.AnyAsync(x => x.Mobile == mobile && x.IsUsed);
		}

		public async Task SendOtpAsync(string mobile)
		{
			// Normalize mobile number to E.164 format
			var normalizedMobile = NormalizeToE164(mobile);

			// check resend cooldown
			var last = await _db.mobileOtps
				.Where(m => m.Mobile == mobile)
				.OrderByDescending(m => m.CreatedAt)
				.FirstOrDefaultAsync();

			if (last != null && (DateTime.UtcNow - last.CreatedAt).TotalSeconds < _resendCooldownSeconds)
				throw new InvalidOperationException($"Please wait {_resendCooldownSeconds} seconds before requesting a new OTP.");

			var otp = new Random().Next(100000, 999999).ToString();
			var hash = HashOtp(otp);

			var message = await MessageResource.CreateAsync(
				body: $"Your verification code is {otp}. It will expire in {_expiryMinutes} minutes.",
				from: new PhoneNumber(_fromNumber),
				to: new PhoneNumber(normalizedMobile)
			);

			_db.mobileOtps.Add(new MobileOtp
			{
				Mobile = mobile,
				OtpHash = hash,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(_expiryMinutes),
				IsUsed = false,
				Attempts = 0,
				MessageSid = message.Sid
			});

			await _db.SaveChangesAsync();
		}

		public async Task<bool> VerifyOtpAsync(string mobile, string otp)
		{
			var record = await _db.mobileOtps
	.Where(x => x.Mobile == mobile && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
	.OrderByDescending(x => x.CreatedAt)
	.FirstOrDefaultAsync();

			if (record == null) return false;
			if (record.Attempts >= _maxAttempts) return false;

			if (HashOtp(otp) == record.OtpHash)
			{
				record.IsUsed = true;
				await _db.SaveChangesAsync();
				return true;
			}

			record.Attempts++;
			await _db.SaveChangesAsync();
			return false;
		}


		private string HashOtp(string otp)
		{
			using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hashKey));
			var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));
			return Convert.ToBase64String(hash);
		}

		private string NormalizeToE164(string phone)
		{
			phone = phone.Trim();
			// Remove leading 0 if present
			if (phone.StartsWith("0")) phone = phone.Substring(1);

			// Ensure it starts with +91 for India
			if (!phone.StartsWith("+")) phone = "+91" + phone;

			return phone;
		}

	}
}
