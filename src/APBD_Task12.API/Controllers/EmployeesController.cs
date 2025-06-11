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
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllEmployees(CancellationToken token)
    {
        _logger.LogInformation("GET /api/employees was called in EmployeesController");
        try
        {
            var result = await _employeeService.GetAllEmployees(token);
            return result != null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in GET /api/employees in EmployeesController: {0}", ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id, CancellationToken token)
    {
        _logger.LogInformation("GET /api/employees/{0} was called in EmployeesController", id);
        try
        {
            var result = await _employeeService.GetEmployeeById(id, token);
            return result != null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in GET /api/employees/{0} in EmployeesController: {1}", id, ex.Message);
            return Problem(ex.Message);
        }
    }
}