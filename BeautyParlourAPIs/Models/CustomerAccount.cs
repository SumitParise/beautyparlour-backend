namespace BeautyParlourAPIs.Models
{
	public class CustomerAccount
	{
		public int AccountID { get; set; } = 0;

		public string CustomerName { get; set; } = "";

		public decimal TotalAmount { get; set; } = 0;
		public decimal PaidAmount { get; set; } = 0;

		public DateTime? paidDate { get; set; }

		public decimal BalanceAmount => TotalAmount - PaidAmount;
	}
}
