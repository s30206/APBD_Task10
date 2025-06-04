using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.Models.DTOs;

public class CreateAccountDTO
{
    [Required]
    [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Username must not start with a number")]
    public string Username { get; set; }
    [Required]
    [MinLength(12)]
    public string Password { get; set; }
    [Required]
    public int EmployeeId { get; set; }
}