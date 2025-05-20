namespace APBD_Task10.Repositories;

public interface IDeviceRepository
{
    public Task<List<Device>> GetAllDevices(CancellationToken token);
}