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
    public class SendCodeHandler : IRequestHandler<SendCodeCommand, AbstractViewModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public SendCodeHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<AbstractViewModel> Handle(SendCodeCommand request, CancellationToken cancellationToken)
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
                var random = new Random();
                var code = random.Next(1000, 9999).ToString();
                user.CodeResetPassword = code;
                user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(5);

                await _userManager.UpdateAsync(user);
                var htmlBody = EmailTemplates.SendCodeEmail(user.UserName!, code);
                await _emailSender.SendEmailAsync(request.Email, "Reset Password", htmlBody);

                response.Success = true;
                response.Message = "Code is sent to your email";
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
