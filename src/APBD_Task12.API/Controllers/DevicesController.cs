using System.Security.Claims;
using APBD_Task10.Models.DTOs;
using APBD_Task10.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10Controllers;

[Route("api/devices")]
[Authorize]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DevicesController> _logger;

    public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
    {
        _deviceService = deviceService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllDevices(CancellationToken token)
    {
        _logger.LogInformation("GET /api/devices was called in DevicesController");
        try
        {
            var result = await _deviceService.GetAllDevices(token);
            return result != null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in GET /api/devices in DevicesController: {0}", ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDeviceById(int id, CancellationToken token)
    {
        _logger.LogInformation("GET /api/devices/{0} was called in DevicesController", id);
        try
        {
            var result = await _deviceService.GetDeviceById(id, token);
            return result != null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in GET /api/devices/{0} in DevicesController: {1}", id, ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDeviceById(int id, CancellationToken token)
    {
        _logger.LogInformation("DELETE /api/devices/{0} was called in DevicesController", id);
        try
        {
            var result = await _deviceService.DeleteDeviceById(id, token);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in DELETE /api/devices/{0} in DevicesController: {1}", id, ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDevice([FromBody] InsertDeviceRequestDTO request, CancellationToken token)
    {
        _logger.LogInformation("POST /api/devices was called in DevicesController");
        try
        {
            var result = await _deviceService.AddDevice(request, token);
            return result ? Created("/api/devices/", request) : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in POST /api/devices in DevicesController: {0}", ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] InsertDeviceRequestDTO request, CancellationToken token)
    {
        _logger.LogInformation("PUT /api/devices/{0} was called in DevicesController", id);
        try
        {
            var result = await _deviceService.UpdateDevice(id, request, token);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in PUT /api/devices/{0} in DevicesController: {1}", id, ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("types")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDeviceTypes(CancellationToken token)
    {
        _logger.LogInformation("GET /api/devices/types was called in DevicesController");
        try
        {
            var result = await _deviceService.GetDeviceTypes(token);
            return result is not null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in GET /api/devices/types in DevicesController: {0}", ex.Message);
            return Problem(ex.Message);
        }
    }
}