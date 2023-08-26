using AutoMapper;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers;

public class NewUserRoleIdResolver : IValueResolver<UserSignupDTO, User, int>
{
    private readonly IRoleService _roleService;

    public NewUserRoleIdResolver(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public int Resolve(UserSignupDTO dto, User trip, int newUserRoleId, ResolutionContext context)
    {
        return (int)_roleService.GetRoleIdByName(UtilConstants.SignupDefaultRoleName);
    }
}
