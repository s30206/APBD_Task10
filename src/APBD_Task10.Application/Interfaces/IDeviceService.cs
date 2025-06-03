using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Services;

public interface IDeviceService
{
    public Task<List<ShortDeviceDTO>> GetAllDevices(CancellationToken token);
    public Task<FullDeviceDTO?> GetDeviceById(int id, CancellationToken token);
    public Task<bool> DeleteDeviceById(int id, CancellationToken token);
    public Task<bool> AddDevice(InsertDeviceRequestDTO request, CancellationToken token);
    public Task<bool> UpdateDevice(int id, InsertDeviceRequestDTO request, CancellationToken token);
}