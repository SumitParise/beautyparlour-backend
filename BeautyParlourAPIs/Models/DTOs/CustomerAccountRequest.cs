namespace BeautyParlourAPIs.Models.DTOs
{
	public class CustomerAccountRequest
	{
		public int AccountID { get; set; } = 0;
		public string CustomerName { get; set; } = "";
		
		public decimal TotalAmount { get; set; } = 0;

		public decimal PaidAmount { get; set; } = 0;


	}
}
