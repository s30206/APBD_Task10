using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.Models.DTOs;

public class AccountLoginDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
}