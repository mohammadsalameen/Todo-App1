using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Domain.Entities
{
    public class Comment : BaseModel
    {
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid TaskItemId { get; set; }
        public string? TaskTitle { get; set; }
        public TaskItem TaskItem { get; set; } = null!;
    }
}
