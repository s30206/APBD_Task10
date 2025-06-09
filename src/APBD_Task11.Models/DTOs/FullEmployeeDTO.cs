namespace APBD_Task10.Models.DTOs;

public class FullEmployeeDTO
{
    public PersonDTO Person { get; set; }
    public decimal? Salary { get; set; }
    public string Position { get; set; }
    public DateTime HireDate { get; set; }
}