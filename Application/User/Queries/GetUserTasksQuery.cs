using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.User.Queries
{
    public class GetUserTasksQuery: IRequest<List<UserWithTasksResponse>>
    {
    }
    public class UserWithTasksResponse
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public List<TaskWithCommentsResponse> Tasks { get; set; } = new();
    }

    public class TaskWithCommentsResponse
    {
        public Guid TaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUrgent { get; set; }
        public List<CommentResponse> Comments { get; set; } = new();
    }

    public class CommentResponse
    {
        public Guid CommentId { get; set; }
        public string? Content { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
