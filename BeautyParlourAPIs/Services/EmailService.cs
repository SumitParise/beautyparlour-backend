using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BeautyParlourAPIs.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSetting _emailSettings;
		private readonly ILogger<EmailService> _logger;

		public EmailService(IOptions<EmailSetting> emailSettings, ILogger<EmailService> logger)
		{
			_emailSettings = emailSettings.Value;
			_logger = logger;
		}

		public async Task SendEmailAsync(string to, string subject, string htmlBody)
		{
			try
			{
				using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
				{
					Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
					EnableSsl = true
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress(_emailSettings.Email, "Beauty Parlour"), // Optional: add display name
					Subject = subject,
					Body = htmlBody,
					IsBodyHtml = true
				};

				mailMessage.To.Add(to);

				_logger.LogInformation("Attempting to send email to {Recipient} with subject '{Subject}'", to, subject);

				await client.SendMailAsync(mailMessage);

				_logger.LogInformation("Email successfully sent to {Recipient}", to);
			}
			catch (SmtpException ex)
			{
				_logger.LogError(ex, "SMTP error while sending email to {Recipient}: {Message}", to, ex.Message);
				throw new ApplicationException("An error occurred while sending the email via SMTP.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error while sending email to {Recipient}: {Message}", to, ex.Message);
				throw new ApplicationException("An unexpected error occurred while sending the email.", ex);
			}
		}
	}
}
