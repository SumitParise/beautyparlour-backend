using BeautyParlourAPIs.Models;

namespace BeautyParlourAPIs.Services.Interface
{
	public interface IFeedback
	{
		Task Add(string customerName,int rating,string comments);

		Task<Feedback> Update(int id, string customerName, int rating, string comments,bool isActive);

		Task Delete(int id);

		
		Task<Feedback> GetByID(int customerID);

		Task<IEnumerable<Feedback>> GetListAsync();
		Task<IEnumerable<Feedback>> GetListForAdminAsync();

		Task<bool> UpdateIsActive(int FeedbackID);

	}
}
