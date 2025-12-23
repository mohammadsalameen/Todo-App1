using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int TaskItemId {get; set;}
        public TaskItem TaskItem { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
