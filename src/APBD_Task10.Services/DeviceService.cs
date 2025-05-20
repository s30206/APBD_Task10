using APBD_Task10.Models.DTOs;
using APBD_Task10.Repositories;

namespace APBD_Task10.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<List<ShortDeviceDTO>> GetAllDevices(CancellationToken token)
    {
        var devices = await _deviceRepository.GetAllDevices(token);
        var result = new List<ShortDeviceDTO>();
        foreach (var device in devices)
        {
            result.Add(new ShortDeviceDTO()
            {
                ID = device.Id,
                Name = device.Name,
            });
        }
        return result;
    }
}