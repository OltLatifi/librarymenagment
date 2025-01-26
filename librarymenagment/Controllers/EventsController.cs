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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> EventsIndex(string sortOrder, int? searchMemberId, int? searchBookId, DateTime? startDate, DateTime? endDate, bool? isReturned, int? pageNumber)
        {
           
            ViewData["MemberSortParam"] = String.IsNullOrEmpty(sortOrder) ? "member_desc" : "";
            ViewData["BookSortParam"] = sortOrder == "book" ? "book_desc" : "book";
            ViewData["LoanDateSortParam"] = sortOrder == "loanDate" ? "loanDate_desc" : "loanDate";
            ViewData["ReturnDateSortParam"] = sortOrder == "returnDate" ? "returnDate_desc" : "returnDate";

            
            ViewData["CurrentMemberFilter"] = searchMemberId;
            ViewData["CurrentBookFilter"] = searchBookId;
            ViewData["CurrentStartDateFilter"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentEndDateFilter"] = endDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentIsReturnedFilter"] = isReturned;

            
            var events = from l in _context.Loans
                         
                         select l;

            
            if (searchMemberId.HasValue)
            {
                events = events.Where(l => l.MemberId == searchMemberId);
            }

            if (searchBookId.HasValue)
            {
                events = events.Where(l => l.BookId == searchBookId);
            }

           
            if (startDate.HasValue)
            {
                events = events.Where(l => l.LoanDate >= startDate);
            }
            if (endDate.HasValue)
            {
                events = events.Where(l => l.LoanDate <= endDate);
            }

            if (isReturned.HasValue)
            {
                events = events.Where(l => l.ReturnDate.HasValue == isReturned.Value);
            }

           
            events = sortOrder switch
            {
                "member_desc" => events.OrderByDescending(l => l.MemberId),
                "book" => events.OrderBy(l => l.BookId),
                "book_desc" => events.OrderByDescending(l => l.BookId),
                "loanDate" => events.OrderBy(l => l.LoanDate),
                "loanDate_desc" => events.OrderByDescending(l => l.LoanDate),
                "returnDate" => events.OrderBy(l => l.ReturnDate),
                "returnDate_desc" => events.OrderByDescending(l => l.ReturnDate),
                _ => events.OrderBy(l => l.LoanDate),
            };

            
            var members = _context.Member.ToList();
            ViewBag.Member = members;
            var books = _context.Book.ToList();
            ViewBag.Book = books;

            
            return View(await PaginatedList<Loans>.CreateAsync(events, pageNumber ?? 1));
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate,EndDate,CreatedAt,UpdatedAt,Active")] Events events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,CreatedAt,UpdatedAt,Active")] Events events)
        {
            if (id != events.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsExists(events.Id))
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
            return View(events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events != null)
            {
                _context.Events.Remove(events);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
