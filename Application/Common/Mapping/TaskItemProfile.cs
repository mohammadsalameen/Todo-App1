using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Tasks.Commands;
using Todo_App.Application.Tasks.Queries;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Common.Mapping
{
    public class TaskItemProfile: Profile
    {
        public TaskItemProfile()
        {
            CreateMap<CreateTaskCommand, TaskItem>()
                .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsUrgent,
                opt => opt.MapFrom(src => src.IsUrgent))
                .ForMember(dest => dest.IsCompleted,
                opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.UserId,
                opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy,
                opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy,
                opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt,
                opt => opt.Ignore());
            CreateMap<TaskItem, GTBUQ_Response>();
            CreateMap<TaskItem, GMTQ_Response>();
        }
    }
}
