using System.Security.Claims;
using librarymenagment.Data;
using librarymenagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace librarymenagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ApplicationDbContext context, ILogger<CommentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/<CommentsController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int bookId)
        {
            var comments = await _context.Coments
                .Include(c => c.User)
                .Where(c => c.BookId == bookId && c.Active)
                .ToListAsync();

            if (comments == null || !comments.Any())
            {
                return NotFound();
            }

            return Ok(comments);
        }
    }
}

