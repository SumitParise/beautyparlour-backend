using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Models.DTOs;

namespace BeautyParlourAPIs.Services.Interface
{
	public interface IAdmissionService
	{
		Task<Admission> Add(AdmissionRequest admission);
		Task<bool> Delete(int id);
		Task<Admission> GetByID(int admissionID);
		Task<IEnumerable<Admission>> GetListAsync();

		Task<bool> IsValidFile(IFormFile file);
		Task<string> SaveFile(IFormFile file,string Name);
		Task<bool> UpdatePath(int id, string aadharPath, string passPortPhotoPath, string rationCardPath, string leavingCert);
	}
}
