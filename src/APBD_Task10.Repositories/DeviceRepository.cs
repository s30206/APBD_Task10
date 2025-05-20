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
}