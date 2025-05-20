namespace APBD_Task10.Models.DTOs;

public class FullDeviceDTO
{
    public string Name { get; set; }
    public string? DeviceTypeName { get; set; }
    public bool IsEnabled { get; set; }
    public object? AdditionalProperties { get; set; }
    public ShortEmployeeDTO? Employee { get; set; }
}