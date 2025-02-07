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

namespace librarymenagment
{
    public class AvailablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvailablesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchName, string statusFilter, DateTime? orderDateFilter, int? pageNumber)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["StatusSortParam"] = sortOrder == "status" ? "status_desc" : "status";

            ViewData["CurrentNameFilter"] = searchName;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentOrderDateFilter"] = orderDateFilter;

            var availables = from a in _context.Available
                             .Include(a => a.PublishedBooks)
                             select a;

            // Apply search filters
            if (!String.IsNullOrEmpty(searchName))
            {
                availables = availables.Where(a => a.Name.Contains(searchName));
            }
            if (!String.IsNullOrEmpty(statusFilter))
            {
                availables = availables.Where(a => a.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }
            if (orderDateFilter.HasValue)
            {
                availables = availables.Where(a => a.DataPorosise.Date == orderDateFilter.Value.Date);
            }

            // Apply sorting
            availables = sortOrder switch
            {
                "name_desc" => availables.OrderByDescending(a => a.Name),
                "date" => availables.OrderBy(a => a.DataPorosise),
                "date_desc" => availables.OrderByDescending(a => a.DataPorosise),
                "status" => availables.OrderBy(a => a.Status),
                "status_desc" => availables.OrderByDescending(a => a.Status),
                _ => availables.OrderBy(a => a.Name),
            };

            // Pass data for dropdowns or additional UI options
            var statuses = availables.Select(a => a.Status).Distinct().ToList();
            ViewBag.Statuses = statuses;

            return View(await PaginatedList<Available>.CreateAsync(availables, pageNumber ?? 1));
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
        public async Task<IActionResult> Create([Bind("Id,Name,DataPorosise,Status,Address")] Available available)
        {
                _context.Add(available);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DataPorosise,Status,Address")] Available available)
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
