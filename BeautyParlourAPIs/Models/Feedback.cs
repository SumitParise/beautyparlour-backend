using System.ComponentModel.DataAnnotations;

namespace BeautyParlourAPIs.Models
{
	public class Feedback
	{
		public int FeedbackID { get; set; } = 0;

		[Required]
		[Range(1, 5, ErrorMessage = "Ratings must be between 1 and 5.")]
		public int Ratings { get; set; } = 0;
		public string CustomerName { get; set; } = "";
		public string Comment { get; set; } = "";

		public bool IsActive { get; set; } = true;

		public DateTime FeedbackDate { get; set; } = DateTime.Now;

		
	}
}
