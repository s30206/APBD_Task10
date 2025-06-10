using System.Text.Json;
using APBD_Task10.Models.DTOs;
using APBD_Task10.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Task10.Models.Helpers.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationMiddleware> _logger;

    public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (context.Request.Path.StartsWithSegments("/api/devices")
                && context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                _logger.LogInformation("Begin middleware process");
                context.Request.EnableBuffering();

                var service = context.RequestServices.GetService<IValidationService>();
                if (service == null)
                    throw new Exception("Validation service not found");
                _logger.LogInformation("Successfully loaded ValidationService");

                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                reader.Close();
                context.Request.Body.Position = 0;

                InsertDeviceRequestDTO? jsonObject = JsonSerializer.Deserialize<InsertDeviceRequestDTO>(body,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                _logger.LogInformation($"Successfully deserialized request : {jsonObject}");
                
                var validationResult = await service.ValidateDevice(jsonObject);
                _logger.LogInformation("Successfully validated the device");

                if (!validationResult.IsNullOrEmpty())
                {
                    _logger.LogInformation("Errors found during validation of the device");
                    var errors = "";
                    foreach (var result in validationResult)
                    {
                        errors += result + "\n";
                    }

                    throw new ApplicationException("Validation failed: \n" + errors);
                }

                _logger.LogInformation("End middleware process");
            }

            await _next(context);
        }
        catch (ApplicationException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
            _logger.LogError(ex, "Validation failed: {0}", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(ex.Message);
            _logger.LogError(ex, "NotFound: {0}", ex.Message);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(ex.Message);
            _logger.LogError(ex, "Error occured when validating properties {0}", ex.Message);
        }
    }
}