using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_Task10;
using APBD_Task10.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace APBD_Task10.Controllers;

[Route("api/accounts")]
[Authorize(Roles = "Admin")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly DeviceContext _context;
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public AccountsController(DeviceContext context)
    {
        _context = context;
    }

    // GET: api/Accounts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShortAccountDTO>>> GetAccounts()
    {
        var accounts = await _context.Accounts.ToListAsync();

        var result = new List<ShortAccountDTO>();
        
        foreach (var account in accounts)
        {
            result.Add(new ShortAccountDTO()
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.Password
            });
        }

        return result;
    }

    // GET: api/Accounts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShortAccountDTO>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        return new ShortAccountDTO()
        {
            Id = account.Id,
            Username = account.Username,
            Password = account.Password
        };
    }

    // PUT: api/Accounts/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAccount(int id, CreateAccountDTO newAccount)
    {
        var account = await _context.Accounts.FindAsync(id);
        
        if (account is null || newAccount.Username != account.Username || newAccount.EmployeeId != account.EmployeeId)
        {
            return BadRequest();
        }

        account.Username = newAccount.Username;
        account.Password = _passwordHasher.HashPassword(account, newAccount.Password);

        _context.Entry(account).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Accounts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Account>> PostAccount([FromBody] CreateAccountDTO newAccount)
    {
        var account = new Account
        {
            Username = newAccount.Username,
            Password = newAccount.Password,
            EmployeeId = newAccount.EmployeeId,
            RoleId = 1
        };
        
        account.Password = _passwordHasher.HashPassword(account, newAccount.Password);
        
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAccount", new { id = account.Id }, account);
    }

    // DELETE: api/Accounts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AccountExists(int id)
    {
        return _context.Accounts.Any(e => e.Id == id);
    }
}