namespace BeautyParlourAPIs.Models
{
	public class Admin
	{
		public int AdminId { get; set; } = 0;
		public string Email { get; set; } = "";
		public string PasswordHash { get; set; } = ""; // store hashed password
		public string FullName { get; set; } = "";
		public DateTime CreatedAt { get; set; } = DateTime.Now;


	}
}
