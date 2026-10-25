using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Models.DTOs;

namespace BeautyParlourAPIs.Services.Interface
{
	public interface IAccountService
	{
		Task<bool> AddAccountAsync(CustomerAccountRequest account);
		Task<bool> DeleteAccountAsync(int accountId);
		Task<CustomerAccount?> GetAccountByIdAsync(int accountId);
		Task<IEnumerable<CustomerAccount>> GetAllAccountsAsync();
		Task<bool> UpdateAccountAsync(CustomerAccountRequest account);
	}
}
