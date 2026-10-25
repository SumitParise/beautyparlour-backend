using Azure.Core;
using BeautyParlourAPIs.DAL.Repositories;
using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;
using Hangfire;

namespace BeautyParlourAPIs.Services
{
	public class AdmissionService: IAdmissionService
	{
		private readonly IAdmission _admission;
		private readonly IWebHostEnvironment _env;
		private readonly IEmailService _emailService;
		private readonly IEmailTemplateService _emailTemplateService;

		public AdmissionService(IAdmission admission, IWebHostEnvironment env, IEmailService emailService, IEmailTemplateService emailTemplateService)
		{
			_admission = admission;
			_env = env;
			_emailService = emailService;
			_emailTemplateService = emailTemplateService;
		}

		public async Task<Admission> Add(AdmissionRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentException("Name is required.");
			if (string.IsNullOrWhiteSpace(request.MobileNo) || request.MobileNo.Length != 10)
				throw new ArgumentException("Valid 10-digit MobileNo is required.");
			if(string.IsNullOrWhiteSpace(request.Address) || request.Address.Length<10)
				throw new ArgumentException("Valid Address is required (min 10 characters).");

			// ✅ Map DTO → Entity
			var admission = new Admission
			{
				Name = request.Name,
				MobileNo = request.MobileNo,
				Address = request.Address,
				Email= request.Email,
			};

			// ✅ Save
			await _admission.Add(admission);

			var placeholders = new Dictionary<string, string>
			{
				{ "Name", admission.Name }
			};

			var htmlBody = await _emailTemplateService.GetTemplateAsync("AdmissionSuccess", placeholders);

			// Schedule the email in background using Hangfire
			BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(
				admission.Email,
				"🌸 Admission Successful! 🌸",
				htmlBody
			));

			return admission; 
		}

		public async Task<bool> Delete(int id)
		{
			var existingAdmission = await _admission.GetByID(id);
			if (existingAdmission == null)
			{
				throw new KeyNotFoundException($"Admission with ID {id} not found.");
			}

			await _admission.Delete(id);
			return true;
		}

		public async Task<Admission> GetByID(int admissionID)
		{
			var admission = await _admission.GetByID(admissionID);
			if (admission == null)
			{
				throw new KeyNotFoundException($"Admission with ID {admissionID} not found.");
			}

			return admission;
		}

		public Task<IEnumerable<Admission>> GetListAsync()
		{
			return _admission.GetListAsync();
		}

		public async Task<string> SaveFile(IFormFile file,string Name)
		{
			var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var admissions = await _admission.GetListAsync();
			var studentName = admissions.FirstOrDefault(f => f.Name.Equals(Name));

			var document = file.Name;

			if(studentName == null) 
				throw new ArgumentException("Student not found with the given name.");

			var fileName = studentName.Name + "_" + document + Path.GetExtension(file.FileName);
			var filePath = Path.Combine(uploadsFolder, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return $"/uploads/{fileName}";
		}

		//file validation
		public async Task<bool> IsValidFile(IFormFile file)
		{
			if (file == null) return true; // Skip validation if not provided

			var allowedExtensions = new[] { ".jpg", ".png" };
			var allowedMimeTypes = new[] { "image/jpeg", "image/png" };

			var extension = Path.GetExtension(file.FileName).ToLower();
			var mimeType = file.ContentType.ToLower();

			return allowedExtensions.Contains(extension) && allowedMimeTypes.Contains(mimeType);
		}

		public async Task<bool> UpdatePath(int id,string aadharPath,string passPortPhotoPath,string rationCardPath, string leavingCert)
		{
			var result = await _admission.Update(id,aadharPath,passPortPhotoPath,rationCardPath,leavingCert);

			if(!result)
				throw new KeyNotFoundException($"Admission with ID {id} not found.");

			return result;
		}
	}
}
