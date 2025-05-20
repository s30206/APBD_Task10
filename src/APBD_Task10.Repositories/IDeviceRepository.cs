using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Repositories;

public interface IDeviceRepository
{
    public Task<List<Device>> GetAllDevices(CancellationToken token);
    public Task<Device?> GetDeviceById(int id, CancellationToken token);
    public Task<List<Employee>> GetAllEmployees(CancellationToken token);
    public Task<Employee?> GetEmployeeById(int id, CancellationToken token);
    public Task<int> DeleteDeviceById(int id, CancellationToken token);
}