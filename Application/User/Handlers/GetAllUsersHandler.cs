using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.User.Queries;
using Todo_App.Domain.Entities;

public class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQuery, List<GAUQ_Response>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<GAUQ_Response>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);

        var result = new List<GAUQ_Response>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new GAUQ_Response
            {
                Id = user.Id,
                UserName = user.UserName!,
                Role = roles.FirstOrDefault() ?? "NoRole"
            });
        }

        return result;
    }
}
