using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.DAL.Repositories
{
	public class FeedbackRepository:IFeedbackRepository,IDisposable
	{
		private readonly AppDbContext _context;
		private bool _disposed = false;

		public FeedbackRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Feedback feedback)
		{
			await _context.feedbacks.AddAsync(feedback);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var feedback = await _context.feedbacks.FindAsync(id);
			if (feedback != null)
			{
				_context.feedbacks.Remove(feedback);
				await _context.SaveChangesAsync();
			}
		}

	

		public async Task<IEnumerable<Feedback>> GetAllFeedbackAsync()
		{
			var feedbacks = await _context.feedbacks.ToListAsync();

			return feedbacks;
		}

		public async Task<Feedback?> GetByIdAsync(int id)
		{
			return await _context.feedbacks.FindAsync(id);
		}

		public async Task UpdateAsync(Feedback feedback)
		{
			_context.feedbacks.Update(feedback);
			await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); // Prevent finalizer from running
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// Dispose managed resources
					_context?.Dispose();
				}

				// Dispose unmanaged resources here if any

				_disposed = true;
			}
		}

		public async Task<bool> UpdateIsActive(int feedbackID)
		{
			var feedback = await _context.feedbacks.
							Where(f => f.FeedbackID == feedbackID)
							.FirstOrDefaultAsync();
			if (feedback == null)
				return false;

			// Toggle or set IsActive
			feedback.IsActive = !feedback.IsActive;

			// Save changes
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
