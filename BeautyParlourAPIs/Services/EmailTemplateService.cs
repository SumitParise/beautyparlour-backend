using BeautyParlourAPIs.Services.Interface;

namespace BeautyParlourAPIs.Services
{
	public class EmailTemplateService : IEmailTemplateService
	{
		private readonly IWebHostEnvironment _env;

		public EmailTemplateService(IWebHostEnvironment env)
		{
			_env = env;
		}

		public async Task<string> GetTemplateAsync(string templateName, Dictionary<string, string> placeholders)
		{
			var templatePath = Path.Combine(_env.ContentRootPath, "EmailTemplates", $"{templateName}.html");

			if (!File.Exists(templatePath))
				throw new FileNotFoundException($"Email template '{templateName}.html' not found.");

			var html = await File.ReadAllTextAsync(templatePath);

			// Replace placeholders like {{Name}} or {{OTP}}
			foreach (var placeholder in placeholders)
			{
				html = html.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
			}

			return html;
		}
	}
}
