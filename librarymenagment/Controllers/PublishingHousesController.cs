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
    public class PublishingHousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublishingHousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PublishingHouseIndex(string sortOrder, string searchName, string searchAddress, int? pageNumber)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "foundedDate" ? "foundedDate_desc" : "foundedDate";
            ViewData["AddressSortParam"] = sortOrder == "address" ? "address_desc" : "address";

            ViewData["CurrentNameFilter"] = searchName;
            ViewData["CurrentAddressFilter"] = searchAddress;

            var publishingHouses = from ph in _context.PublishingHouses
                                   .Include(ph => ph.Authors)
                                   .Include(ph => ph.PublishedBooks)
                                   select ph;

            if (!String.IsNullOrEmpty(searchName))
            {
                publishingHouses = publishingHouses.Where(ph => ph.Name.Contains(searchName));
            }
            if (!String.IsNullOrEmpty(searchAddress))
            {
                publishingHouses = publishingHouses.Where(ph => ph.Address.Contains(searchAddress));
            }

            publishingHouses = sortOrder switch
            {
                "name_desc" => publishingHouses.OrderByDescending(ph => ph.Name),
                "foundedDate" => publishingHouses.OrderBy(ph => ph.FoundedDate),
                "foundedDate_desc" => publishingHouses.OrderByDescending(ph => ph.FoundedDate),
                "address" => publishingHouses.OrderBy(ph => ph.Address),
                "address_desc" => publishingHouses.OrderByDescending(ph => ph.Address),
                _ => publishingHouses.OrderBy(ph => ph.Name),
            };

            return View(await PaginatedList<PublishingHouse>.CreateAsync(publishingHouses, pageNumber ?? 1));
        }


        // GET: PublishingHouses
        public async Task<IActionResult> Index()
        {
            return View(await _context.PublishingHouses.Where(p => p.Active == true).ToListAsync());
        }
        

        // GET: PublishingHouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publishingHouse = await _context.PublishingHouses
                .FirstOrDefaultAsync(m => m.Id == id && m.Active == true);
            if (publishingHouse == null)
            {
                return NotFound();
            }

            return View(publishingHouse);
        }

        // GET: PublishingHouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublishingHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FoundedDate,Name,Address,CreatedAt,UpdatedAt,Active")] PublishingHouse publishingHouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publishingHouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publishingHouse);
        }

        // GET: PublishingHouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publishingHouse = await _context.PublishingHouses.FindAsync(id);
            if (publishingHouse == null)
            {
                return NotFound();
            }
            return View(publishingHouse);
        }

        // POST: PublishingHouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FoundedDate,Name,Address,CreatedAt,UpdatedAt,Active")] PublishingHouse publishingHouse)
        {
            if (id != publishingHouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publishingHouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublishingHouseExists(publishingHouse.Id))
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
            return View(publishingHouse);
        }

        // GET: PublishingHouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publishingHouse = await _context.PublishingHouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publishingHouse == null)
            {
                return NotFound();
            }

            return View(publishingHouse);
        }

        // POST: PublishingHouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publishingHouse = await _context.PublishingHouses.FindAsync(id);
            if (publishingHouse != null)
            {
                _context.PublishingHouses.Remove(publishingHouse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublishingHouseExists(int id)
        {
            return _context.PublishingHouses.Any(e => e.Id == id);
        }
    }
}
