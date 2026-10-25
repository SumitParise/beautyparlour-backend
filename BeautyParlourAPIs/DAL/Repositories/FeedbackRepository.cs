using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.DAL.Repositories
{
	public class FeedbackRepository:IFeedbackRepository
	{
		private readonly AppDbContext _context;

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

		public async Task<bool> UpdateIsActive(int feedbackID)
		{
			var feedback = await _context.feedbacks.
							Where(f => f.FeedbackID == feedbackID)
							.FirstOrDefaultAsync();
			if (feedback == null)
				return false;

			feedback.IsActive = !feedback.IsActive;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
