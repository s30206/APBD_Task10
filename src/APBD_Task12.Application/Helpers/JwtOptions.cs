﻿namespace APBD_Task10.Models.Helpers;

public class JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int ValidInMinutes { get; init; }
}