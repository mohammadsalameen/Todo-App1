using AutoMapper;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Comments.Queries;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Common.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentCommand, Comment>()
             .ForMember(dest => dest.Content,
                opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.TaskItemId,
                 opt => opt.MapFrom(src => src.TaskItemId))
                .ForMember(dest => dest.TaskTitle,
                 opt => opt.MapFrom(src => src.TaskTitle))

             .ForMember(dest => dest.UserId, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Comment, GCQ_Response>()
                    .ForMember(dest => dest.TaskTitle,
                      opt => opt.MapFrom(src => src.TaskItem.Title));

            CreateMap<Comment, GCBTId_Response>()
             .ForMember(dest => dest.TaskTitle,
                opt => opt.MapFrom(src => src.TaskItem.Title))
             .ForMember(dest => dest.CreatedBy,
             opt => opt.MapFrom(src => src.User.UserName))
             .ForMember(dest => dest.UpdatedBy,
             opt => opt.MapFrom(src => src.UpdatedBy));
        }
    }
}
