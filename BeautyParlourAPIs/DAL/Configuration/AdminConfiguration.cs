using BeautyParlourAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyParlourAPIs.DAL.Configuration
{
	public class AdminConfiguration:IEntityTypeConfiguration<Admin>
	{
		public void Configure(EntityTypeBuilder<Admin> builder)
		{
			builder.HasKey(f => f.AdminId);
			builder.Property(f => f.Email).IsRequired().HasMaxLength(100);
			builder.HasIndex(f => f.Email).IsUnique();
			builder.Property(f => f.PasswordHash).IsRequired();
			builder.Property(f => f.FullName).IsRequired().HasMaxLength(100);
			builder.Property(f => f.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
		}
	}
}
