using BeautyParlourAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyParlourAPIs.DAL.Configuration
{
	public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
	{
		public void Configure(EntityTypeBuilder<CustomerAccount> builder)
		{
			builder.HasKey(c => c.AccountID);
			builder.Property(c => c.CustomerName).IsRequired().HasMaxLength(100);
			builder.Property(c => c.TotalAmount).IsRequired().HasColumnType("decimal(18,2)").HasDefaultValue(0);
			builder.Property(c => c.PaidAmount).IsRequired().HasColumnType("decimal(18,2)").HasDefaultValue(0);
			builder.Property(c => c.paidDate).IsRequired(false);

		}
	}
}
