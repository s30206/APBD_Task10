using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Services;

public interface IEmployeeService
{
    public Task<List<ShortEmployeeDTO>> GetAllEmployees(CancellationToken token);
    public Task<FullEmployeeDTO?> GetEmployeeById(int id, CancellationToken token);
}