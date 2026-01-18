using MediatR;
using Todo_App.Application.Tasks.Queries;
using System;

public class FilterTasksQuery : IRequest<List<GTPQ_Response>>
{
    public string Status { get; set; } = "all";
    public Guid? UserId { get; set; }
}

public class GTPQ_Response: GTBUQ_Response { }