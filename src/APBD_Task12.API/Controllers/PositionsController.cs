using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_Task10;
using APBD_Task10.Models.DTOs;
using APBD_Task10.Services;
using Microsoft.AspNetCore.Authorization;

namespace APBD_Task10.Controllers
{
    [Route("api/positions")]
    [ApiController]
    [Authorize]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly ILogger<PositionsController> _logger;

        public PositionsController(IPositionService service, ILogger<PositionsController> logger)
        {
            _positionService = service;
            _logger = logger;
        }

        // GET: api/Positions
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShortPositionDTO>>> GetPositions()
        {
            _logger.LogInformation("GET /api/positions was called in PositionsController");
            try
            {
                var result = await _positionService.GetPositions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in GET /api/positions in PositionsController: {0}", ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
