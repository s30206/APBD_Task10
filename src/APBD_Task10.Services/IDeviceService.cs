using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Services;

public interface IDeviceService
{
    public Task<List<ShortDeviceDTO>> GetAllDevices(CancellationToken token);
}