# Overview

This project contains an API that connects to a Microsoft SQL Server database. The database connection is configured via a connection string stored in the `appsettings.json` file.

# Setting up `appsettings.json`

Create a file named `appsettings.json` inside the `src/APBD_Task12.API` folder with the following content:

<pre>
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "Database": ""
    },
    "Jwt": {
        "Issuer": "http://localhost:5300",
        "Audience": "http://localhost:5300",
        "Key": "",
        "ValidInMinutes": 10
    }
}
</pre>

In the `Database` field, provide your connection string to the Microsoft SQL Server instance.\
In the `Key` field, provide a secure key used to sign and validate JWT tokens (it should be a long, random string)

# Project structure

The code is split into multiple projects to improve the structure, readability and maintainability of the application and to avoid issues like circular dependency.\
Also, I got used to doing it this way :D
