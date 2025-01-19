using librarymenagment.Data;
using librarymenagment.Helpers;
using librarymenagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace librarymenagment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LastNameSortParam"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewData["CreatedAtSortParam"] = sortOrder == "createdAt" ? "createdAt_desc" : "createdAt";
            ViewData["UpdatedAtSortParam"] = sortOrder == "updatedAt" ? "updatedAt_desc" : "updatedAt";
            ViewData["ActiveSortParam"] = sortOrder == "active" ? "active_desc" : "active";

            var authors = from a in _context.Author select a;

            if (!String.IsNullOrEmpty(search))
            {
                authors = authors.Where(a =>
                    a.Name.Contains(search) ||
                    a.LastName.Contains(search) ||
                    (a.Name + " " + a.LastName).Contains(search)
                );
            }

            authors = sortOrder switch
            {
                "name_desc" => authors.OrderByDescending(a => a.Name),
                "lastname" => authors.OrderBy(a => a.LastName),
                "lastname_desc" => authors.OrderByDescending(a => a.LastName),
                "createdAt" => authors.OrderBy(a => a.CreatedAt),
                "createdAt_desc" => authors.OrderByDescending(a => a.CreatedAt),
                "updatedAt" => authors.OrderBy(a => a.UpdatedAt),
                "updatedAt_desc" => authors.OrderByDescending(a => a.UpdatedAt),
                "active" => authors.OrderBy(a => a.Active),
                "active_desc" => authors.OrderByDescending(a => a.Active),
                _ => authors.OrderBy(a => a.Name),
            };

            return View(await PaginatedList<Author>.CreateAsync(authors, pageNumber ?? 1));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Active")] Author author)
        {
            if (ModelState.IsValid)
            {
                author.Active = true;
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Active")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author != null)
            {
                _context.Author.Remove(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.Id == id);
        }
    }
}