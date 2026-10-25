using BeautyParlourAPIs.DAL;
using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.DAL.Repositories
{
	public class AuthRepository:IAuthRepository
	{
		private readonly AppDbContext _context;

		public AuthRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Admin?> Login(Admin admin)
		{



			var existingAdmin =  await _context.admins
					.FirstOrDefaultAsync(a => a.Email == admin.Email);

			if (existingAdmin == null)
				throw new Exception("Invalid credentials");

			return existingAdmin;
		}

		public async Task<bool> Register(Admin admin)
		{
			var existingAdmin = await _context.admins
					.FirstOrDefaultAsync(a => a.Email == admin.Email);
			if (existingAdmin != null)
				throw new Exception("User already present");

			await _context.admins.AddAsync(admin);

			var result = await _context.SaveChangesAsync();

			return result > 0;

		}

		public async Task<bool> ResetPassword(Admin admin)
		{
			if(admin.Email == null)
				throw new Exception("Email is required");

			var existingAdmin = await _context.admins
					.FirstOrDefaultAsync(a => a.Email == admin.Email);

			if (existingAdmin == null)
			{
				throw new Exception("User not found");
			}
			existingAdmin.PasswordHash = admin.PasswordHash;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
