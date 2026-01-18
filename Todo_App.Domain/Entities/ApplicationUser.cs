using Microsoft.AspNetCore.Identity;
using Todo_App.Domain.Enums;

namespace Todo_App.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? CodeResetPassword { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
