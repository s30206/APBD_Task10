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
                ID = device.Id,
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
            DeviceTypeName = device.DeviceType?.Name,
            IsEnabled = device.IsEnabled,
            Employee = currentEmployee == null
                ? null
                : new ShortEmployeeDTO
                {
                    Id = currentEmployee.Id,
                    Name = currentEmployee.Employee.Person.FirstName 
                           + " " + currentEmployee.Employee.Person.MiddleName + " " + currentEmployee.Employee.Person.LastName,
                },
            AdditionalProperties = JsonDocument.Parse(device.AdditionalProperties)
        };
    }
}