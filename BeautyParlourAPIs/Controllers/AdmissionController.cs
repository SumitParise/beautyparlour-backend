using Azure;
using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautyParlourAPIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdmissionController : ControllerBase
	{
		private readonly IAdmissionService _admissionService;
		private readonly IOtpService _otpService;
		private readonly IMobileOtpService _mobileOtpService;
		public AdmissionController(IAdmissionService admissionService, IOtpService otpService,IMobileOtpService mobileOtp)
		{
			_admissionService = admissionService;
			_otpService = otpService;
			_mobileOtpService = mobileOtp;
		}

		[AllowAnonymous]
		[HttpPost("getotp")]
		public async Task<IActionResult> GetOTP([FromQuery] string email)
		{
			if (string.IsNullOrEmpty(email))
				return BadRequest("Email is required.");

			try
			{
				await _otpService.SendOTPAsync(email);
				var response = new ResponseDto
				{
					Message = "OTP sent to your email..!",
					IsSuccess = true
				};
				return Ok(response);
			}
			catch (Exception ex)
			{
				var response = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false
				};

				return StatusCode(500, response);
			}
		}


		[HttpPost]
		public async Task<IActionResult> AddAdmission([FromForm] AdmissionRequest request)
		{
			try
			{
				if (!_otpService.VerifyOTP(request.Email, request.OTP ?? ""))
				{
					var response = new ResponseDto
					{
						Message = "OTP is wrong or expired.",
						IsSuccess = false
					};

					return BadRequest(response);
				}

				if (!await _admissionService.IsValidFile(request.AadhaarCard) ||
					 ! await _admissionService.IsValidFile(request.RationCard) ||
					 ! await _admissionService.IsValidFile(request.LeavingCertificate))
				{
					return BadRequest(new ResponseDto
					{
						Message = "Only .jpg and .png files are allowed.",
						IsSuccess = false
					});
				}



				var admission = await _admissionService.Add(request);

				if (request.AadhaarCard != null)
					admission.AadhaarCardPath = await _admissionService.SaveFile(request.AadhaarCard,request.Name);

				if (request.PassPortPhoto != null)
					admission.Passportphotopath = await _admissionService.SaveFile(request.PassPortPhoto, request.Name);


				if (request.RationCard != null)
					admission.RationCardPath = await _admissionService.SaveFile(request.RationCard,request.Name);

				if (request.LeavingCertificate != null)
					admission.LeavingCertificatePath = await _admissionService.SaveFile(request.LeavingCertificate,request.Name);

				if (admission == null)
					return StatusCode(500, "Failed to add admission.");


				await _admissionService.UpdatePath(admission.AdmissionID, admission.AadhaarCardPath, admission.Passportphotopath, admission.RationCardPath, admission.LeavingCertificatePath);

				return Ok(admission);
			}
			catch (ArgumentException ex)
			{
				var Respose = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false,
				};

				return BadRequest(Respose);
			}
			catch (Exception e)
			{
				var Respose = new ResponseDto
				{
					Message = e.Message,
					IsSuccess = false,
				};

				return StatusCode(500, Respose);
			}
		}
		[HttpGet("list")]
		public async Task<IActionResult> GetList()
		{
			try
			{
				var admissions = await _admissionService.GetListAsync();
				if (admissions != null)
				return Ok(admissions);
				else
					return NotFound("No admissions found.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while retrieving admissions.");
			}
		}
		[HttpPost("getbyid")]
		public async Task<IActionResult> GetByID([FromBody] int id)
		{
			try
			{
				var admission = await _admissionService.GetByID(id);
				if (admission == null)
				{
					return NotFound("Admission not found.");
				}
				return Ok(admission);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while retrieving the admission.");
			}
		}

		[HttpPost("delete")]
		public async Task<IActionResult> Delete([FromBody] int id)
		{
			try
			{
				var result = await _admissionService.Delete(id);
				if (result)
				{
					var response = new ResponseDto
					{
						Message = "Admission deleted successfully.",
						IsSuccess = true
					};

					return Ok(response);
				}
				else {
					var response = new ResponseDto
					{
						Message = "Admission not found.",
						IsSuccess = false
					};
					return NotFound(response);
				}
				
			}
			catch (KeyNotFoundException ex)
			{
				var response = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false
				};
				return NotFound(response);
			}
			catch (Exception ex)
			{
				var response = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false
				};

				return StatusCode(500, response);
			}


		}
	

	}
}
