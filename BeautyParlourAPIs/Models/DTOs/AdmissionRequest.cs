namespace BeautyParlourAPIs.Models.DTOs
{
	public class AdmissionRequest
	{
		public string Name { get; set; } = "";
		public string MobileNo { get; set; } = "";
		public string Address { get; set; } = "";
		public string Email { get; set; } = "";

		public IFormFile? PassPortPhoto { get; set; }
		public IFormFile? AadhaarCard { get; set; }
		
		public IFormFile? RationCard { get; set; }
		public IFormFile? LeavingCertificate { get; set; }
		public string? OTP { get; set; } = ""; // OTP entered by user
	}
}
