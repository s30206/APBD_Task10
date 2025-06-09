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

namespace APBD_Task10.Controllers
{
    [Route("api/positions")]
    [ApiController]
    [Authorize]
    public class PositionsController : ControllerBase
    {
        private readonly DeviceContext _context;

        public PositionsController(DeviceContext context)
        {
            _context = context;
        }

        // GET: api/Positions
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShortPositionDTO>>> GetPositions()
        {
            try
            {
                var positions = await _context.Positions.ToListAsync();

                var result = new List<ShortPositionDTO>();

                foreach (var position in positions)
                {
                    result.Add(new ShortPositionDTO()
                    {
                        Id = position.Id,
                        Name = position.Name,
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
