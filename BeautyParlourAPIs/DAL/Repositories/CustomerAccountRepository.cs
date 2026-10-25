using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.DAL.Repositories
{
	public class CustomerAccountRepository : ICustomerAccountRepository
	{
		private readonly AppDbContext _context;

		public CustomerAccountRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AddAccountAsync(CustomerAccount account)
		{
			
			await _context.accounts.AddAsync(account);

			var result = await _context.SaveChangesAsync();
			return result > 0;
		}

		public async Task<bool> DeleteAccountAsync(int accountId)
		{
			var getAccount = await _context.accounts.FindAsync(accountId);

			if (getAccount == null)
			{
				return false;
			}
			_context.accounts.Remove(getAccount);
			var result = await _context.SaveChangesAsync();
			return result>0;
		}

		public async Task<CustomerAccount?> GetAccountByIdAsync(int accountId)
		{
			return await _context.accounts.FindAsync(accountId);


		}

		public async Task<IEnumerable<CustomerAccount>> GetAllAccountsAsync()
		{
			var accounts = await _context.accounts.ToListAsync();
			return accounts;
		}

		
		public async Task<bool> UpdateAccountAsync(CustomerAccount account)
		{
			var existingAccount = await _context.accounts
					.FirstOrDefaultAsync(a => a.AccountID == account.AccountID);


			if (existingAccount == null)
			{
				return false;
			}
			existingAccount.CustomerName = account.CustomerName;
			existingAccount.TotalAmount = account.TotalAmount;
			existingAccount.PaidAmount = account.PaidAmount;
			existingAccount.paidDate = account.PaidAmount > 0 ? DateTime.Now : null;

			var result = await _context.SaveChangesAsync();

			return result>0;

		}
	}
}
