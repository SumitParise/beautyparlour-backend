namespace BeautyParlourAPIs.Services.Interface
{
	public interface IMobileOtpService
	{
		Task SendOtpAsync(string mobile);
	    Task<bool> VerifyOtpAsync(string mobile, string otp);
		Task<bool> IsMobileVerifiedAsync(string mobile);
	}
}
