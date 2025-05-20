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
            Employee = currentEmployee is null
                ? null
                : new ShortEmployeeDTO
                {
                    Id = currentEmployee.Id,
                    Name = $"{currentEmployee.Employee.Person.FirstName} {currentEmployee.Employee.Person.MiddleName} {currentEmployee.Employee.Person.LastName}",
                },
            AdditionalProperties = JsonDocument.Parse(device.AdditionalProperties).RootElement
        };
    }

    public async Task<List<ShortEmployeeDTO>> GetAllEmployees(CancellationToken token)
    {
        var employees = await _deviceRepository.GetAllEmployees(token);
        var result = new List<ShortEmployeeDTO>();

        foreach (var employee in employees)
        {
            result.Add(new ShortEmployeeDTO
            {
                Id = employee.Id,
                Name = $"{employee.Person.FirstName} {employee.Person.MiddleName} {employee.Person.LastName}",
            });
        }
        
        return result;
    }

    public async Task<FullEmployeeDTO?> GetEmployeeById(int id, CancellationToken token)
    {
        var employee = await _deviceRepository.GetEmployeeById(id, token);
        if (employee == null) return null;

        return new FullEmployeeDTO
        {
            Id = employee.Id,
            FirstName = employee.Person.FirstName,
            MiddleName = employee.Person.MiddleName,
            LastName = employee.Person.LastName,
            PassportNumber = employee.Person.PassportNumber,
            PhoneNumber = employee.Person.PhoneNumber,
            Email = employee.Person.Email,
            Salary = employee.Salary,
            Position = new ShortPositionDTO
            {
                Id = employee.PositionId,
                Name = employee.Position.Name,
            },
            HireDate = employee.HireDate,
        };
    }

    public async Task<bool> DeleteDeviceById(int id, CancellationToken token)
    {
        var result = await _deviceRepository.DeleteDeviceById(id, token);
        return result > 0;
    }
}