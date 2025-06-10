using APBD_Task10.Models.DTOs.Roles;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.Services;

public class RoleService : IRoleService
{
    public readonly DeviceContext _context;

    public RoleService(DeviceContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ShortRoleDTO>> GetRoles()
    {
        var roles = await _context.Roles.ToListAsync();

        var result = new List<ShortRoleDTO>();

        foreach (var role in roles)
        {
            result.Add(new ShortRoleDTO()
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        return result;
    }
}