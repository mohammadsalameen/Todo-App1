using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Services;
using Todo_App.Application.Common;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _generateToken;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginHandler(UserManager<ApplicationUser> userManager, JwtTokenService generateToken, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _generateToken = generateToken;
            _signInManager = signInManager;
        }
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new LoginResponse();
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {
                    response.Success = false;
                    response.Message = "Invalid Email";
                    return response;
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    response.Success = false;
                    response.Message = "Account is locked, try again later";
                    return response;
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
                //var result = await _userManager.CheckPasswordAsync(user, request.Password);

                if (result.IsLockedOut)
                {
                    response.Success = false;
                    response.Message = "Account is Locked due to multiple failed attempts";
                    return response;
                }
                else if (result.IsNotAllowed)
                {
                    response.Success = false;
                    response.Message = "Plz confirm your email";
                    return response;
                }

                if (!result.Succeeded)
                {
                    response.Success = false;
                    response.Message = "Invalid Password";
                    return response;
                }

                response.Success = true;
                response.Message = "Login Successfully";
                response.AccessToken = await _generateToken.Generate(user);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Un expected error";
                response.LstErros = new List<string> { ex.Message };
                return response;
            }
        }
    }
}
