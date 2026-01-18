using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Queries;
using Todo_App.Application.User.Queries;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Common.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, GAUQ_Response>();
        }
    }
}
