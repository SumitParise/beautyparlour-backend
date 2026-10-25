using BeautyParlourAPIs.Models;

namespace BeautyParlourAPIs.DAL.Repositories.Interfaces
{
	public interface IAuthRepository
	{
		Task<bool> Register(Admin admin);
		Task<Admin> Login(Admin admin);
		Task<bool> ResetPassword(Admin admin);
	}
}
