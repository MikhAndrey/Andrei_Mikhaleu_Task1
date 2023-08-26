using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public int? GetRoleIdByName(string roleName)
    {
        Role? roleWithRequiredName = _unitOfWork.Roles.GetByName(roleName);
        return roleWithRequiredName?.Id;
    }
}
