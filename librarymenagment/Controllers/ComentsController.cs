using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using librarymenagment.Data;
using librarymenagment.Models;
using librarymenagment.Helpers;

namespace librarymenagment.Controllers
{
    public class ComentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CommentsIndex(string sortOrder, string searchDescription, string userId, int? bookId, int? pageNumber)
        {
            ViewData["DescriptionSortParam"] = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewData["UserSortParam"] = sortOrder == "user" ? "user_desc" : "user";
            ViewData["BookSortParam"] = sortOrder == "book" ? "book_desc" : "book";

            ViewData["CurrentDescriptionFilter"] = searchDescription;
            ViewData["CurrentUserFilter"] = userId;
            ViewData["CurrentBookFilter"] = bookId;

            var comments = from c in _context.Coments
                           .Include(c => c.Book)
                           .Include(c => c.User)
                           select c;

            if (!String.IsNullOrEmpty(searchDescription))
            {
                comments = comments.Where(c => c.Description.Contains(searchDescription));
            }
            if (!String.IsNullOrEmpty(userId))
            {
                comments = comments.Where(c => c.UserId == userId);
            }
            if (bookId.HasValue)
            {
                comments = comments.Where(c => c.BookId == bookId.Value);
            }

            comments = sortOrder switch
            {
                "description_desc" => comments.OrderByDescending(c => c.Description),
                "user" => comments.OrderBy(c => c.User.UserName),
                "user_desc" => comments.OrderByDescending(c => c.User.UserName),
                "book" => comments.OrderBy(c => c.Book.Title),
                "book_desc" => comments.OrderByDescending(c => c.Book.Title),
                _ => comments.OrderBy(c => c.Description),
            };

            return View(await PaginatedList<Coments>.CreateAsync(comments, pageNumber ?? 1));
        }

        // GET: Coments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Coments.Include(c => c.Book).Where(c => c.Active ==  true);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Coments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coments = await _context.Coments
                .Include(c => c.Book)
                .FirstOrDefaultAsync(m => m.Id == id && m.Active == true);
            if (coments == null)
            {
                return NotFound();
            }

            return View(coments);
        }

        // GET: Coments/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Description");
            return View();
        }

        // POST: Coments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,UserId,BookId,CreatedAt,UpdatedAt,Active")] Coments coments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Description", coments.BookId);
            return View(coments);
        }

        // GET: Coments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coments = await _context.Coments.FindAsync(id);
            if (coments == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Description", coments.BookId);
            return View(coments);
        }

        // POST: Coments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,UserId,BookId,CreatedAt,UpdatedAt,Active")] Coments coments)
        {
            if (id != coments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentsExists(coments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Description", coments.BookId);
            return View(coments);
        }

        // GET: Coments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coments = await _context.Coments
                .Include(c => c.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coments == null)
            {
                return NotFound();
            }

            return View(coments);
        }

        // POST: Coments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coments = await _context.Coments.FindAsync(id);
            if (coments != null)
            {
                _context.Coments.Remove(coments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComentsExists(int id)
        {
            return _context.Coments.Any(e => e.Id == id);
        }
    }
}
