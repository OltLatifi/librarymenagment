using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using librarymenagment.Data;
using librarymenagment.Models;
using Microsoft.AspNetCore.Authorization;

namespace librarymenagment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ComentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Coments.Include(c => c.Book);
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
