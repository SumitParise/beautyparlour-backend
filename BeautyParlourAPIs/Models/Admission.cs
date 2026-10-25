namespace BeautyParlourAPIs.Models
{
	public class Admission
	{
		public int AdmissionID { get; set; } = 0;
		public string Name { get; set; } = "";
		public string MobileNo { get; set; } = "";
		public string Address { get; set; }="";
		public string Email { get; set; } = "";
		public DateTime AdmissionDate { get; set; }=DateTime.Now;

		public string AadhaarCardPath { get; set; } = "";
		public string Passportphotopath { get; set; } = "";
		public string RationCardPath { get; set; } = "";
		public string LeavingCertificatePath { get; set; } = "";
	}
}
