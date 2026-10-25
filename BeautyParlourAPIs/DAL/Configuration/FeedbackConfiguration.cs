using BeautyParlourAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyParlourAPIs.DAL.Configuration
{
	public class FeedbackConfiguration: IEntityTypeConfiguration<Feedback>
	{
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{
			builder.HasKey(f => f.FeedbackID);
			builder.Property(f => f.Ratings).IsRequired();
			builder.HasCheckConstraint("CK_Feedback_Ratings", "[Ratings] >= 1 AND [Ratings] <= 5");
			builder.Property(f => f.CustomerName).IsRequired().HasMaxLength(100);
			builder.Property(f => f.Comment).HasMaxLength(500);
			builder.Property(f=>f.IsActive).IsRequired().HasDefaultValue(true);
			builder.Property(f => f.FeedbackDate).IsRequired().HasDefaultValueSql("GETDATE()");
		}
	}
}
