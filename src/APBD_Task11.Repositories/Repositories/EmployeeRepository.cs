using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DeviceContext _context;

    public EmployeeRepository(DeviceContext context)
    {
        _context = context;
    }
    
    public async Task<List<Employee>> GetAllEmployees(CancellationToken token)
    {
        return await _context.Employees
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