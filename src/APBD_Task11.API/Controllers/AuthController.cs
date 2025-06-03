using APBD_Task10.Models.DTOs;
using APBD_Task10.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.Controllers;

[Route("api/auth")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly DeviceContext _context;
    private readonly PasswordHasher<Account> _passwordHasher = new();
    private readonly ITokenService _tokenService;

    public AuthController(DeviceContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] AccountLoginDTO request, CancellationToken cancellationToken)
    {
        var foundAccount = await _context.Accounts.Include(r => r.Role).FirstOrDefaultAsync(a => a.Username == request.Username, cancellationToken);
        
        if (foundAccount == null)
            return Unauthorized();
        
        var verification = _passwordHasher.VerifyHashedPassword(foundAccount, foundAccount.Password, request.Password);
        
        if (verification == PasswordVerificationResult.Failed)
            return Unauthorized();

        var token = new
        {
            AccessToken = _tokenService.GenerateToken(foundAccount),
        };
        
        return Ok(token);
    }
}