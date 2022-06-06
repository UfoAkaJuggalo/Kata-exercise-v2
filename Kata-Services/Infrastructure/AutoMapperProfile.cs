using AutoMapper;
using Kata_DAL.Entities;
using Kata_Services.CommonViewModels;
using Kata_Services.Queries;
using Kata_Services.Queries.GetFeed;

namespace Kata_Services.Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Message, GetAllMessagesByUserViewModel>();
        CreateMap<Message, GetFeedViewModel>();
        CreateMap<User, UserInfoViewModel>();
    }
}