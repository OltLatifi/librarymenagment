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

        // POST api/<CommentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Coments comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID is missing from claims.");
                }

                comment.UserId = userId;
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }

            _context.Coments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { bookId = comment.BookId }, comment);
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Coments comment)
        {
            comment.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID is missing from claims.");
                }

                comment.UserId = userId;
                _context.Entry(comment).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!_context.Coments.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Concurrency error occurred while updating comment.");
                        throw;
                    }
                }

                return NoContent();
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var comment = await _context.Coments.FindAsync(id);
                if (comment == null)
                {
                    return NotFound();
                }

                _context.Coments.Remove(comment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }
    }
}

