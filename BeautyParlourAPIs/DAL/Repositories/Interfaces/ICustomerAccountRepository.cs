using BeautyParlourAPIs.Models;

namespace BeautyParlourAPIs.DAL.Repositories.Interfaces
{
	public interface ICustomerAccountRepository
	{
		Task<bool> AddAccountAsync(CustomerAccount account);
		Task<CustomerAccount> GetAccountByIdAsync(int accountId);
		Task<IEnumerable<CustomerAccount>> GetAllAccountsAsync();
		Task<bool> UpdateAccountAsync(CustomerAccount account);
		Task<bool> DeleteAccountAsync(int accountId);
	}
}
