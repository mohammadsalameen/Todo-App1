using MediatR;
using Microsoft.AspNetCore.Identity;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Common;
using Todo_App.Domain.Entities;

public class ConfirmEmailHandler
    : IRequestHandler<ConfirmEmailCommand, AbstractViewModel>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AbstractViewModel> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        var response = new AbstractViewModel();

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
        {
            response.Success = false;
            response.LstErros = result.Errors
                .Select(e => e.Description)
                .ToList();
            return response;
        }

        response.Success = true;
        response.Message = "Email confirmed successfully";
        return response;
    }
}
