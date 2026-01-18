using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Templates;
using Todo_App.Application.Common;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Auth.Handlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, AbstractViewModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<AbstractViewModel> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {
                    response.Success = false;
                    response.Message = "Email is not found";
                    return response;
                }
                else if (user.CodeResetPassword != request.Code)
                {
                    response.Success = false;
                    response.Message = "Invalid code";
                    return response;
                }
                else if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
                {
                    response.Success = false;
                    response.Message = "Code expired";
                    return response;
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
                if (!result.Succeeded)
                {
                    response.Success = false;
                    response.Message = "Reset password faild";
                    response.LstErros = result.Errors.Select(e => e.Description).ToList();
                    return response;
                }
                var htmlBody = EmailTemplates.ChangePasswordEmail(user.UserName!);
                await _emailSender.SendEmailAsync(request.Email, "Change Password", htmlBody);
                response.Success = true;
                response.Message = "Password reset succesfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}

