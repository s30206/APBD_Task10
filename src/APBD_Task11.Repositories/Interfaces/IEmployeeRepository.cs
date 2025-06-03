namespace APBD_Task10.Repositories;

public interface IEmployeeRepository
{
    public Task<List<Employee>> GetAllEmployees(CancellationToken token);
    public Task<Employee?> GetEmployeeById(int id, CancellationToken token);
}