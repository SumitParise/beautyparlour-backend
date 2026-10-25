using BeautyParlourAPIs.Models;

namespace BeautyParlourAPIs.DAL.Repositories.Interfaces
{
	public interface IAdmission
	{
		Task Add(Admission admission);
		Task Delete(int id);
		Task<Admission> GetByID(int admissionID);
		Task<IEnumerable<Admission>> GetListAsync();

		Task<bool> Update(int id, string aadharPath, string passPortPhotoPath, string rationCardPath, string leavingCert);

	}
}
