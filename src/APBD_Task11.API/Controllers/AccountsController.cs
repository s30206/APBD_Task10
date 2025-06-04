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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ShortAccountDTO>>> GetAccounts()
    {
        try
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
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // GET: api/Accounts/5
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ShortAccountDTO>> GetAccount(int id)
    {
        try
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            // I have no idea why roles are accessed this way, but it works :D
            if (User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value != "Admin" &&
                int.Parse(User.FindFirst("id")?.Value) != account.Id)
            {
                return BadRequest("Not admins can check only their own data");
            }

            return new ShortAccountDTO()
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.Password
            };
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // PUT: api/Accounts/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> PutAccount(int id, UpdateAccountDTO newAccount)
    {
        try
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account is null || newAccount.Username != account.Username)
            {
                return BadRequest();
            }

            if (User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value != "Admin" &&
                newAccount.RoleId is not null)
            {
                return BadRequest("Not admins cannot change roles for accounts");
            }

            if (User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value != "Admin" &&
                account.Username != User.FindFirst("username").Value)
            {
                return BadRequest("Not admins cannot modify not their account");
            }


            account.Username = newAccount.Username;
            account.Password = _passwordHasher.HashPassword(account, newAccount.Password);
            account.RoleId = newAccount.RoleId ?? account.RoleId;

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
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // POST: api/Accounts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Account>> PostAccount([FromBody] CreateAccountDTO newAccount)
    {
        try
        {
            if (await _context.Accounts.Where(a => a.Username == newAccount.Username).FirstOrDefaultAsync() != null)
                return BadRequest("This username is taken");

            if (await _context.Roles.FindAsync(newAccount.RoleId) == null)
                return BadRequest("This role id doesn't exist. Possible values: 1 (Admin), 2(User)");

            var account = new Account
            {
                Username = newAccount.Username,
                Password = newAccount.Password,
                EmployeeId = newAccount.EmployeeId,
                RoleId = newAccount.RoleId ?? 2
            };

            account.Password = _passwordHasher.HashPassword(account, newAccount.Password);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, new ShortAccountDTO()
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.Password
            });
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // DELETE: api/Accounts/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        try
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
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    private bool AccountExists(int id)
    {
        return _context.Accounts.Any(e => e.Id == id);
    }
}