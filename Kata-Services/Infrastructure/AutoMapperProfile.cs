using AutoMapper;
using Kata_DAL.Entities;
using Kata_Services.Queries;

namespace Kata_Services.Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Message, GetAllMessagesByUserViewModel>();
    }
}