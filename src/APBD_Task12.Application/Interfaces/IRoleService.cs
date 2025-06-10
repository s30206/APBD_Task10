using APBD_Task10.Models.DTOs.Roles;

namespace APBD_Task10.Services;

public interface IRoleService
{
    public Task<IEnumerable<ShortRoleDTO>> GetRoles();
}