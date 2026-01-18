using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Todo_App.Application.Common.Pagination;
using Todo_App.Application.User.Queries;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

public class GetUserPagedQueryHandler
    : IRequestHandler<GetUsersPagedQuery, PagedResult<GUPQ_Response>>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserPagedQueryHandler(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager
        )
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<PagedResult<GUPQ_Response>> Handle(
        GetUsersPagedQuery request,
        CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User
            ?? throw new UnauthorizedAccessException();

        var isAdmin = user.IsInRole("Admin");
        var currentUserId = Guid.Parse(
            user.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        //  Base Query
        var query =
            from u in _context.Users
            join ur in _context.UserRoles on u.Id equals ur.UserId into urj
            from ur in urj.DefaultIfEmpty()
            join r in _context.Roles on ur.RoleId equals r.Id into rj
            from r in rj.DefaultIfEmpty()
            select new
            {
                u.Id,
                u.UserName,
                Role = r.Name
            };

        //  Authorization logic
        if (!isAdmin)
        {
            // User sees himself only
            query = query.Where(u => u.Id == currentUserId);
        }

        //  Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                u.UserName!.Contains(request.Search));
        }

        //  Total Count (before pagination)
        var totalCount = await query.CountAsync(cancellationToken);

        //  Pagination
        var users = await query
            .OrderByDescending(u => u.UserName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new GUPQ_Response
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.Role ?? "NoRole"
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<GUPQ_Response>
        {
            Items = users,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
