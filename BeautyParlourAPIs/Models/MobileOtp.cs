using System.ComponentModel.DataAnnotations;

namespace BeautyParlourAPIs.Models
{
	public class MobileOtp
	{
		
		public int Id { get; set; }

		[Required]
		public string Mobile { get; set; } = null!;
		public string OtpHash { get; set; } = null!;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime ExpiresAt { get; set; }
		public bool IsUsed { get; set; }
		public int Attempts { get; set; }
		public string? MessageSid { get; set; } // Twilio SMS message id
	}
}
