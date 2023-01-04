using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace MicroserviceDemo.Web.ObjectMapping;

public class WebAutoMapperProfile : Profile
{
    public WebAutoMapperProfile()
    {
        CreateMap<IdentityUserDto, IdentityUserUpdateDto>()
            .MapExtraProperties()
            .Ignore(x => x.Password)
            .Ignore(x => x.RoleNames);

        CreateMap<IdentityRoleDto, IdentityRoleUpdateDto>()
            .MapExtraProperties();
    }
}