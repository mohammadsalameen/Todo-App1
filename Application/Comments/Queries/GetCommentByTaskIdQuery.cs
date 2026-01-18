using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Comments.Queries
{
    public class GetCommentByTaskIdQuery: IRequest<List<GCBTId_Response>>
    {
        public Guid TaskItemId { get; set; }
    }
    public class GCBTId_Response
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? TaskTitle { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
