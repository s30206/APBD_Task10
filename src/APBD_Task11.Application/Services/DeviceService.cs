using System.Text.Json;
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
            result.Add(new ShortDeviceDTO
            {
                Id = device.Id,
                Name = device.Name
            });
        }
        return result;
    }

    public async Task<FullDeviceDTO?> GetDeviceById(int id, CancellationToken token)
    {
        var device = await _deviceRepository.GetDeviceById(id, token);
        if (device == null) return null;
        
        var currentEmployee = device.DeviceEmployees.FirstOrDefault(o => o.ReturnDate == null);


        return new FullDeviceDTO
        {
            Name = device.Name,
            Type = device.DeviceType?.Name,
            IsEnabled = device.IsEnabled,
            AdditionalProperties = JsonDocument.Parse(device.AdditionalProperties).RootElement
        };
    }

    public async Task<bool> DeleteDeviceById(int id, CancellationToken token)
    {
        var result = await _deviceRepository.DeleteDeviceById(id, token);
        return result > 0;
    }

    public async Task<bool> AddDevice(InsertDeviceRequestDTO request, CancellationToken token)
    {
        var deviceType = await _deviceRepository.GetDeviceType(request.TypeId, token);
        if (deviceType == null) return false;

        var device = new Device()
        {
            Name = request.Name,
            DeviceTypeId = deviceType.Id,
            IsEnabled = request.IsEnabled,
            AdditionalProperties = request.AdditionalProperties?.GetRawText() ?? "",
        };
        
        var result = await _deviceRepository.AddDevice(device, token);
        return result > 0;
    }

    public async Task<bool> UpdateDevice(int id, InsertDeviceRequestDTO request, CancellationToken token)
    {
        var device = await _deviceRepository.GetDeviceById(id, token);
        if (device == null) return false;
        
        var deviceType = await _deviceRepository.GetDeviceType(request.TypeId, token);
        if (deviceType == null) return false;
        
        device.Name = request.Name;
        device.DeviceTypeId = deviceType.Id;
        device.IsEnabled = request.IsEnabled;
        device.AdditionalProperties = request.AdditionalProperties?.GetRawText() ?? "";
        
        var result = await _deviceRepository.UpdateDevice(device, token);
        return result > 0;
    }
}