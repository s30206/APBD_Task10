using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace APBD_Task10.Models.DTOs;

public class InsertDeviceRequestDTO
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public int TypeId { get; set; }
    
    [Required]
    public bool IsEnabled { get; set; }
    public JsonElement? AdditionalProperties { get; set; }
}