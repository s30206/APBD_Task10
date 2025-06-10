using APBD_Task10.Models.DTOs;
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
    
    public async Task<int> DeleteDeviceById(int id, CancellationToken token)
    {
        var device = await _context.Devices
            .Include(de => de.DeviceEmployees)
            .FirstOrDefaultAsync(x => x.Id == id, token);
        if (device == null) return 0;
        
        _context.DeviceEmployees.RemoveRange(device.DeviceEmployees);
        
        _context.Devices.Remove(device);
        return await _context.SaveChangesAsync(token);
    }

    public async Task<int> AddDevice(Device device, CancellationToken token)
    {
        await _context.Devices.AddAsync(device, token);
        return await _context.SaveChangesAsync(token);
    }
    
    public async Task<DeviceType?> GetDeviceType(int typeId, CancellationToken token)
    {
        return await _context.DeviceTypes.FirstOrDefaultAsync(x => x.Id == typeId, token);
    }

    public async Task<int> UpdateDevice(Device device, CancellationToken token)
    {
        _context.Entry(device).State = EntityState.Modified;
        return await _context.SaveChangesAsync(token);
    }

    public async Task<List<DeviceType>?> GetDeviceTypes(CancellationToken token)
    {
        return await _context.DeviceTypes.ToListAsync(token);
    }
}