using APBD_Task10.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.Controllers;

[Route("api/employees")]
[Authorize]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllEmployees(CancellationToken token)
    {
        try
        {
            var result = await _employeeService.GetAllEmployees(token);
            return result != null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id, CancellationToken token)
    {
        try
        {
            var result = await _employeeService.GetEmployeeById(id, token);
            return result != null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}