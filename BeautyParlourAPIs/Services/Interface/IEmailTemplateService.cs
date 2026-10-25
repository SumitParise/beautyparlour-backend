namespace BeautyParlourAPIs.Services.Interface
{
	public interface IEmailTemplateService
	{
		Task<string> GetTemplateAsync(string templateName, Dictionary<string, string> placeholders);

	}
}
