namespace BeautyParlourAPIs.Services.Interface
{
	public interface IOtpService
	{
		
			Task SendOTPAsync(string email);
			bool VerifyOTP(string email, string otp);

	}
}
