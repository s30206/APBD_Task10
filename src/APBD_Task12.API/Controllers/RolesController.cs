using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_Task10;
using APBD_Task10.Models.DTOs.Roles;
using APBD_Task10.Services;
using Microsoft.AspNetCore.Authorization;

namespace APBD_Task10.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService service, ILogger<RolesController> logger)
        {
            _roleService = service;
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShortRoleDTO>>> GetRoles()
        {
            _logger.LogInformation("GET /api/roles was called in RolesController");
            try
            {
                var result = await _roleService.GetRoles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in GET /api/roles in RolesController: {0}", ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
