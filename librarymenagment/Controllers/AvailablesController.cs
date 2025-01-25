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
    public class AvailablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvailablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Availables
        public async Task<IActionResult> Index()
        {
            return View(await _context.Available.ToListAsync());
        }
        public async Task<IActionResult> Index(string sortOrder, string searchName, string authorId,string status, string categoryId, int? pageNumber)
        {
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["StatusSortParam"] = sortOrder == "status" ? "status_desc" : "status";
            ViewData["CopiesSortParam"] = sortOrder == "copies" ? "copies_desc" : "copies";
            ViewData["AuthorSortParam"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["CategorySortParam"] = sortOrder == "category" ? "category_desc" : "category";
            ViewData["ActiveSortParam"] = sortOrder == "active" ? "active_desc" : "active";

            ViewData["CurrentTitleFilter"] = searchName;
            ViewData["CurrentStatusFilter"] = status;
            ViewData["CurrentCategoryFilter"] = categoryId;

            var available = from b in _context.Available
                        .Include(b => b.Name)
                      
                        select b;

            if (!String.IsNullOrEmpty(searchName))
            {
                available = available.Where(a => a.Name.Contains(searchName));
            }
            if (!String.IsNullOrEmpty(status))
            {
                available = available.Where(a => a.Status.Contains(status));
            }


            available = sortOrder switch
            {
                "name_desc" => available.OrderByDescending(a => a.Name),
                "description" => available.OrderBy(a => a.Status),
                "description_desc" => available.OrderByDescending(a => a.Status),

                _ => available.OrderBy(a => a Status)
            };
            

            var userBookPermissions = await _context.UserBookPermission.ToListAsync();

            var userBookPermissionDict = userBookPermissions
                .GroupBy(ubp => ubp.BookId)
                .ToDictionary(g => g.Key, g => g.Select(ubp => ubp.UserId).ToList());

            ViewBag.UserBookPermissions = userBookPermissionDict;

            return View(await PaginatedList<Available>.CreateAsync(available, pageNumber ?? 1));
        }


        // GET: Availables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var available = await _context.Available
                .FirstOrDefaultAsync(m => m.Id == id);
            if (available == null)
            {
                return NotFound();
            }

            return View(available);
        }

        // GET: Availables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Availables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DataPorosise,Statusi,Address,CreatedAt,UpdatedAt,Active")] Available available)
        {
            if (ModelState.IsValid)
            {
                _context.Add(available);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(available);
        }

        // GET: Availables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var available = await _context.Available.FindAsync(id);
            if (available == null)
            {
                return NotFound();
            }
            return View(available);
        }

        // POST: Availables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DataPorosise,Statusi,Address,CreatedAt,UpdatedAt,Active")] Available available)
        {
            if (id != available.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(available);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvailableExists(available.Id))
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
            return View(available);
        }

        // GET: Availables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var available = await _context.Available
                .FirstOrDefaultAsync(m => m.Id == id);
            if (available == null)
            {
                return NotFound();
            }

            return View(available);
        }

        // POST: Availables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var available = await _context.Available.FindAsync(id);
            if (available != null)
            {
                _context.Available.Remove(available);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvailableExists(int id)
        {
            return _context.Available.Any(e => e.Id == id);
        }
    }
}
