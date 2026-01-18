using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Comments.Commands
{
    public class GetCommentsQuery: IRequest<List<GCQ_Response>>
    {
    }
    public class GCQ_Response
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid? TaskItemId { get; set; }
        public string? TaskTitle { get; set; }

    }
}
