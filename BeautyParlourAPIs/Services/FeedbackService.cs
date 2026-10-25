using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.Services
{
	public class FeedbackService:IFeedback
	{
		private readonly IFeedbackRepository _feedbackRepository;

		public FeedbackService(IFeedbackRepository feedbackRepository)
		{
			_feedbackRepository = feedbackRepository;
		}

		public Task Add(string customerName, int rating, string comments)
		{
			if (string.IsNullOrWhiteSpace(customerName))
			{
				throw new ArgumentException("Customer name cannot be null or empty.", nameof(customerName));
			}
			if (rating < 1 || rating > 5)
			{
				throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
			}
			if (string.IsNullOrWhiteSpace(comments))
			{

				throw new ArgumentException("Comments cannot be null or empty.", nameof(comments));
			}

			var feedback = new Feedback
			{
				CustomerName = customerName,
				Ratings = rating,
				Comment = comments,
				FeedbackDate = DateTime.Now
			};

			return _feedbackRepository.AddAsync(feedback);

		}

		public async Task Delete(int id)
		{
			var feedback = await _feedbackRepository.GetByIdAsync(id);

			if (feedback == null)
			{
				throw new ArgumentException("Feedback not found.", nameof(id));
			}

			await _feedbackRepository.DeleteAsync(id);
		}

		public async Task<Feedback> GetByID(int id)
		{
			var feedback = await _feedbackRepository.GetByIdAsync(id);

			if (feedback == null)
			{
				throw new ArgumentException("Feedback not found.", nameof(id));
			}

			return feedback;
		}

		public async Task<IEnumerable<Feedback>> GetListAsync()
		{
			var todaysDate = DateTime.UtcNow.Date;

			var result = await _feedbackRepository.GetAllFeedbackAsync();

			var olderFeedbacks = result
									.Where(f => f.FeedbackDate.Date <= todaysDate && f.IsActive)
									.OrderByDescending(f => f.FeedbackDate)
									.Take(10)
									.ToList();

			return olderFeedbacks;
		}

		public async Task<IEnumerable<Feedback>> GetListForAdminAsync()
		{
			return await _feedbackRepository.GetAllFeedbackAsync();
		}

		public async Task<Feedback> Update(int id,string customerName, int rating, string comments, bool isActive)
		{
			if (string.IsNullOrWhiteSpace(customerName))
			{
				throw new ArgumentException("Customer name cannot be null or empty.", nameof(customerName));
			}

			if (rating < 1 || rating > 5)
			{
				throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
			}

			if (string.IsNullOrWhiteSpace(comments))
			{
				throw new ArgumentException("Comments cannot be null or empty.", nameof(comments));
			}

			var feedbacks = await _feedbackRepository.GetAllFeedbackAsync();
			var existingFeedback = feedbacks.FirstOrDefault(f => f.FeedbackID == id);

			if (existingFeedback == null)
				throw new KeyNotFoundException($"Feedback with CustomerName '{customerName}' not found");

			existingFeedback.CustomerName = customerName;
			existingFeedback.Ratings = rating;
			existingFeedback.Comment = comments;
			existingFeedback.IsActive = isActive;

			await _feedbackRepository.UpdateAsync(existingFeedback);
			return existingFeedback;
		}

		public async Task<bool> UpdateIsActive(int FeedbackID)
		{
			var result = await _feedbackRepository.UpdateIsActive(FeedbackID);

			if (!result)
			{
				throw new KeyNotFoundException($"Feedback with ID {FeedbackID} not found.");
			}

			return result;
		}
	}
}
