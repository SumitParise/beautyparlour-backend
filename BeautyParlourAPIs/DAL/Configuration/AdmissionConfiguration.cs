using BeautyParlourAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyParlourAPIs.DAL.Configuration;

public class AdmissionConfiguration:IEntityTypeConfiguration<Admission>
{
	public void Configure(EntityTypeBuilder<Admission> builder)
	{
		builder.HasKey(f => f.AdmissionID);
		builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
		builder.Property(f => f.MobileNo).IsRequired().HasMaxLength(15);
		builder.HasIndex(f => f.MobileNo).IsUnique(); 
		builder.Property(f => f.Address).HasMaxLength(500);
		builder.Property(f => f.Email).IsRequired().HasMaxLength(100);
		builder.HasIndex(f=>f.Email).IsUnique();
		builder.Property(f => f.AdmissionDate).IsRequired().HasDefaultValueSql("GETDATE()");
	}

	
}

