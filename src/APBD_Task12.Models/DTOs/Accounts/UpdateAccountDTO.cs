namespace APBD_Task10.Models.DTOs;

public class UpdateAccountDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int? RoleId { get; set; }
}