using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;

namespace BeautyParlourAPIs.Services
{
	public class CustomerAccountService : IAccountService
	{
		private readonly ICustomerAccountRepository _accountRepository;


		public CustomerAccountService(ICustomerAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<bool> AddAccountAsync(CustomerAccountRequest account)
		{
			if(string.IsNullOrWhiteSpace(account.CustomerName) || account.TotalAmount < 0 || account.PaidAmount < 0)
			{
				return false;
			}

			var newAccount = new CustomerAccount
			{
				CustomerName = account.CustomerName,
				TotalAmount = account.TotalAmount,
				PaidAmount = account.PaidAmount,
				paidDate = account.PaidAmount > 0 ? DateTime.Now : null
			};

			return await _accountRepository.AddAccountAsync(newAccount);

		}

		public async Task<bool> DeleteAccountAsync(int accountId)
		{
			var account = await _accountRepository.GetAccountByIdAsync(accountId);	
			if(account == null)
			{
				return false;
			}
			return await _accountRepository.DeleteAccountAsync(accountId);
		}

		public async Task<CustomerAccount?> GetAccountByIdAsync(int accountId)
		{
			var account = await _accountRepository.GetAccountByIdAsync(accountId);

			if(account == null)
			{
				return null;
			}
			return account;
		}

		public async Task<IEnumerable<CustomerAccount>> GetAllAccountsAsync()
		{
			var accounts = await _accountRepository.GetAllAccountsAsync();

			return accounts;

		}

		public async Task<bool> UpdateAccountAsync(CustomerAccountRequest account)
		{

			if (string.IsNullOrWhiteSpace(account.CustomerName) || account.TotalAmount < 0 || account.PaidAmount < 0)
			{
				return false;
			}

			var entity = new CustomerAccount
			{
				AccountID = account.AccountID,
				CustomerName = account.CustomerName,
				TotalAmount = account.TotalAmount,
				PaidAmount = account.PaidAmount,
				paidDate = account.PaidAmount > 0 ? DateTime.Now : null
			};

			

			var UpdatedEntity = await _accountRepository.UpdateAccountAsync(entity);
			return UpdatedEntity;

		}
	}
}
