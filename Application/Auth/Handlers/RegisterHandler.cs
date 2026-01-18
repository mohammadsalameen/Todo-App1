using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Templates;
using Todo_App.Application.Common;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.Auth.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AbstractViewModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public RegisterHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper,
            IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _emailSender = emailSender;
            _configuration = configuration;
        }
        public async Task<AbstractViewModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();
            try
            {
                var validationErrors = await ValidateAsync(request);
                if (validationErrors.Any())
                {
                    response.Success = false;
                    response.LstErros = validationErrors.ToList();
                    return response;
                }
                //var user = new ApplicationUser
                //{
                //    UserName = request.UserName,
                //    Email = request.Email,
                //};

                var user = _mapper.Map<ApplicationUser>(request);
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!await _roleManager.RoleExistsAsync(request.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(request.Role));
                }

                await _userManager.AddToRoleAsync(user, request.Role);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var confirmUrl = $"{_configuration["App:BaseUrl"]}/api/auth/confirm-email?token={token}&userId={user.Id}";
                var htmlBody = EmailTemplates.ConfirmEmail(user.UserName!, confirmUrl);
                await _emailSender.SendEmailAsync(user.Email!, "Confirm Your Email ", htmlBody);

                response.Id = user.Id;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An unexpected error";
                response.LstErros = new List<string> { ex.Message };
                return response;
            }
        }

        private async Task<List<string>> ValidateAsync(RegisterCommand request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.UserName))
                errors.Add("Username is required");

            if (request.UserName?.Length < 3)
                errors.Add("Username must be at least 3 characters");

            if (string.IsNullOrWhiteSpace(request.Email))
                errors.Add("Email is required");

            if (!request.Email.Contains("@"))
                errors.Add("Invalid email format");

            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("Password is required");

            if (request.Password.Length < 6)
                errors.Add("Password must be at least 6 characters");

            if (request.Role != "User" && request.Role != "Admin")
                errors.Add("Role must be User or Admin");

            // async validation (DB check)
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                errors.Add("Email already exists");

            return errors;
        }
    }

}

