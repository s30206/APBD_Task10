using APBD_Task10.Models.DTOs;

namespace APBD_Task10.Services;

public interface IValidationService
{
    public Task<List<string>?> ValidateDevice(InsertDeviceRequestDTO request);
}