using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Services;

public interface IDeviceService
{
    public Task<List<ShortDeviceDTO>> GetAllDevices(CancellationToken token);
    public Task<FullDeviceDTO?> GetDeviceById(int id, CancellationToken token);
    public Task<List<ShortEmployeeDTO>> GetAllEmployees(CancellationToken token);
    public Task<FullEmployeeDTO?> GetEmployeeById(int id, CancellationToken token);
}