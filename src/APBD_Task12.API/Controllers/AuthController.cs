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
    private readonly ILogger<AuthController> _logger;

    public AuthController(DeviceContext context, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] AccountLoginDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("POST /api/auth was called in AuthController");
        try
        {
            var foundAccount = await _context.Accounts.Include(r => r.Role)
                .FirstOrDefaultAsync(a => a.Username == request.Login, cancellationToken);

            if (foundAccount == null)
                return Unauthorized();

            var verification =
                _passwordHasher.VerifyHashedPassword(foundAccount, foundAccount.Password, request.Password);

            if (verification == PasswordVerificationResult.Failed)
                return Unauthorized();

            var token = new
            {
                AccessToken = _tokenService.GenerateToken(foundAccount),
            };

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in POST /api/auth in AuthController: {0}", ex.Message);
            return Problem(ex.Message);
        }
    }
}