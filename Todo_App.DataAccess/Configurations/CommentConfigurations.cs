using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo_App.Domain.Entities;

namespace Todo_App.DataAccess.Configurations
{
    public class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Content).IsRequired();

            
            builder.HasOne(t => t.TaskItem)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.TaskItemId);

            builder.HasOne(u => u.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(u => u.UserId);

        }
    }
}
