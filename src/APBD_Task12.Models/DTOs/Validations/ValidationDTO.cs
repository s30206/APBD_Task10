namespace APBD_Task10.Models.DTOs.Validations;

public class ValidationDTO
{
    public string Type { get; set; }
    public string PreRequestName { get; set; }
    public object PreRequestValue { get; set; }
    public List<Rule> Rules { get; set; }
}