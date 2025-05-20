using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly DeviceContext _context;

    public DeviceRepository(DeviceContext context)
    {
        _context = context;
    }

    public async Task<List<Device>> GetAllDevices(CancellationToken token)
    {
        return await _context.Devices.ToListAsync(token);
    }

    public async Task<Device?> GetDeviceById(int id, CancellationToken token)
    {
        return await _context.Devices
            .Include(e => e.DeviceEmployees)
            .ThenInclude(e => e.Employee)
            .ThenInclude(p => p.Person)
            .Include(d => d.DeviceType)
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public Task<List<Employee>> GetAllEmployees(CancellationToken token)
    {
        return _context.Employees
            .Include(e => e.Person)
            .ToListAsync(token);
    }

    public async Task<Employee?> GetEmployeeById(int id, CancellationToken token)
    {
        return await _context.Employees
            .Include(e => e.Person)
            .Include(p => p.Position)
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }
}