namespace BeautyParlourAPIs.Models.DTOs
{
	public class EmailSetting
	{
		public string Host { get; set; } = "";
		public int Port { get; set; } = 587;
		public string Email { get; set; } = "";
		public string Password { get; set; } = "";
	}
}
