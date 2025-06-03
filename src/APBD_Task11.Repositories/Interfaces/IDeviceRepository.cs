using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Repositories;

public interface IDeviceRepository
{
    public Task<List<Device>> GetAllDevices(CancellationToken token);
    public Task<Device?> GetDeviceById(int id, CancellationToken token);
    public Task<int> DeleteDeviceById(int id, CancellationToken token);
    public Task<int> AddDevice(Device device, CancellationToken token);
    public Task<DeviceType?> GetDeviceType(string typeName, CancellationToken token);
    public Task<int> UpdateDevice(Device device, CancellationToken token);
}