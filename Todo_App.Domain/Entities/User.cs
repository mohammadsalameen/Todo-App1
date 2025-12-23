using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Domain.Enums;

namespace Todo_App.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
