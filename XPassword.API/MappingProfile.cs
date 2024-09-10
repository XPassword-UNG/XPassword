using AutoMapper;
using XPassword.API.Models;

namespace XPassword.API;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Register, Database.Model.Register>();
        CreateMap<Database.Model.Register, Register>();
    }
}