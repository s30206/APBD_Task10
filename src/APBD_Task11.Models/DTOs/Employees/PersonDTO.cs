﻿namespace APBD_Task10.Models.DTOs;

public class PersonDTO
{
    public string PassportNumber { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}