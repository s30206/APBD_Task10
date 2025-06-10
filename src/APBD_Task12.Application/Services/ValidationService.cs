using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using APBD_Task10.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APBD_Task10.Services;

public class ValidationService : IValidationService
{
    public readonly ILogger<ValidationService> _logger;
    public readonly DeviceContext _context;
    public readonly JsonElement _validations;

    public ValidationService(ILogger<ValidationService> logger, DeviceContext context)
    {
        _logger = logger;

        var path = "../../validationRules/validation_rules.json";
        _validations = JsonDocument.Parse(File.ReadAllText(path)).RootElement.GetProperty("validations");
        logger.LogInformation("Validation rules loaded");
        _context = context;
    }

    public async Task<List<string>?> ValidateDevice(InsertDeviceRequestDTO request)
    {
        var deviceType = await _context.DeviceTypes.FirstOrDefaultAsync(d => d.Id == request.TypeId);
        if (deviceType == null)
            throw new KeyNotFoundException($"Device type {request.TypeId} not found");
        _logger.LogInformation("Successfully found device type");
        
        var entry = FindEntryForDeviceType(deviceType.Name);
        if (entry == null)
            return null;
        _logger.LogInformation("Successfully found device entry in validation rules");

        var preRequestPropertyName = entry.Value.GetProperty("preRequestName").GetString();
        var value = typeof(InsertDeviceRequestDTO).GetProperty(preRequestPropertyName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(request);
        if (value == null)
            throw new KeyNotFoundException($"No field with name {preRequestPropertyName} was found");
        _logger.LogInformation($"Successfully found {preRequestPropertyName} in request body");
        
        var preRequestPropertyValue = entry.Value.GetProperty("preRequestValue").GetString();
        if (!value.ToString()!.Equals(preRequestPropertyValue, StringComparison.OrdinalIgnoreCase))
            return null;
        _logger.LogInformation($"Successfully validated {preRequestPropertyName} value in request body");
        
        var rules = entry.Value.GetProperty("rules").EnumerateArray();
        var errors = new List<string>();
        if (request.AdditionalProperties is null)
            errors.Add("Additional properties are null");
        else
        {
            foreach (var rule in rules)
            {
                var field = rule.GetProperty("paramName").GetString();
                if (!request.AdditionalProperties.Value.TryGetProperty(field, out var fieldElem))
                    errors.Add($"Property {field} not found");
                else
                {
                    var fieldValue = fieldElem.GetString();
                    var regex = rule.GetProperty("regex").GetString();
                    var pattern = regex?.Trim('/');
                    
                    if (!Regex.IsMatch(fieldValue, pattern, RegexOptions.IgnoreCase))
                        errors.Add($"Property {field} doesn't match pattern {pattern}");
                }
            }
        }

        return errors;
    }

    public JsonElement? FindEntryForDeviceType(string deviceType)
    {
        foreach (var entry in _validations.EnumerateArray())
        {
            if (entry.GetProperty("type").GetString() == deviceType)
                return entry;
        }
        return null;
    }
}