using APBD_Task10.Models.DTOs;
using APBD_Task10.Repositories;

namespace APBD_Task10.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<ShortEmployeeDTO>> GetAllEmployees(CancellationToken token)
    {
        var employees = await _employeeRepository.GetAllEmployees(token);
        var result = new List<ShortEmployeeDTO>();

        foreach (var employee in employees)
        {
            result.Add(new ShortEmployeeDTO
            {
                Id = employee.Id,
                FullName = $"{employee.Person.FirstName} {employee.Person.MiddleName} {employee.Person.LastName}",
            });
        }
        
        return result;
    }

    public async Task<FullEmployeeDTO?> GetEmployeeById(int id, CancellationToken token)
    {
        var employee = await _employeeRepository.GetEmployeeById(id, token);
        if (employee == null) return null;

        return new FullEmployeeDTO
        {
            Person = new PersonDTO() {
                FirstName = employee.Person.FirstName,
                MiddleName = employee.Person.MiddleName,
                LastName = employee.Person.LastName,
                PassportNumber = employee.Person.PassportNumber,
                PhoneNumber = employee.Person.PhoneNumber,
                Email = employee.Person.Email
            },
            Salary = employee.Salary,
            Position = employee.Position.Name,
            HireDate = employee.HireDate,
        };
    }
}