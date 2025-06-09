using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_Task10;
using APBD_Task10.Models.DTOs.Roles;
using Microsoft.AspNetCore.Authorization;

namespace APBD_Task10.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly DeviceContext _context;

        public RolesController(DeviceContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShortRoleDTO>>> GetRoles()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();

                var result = new List<ShortRoleDTO>();

                foreach (var role in roles)
                {
                    result.Add(new ShortRoleDTO()
                    {
                        Id = role.Id,
                        Name = role.Name
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
