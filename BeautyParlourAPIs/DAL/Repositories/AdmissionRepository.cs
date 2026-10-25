using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BeautyParlourAPIs.DAL.Repositories
{
	public class AdmissionRepository: IAdmission
	{

		private readonly AppDbContext _context;

		public AdmissionRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task Add(Admission admission)
		{
			try
			{
				await _context.admissions.AddAsync(admission);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				// Check if inner exception contains "UNIQUE" constraint violation
				if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_admissions_Email"))
				{
					throw new ArgumentException("The email is already registered.");
				}
				else if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_admissions_MobileNo"))
				{
					throw new ArgumentException("The mobile number is already registered.");
				}

				// Other DB exceptions
				throw;
			}
		}

		public async Task Delete(int id)
		{
			var admission = await _context.admissions.FindAsync(id);

			if (admission != null)
			{
				_context.admissions.Remove(admission);
				await _context.SaveChangesAsync();
			}

		}

		public async Task<Admission> GetByID(int admissionID)
		{
			var admission = await _context.admissions.FindAsync(admissionID);

			if (admission != null)
			{
				return admission;
			}

			return null;

		}

		public async Task<IEnumerable<Admission>> GetListAsync()
		{
			var admission =await _context.admissions.ToListAsync();

			return admission;

		}

		public async Task<bool> Update(int id,string aadharPath, string passPortPhotoPath, string rationCardPath,string leavingCert)
		{
			var existingAdmission = await _context.admissions.FindAsync(id);
			if (existingAdmission != null)
			{
				existingAdmission.AadhaarCardPath = aadharPath;
				existingAdmission.Passportphotopath = passPortPhotoPath;
				existingAdmission.LeavingCertificatePath = leavingCert;
				existingAdmission.RationCardPath = rationCardPath;
				_context.admissions.Update(existingAdmission);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

	}
}
