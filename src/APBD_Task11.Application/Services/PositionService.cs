using APBD_Task10.Models.DTOs;
using APBD_Task10.Models.DTOs.Roles;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.Services;

public class PositionService : IPositionService
{
    public readonly DeviceContext _context;

    public PositionService(DeviceContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ShortPositionDTO>> GetPositions()
    {
        var positions = await _context.Positions.ToListAsync();

        var result = new List<ShortPositionDTO>();

        foreach (var position in positions)
        {
            result.Add(new ShortPositionDTO()
            {
                Id = position.Id,
                Name = position.Name,
            });
        }

        return result;
    }
}