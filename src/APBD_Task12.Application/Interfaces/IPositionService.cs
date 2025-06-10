using APBD_Task10.Models.DTOs;
using APBD_Task10.Models.DTOs.Roles;

namespace APBD_Task10.Services;

public interface IPositionService
{
    public Task<IEnumerable<ShortPositionDTO>> GetPositions();
}