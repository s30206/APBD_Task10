using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.Models.DTOs;

public class AccountLoginDTO
{
    [Required]
    [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Username must not start with a number")]
    public string Login { get; set; }
    [Required]
    [MinLength(12)]
    public string Password { get; set; }
}