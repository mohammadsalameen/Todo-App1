using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Domain.Entities
{
    public class TaskItem : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public bool IsUrgent { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid? AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
