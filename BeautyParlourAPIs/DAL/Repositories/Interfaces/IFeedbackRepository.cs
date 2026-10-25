using BeautyParlourAPIs.Models;

namespace BeautyParlourAPIs.DAL.Repositories.Interfaces
{
	public interface IFeedbackRepository
	{
		Task<IEnumerable<Feedback>> GetAllFeedbackAsync();
		Task AddAsync(Feedback feedback);

		Task<Feedback?> GetByIdAsync(int id);

		Task UpdateAsync(Feedback feedback);

		Task DeleteAsync(int id);

		Task<bool> UpdateIsActive(int feedbackID);

	}
}
