using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Application.Tasks.Commands;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        public CreateTaskHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AbstractViewModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();
            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                IsCompleted = false,
                UserId = request.UserId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);
            response.Id = task.Id;
            response.Success = true;
            return response;
        }
    }
}
