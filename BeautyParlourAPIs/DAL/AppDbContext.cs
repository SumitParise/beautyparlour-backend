using BeautyParlourAPIs.DAL.Configuration;
using BeautyParlourAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautyParlourAPIs.DAL;

public class AppDbContext:DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Feedback> feedbacks { get; set; }
	public DbSet<Admission> admissions { get; set; }
	public DbSet<CustomerAccount> accounts { get; set; }
	public DbSet<Admin> admins { get; set; }
	public DbSet<MobileOtp> mobileOtps { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(new AdmissionConfiguration());
		modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
		modelBuilder.ApplyConfiguration(new CustomerAccountConfiguration());
		modelBuilder.ApplyConfiguration(new AdminConfiguration());

	}
}