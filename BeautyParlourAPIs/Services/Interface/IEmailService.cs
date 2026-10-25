namespace BeautyParlourAPIs.Services.Interface
{
	public interface IEmailService
	{
		Task SendEmailAsync(string to, string subject, string htmlBody);
	}
}
