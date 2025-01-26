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
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, int? searchMemberId, int? searchBookId, DateTime? startDate, DateTime? endDate, bool? isReturned, int? pageNumber)
        {
            // Parametra për filtrimin dhe renditjen
            ViewData["MemberSortParam"] = String.IsNullOrEmpty(sortOrder) ? "member_desc" : "";
            ViewData["BookSortParam"] = sortOrder == "book" ? "book_desc" : "book";
            ViewData["LoanDateSortParam"] = sortOrder == "loanDate" ? "loanDate_desc" : "loanDate";
            ViewData["ReturnDateSortParam"] = sortOrder == "returnDate" ? "returnDate_desc" : "returnDate";

            // Parametra për filtrat aktualë
            ViewData["CurrentMemberFilter"] = searchMemberId;
            ViewData["CurrentBookFilter"] = searchBookId;
            ViewData["CurrentStartDateFilter"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentEndDateFilter"] = endDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentIsReturnedFilter"] = isReturned;

            // Fillimi i lista e loans nga baza e të dhënave dhe përdorimi i Include për Member dhe Book
            var loans = from l in _context.Loans
                        .Include(l => l.MemberId)  // Këtu lidhim Member me Loans
                        .Include(l => l.BookId)    // Këtu lidhim Book me Loans
                        select l;

            // Filtrimi për MemberId
            if (searchMemberId.HasValue)
            {
                loans = loans.Where(l => l.MemberId == searchMemberId);
            }

            // Filtrimi për BookId
            if (searchBookId.HasValue)
            {
                loans = loans.Where(l => l.BookId == searchBookId);
            }

            // Filtrimi për LoanDate
            if (startDate.HasValue)
            {
                loans = loans.Where(l => l.LoanDate >= startDate);
            }
            if (endDate.HasValue)
            {
                loans = loans.Where(l => l.LoanDate <= endDate);
            }

            // Filtrimi për ReturnDate
            if (isReturned.HasValue)
            {
                loans = loans.Where(l => l.ReturnDate.HasValue == isReturned.Value);
            }

            // Sortimi sipas kërkesës
            loans = sortOrder switch
            {
                "member_desc" => loans.OrderByDescending(l => l.MemberId),
                "book" => loans.OrderBy(l => l.BookId),
                "book_desc" => loans.OrderByDescending(l => l.BookId),
                "loanDate" => loans.OrderBy(l => l.LoanDate),
                "loanDate_desc" => loans.OrderByDescending(l => l.LoanDate),
                "returnDate" => loans.OrderBy(l => l.ReturnDate),
                "returnDate_desc" => loans.OrderByDescending(l => l.ReturnDate),
                _ => loans.OrderBy(l => l.LoanDate),
            };

            // Marrja e të dhënave për anëtarët dhe librat për të shfaqur në ViewBag
            var members = _context.Member.ToList();
            ViewBag.Member = members;
            var books = _context.Book.ToList();
            ViewBag.Book = books;

            // Pagina dhe kthimi i të dhënave të filtruar dhe të renditura
            return View(await PaginatedList<Loans>.CreateAsync(loans, pageNumber ?? 1));
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Loans.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loans = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loans == null)
            {
                return NotFound();
            }

            return View(loans);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,BookId,LoanDate,ReturnDate,CreatedAt,UpdatedAt,Active")] Loans loans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loans);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loans = await _context.Loans.FindAsync(id);
            if (loans == null)
            {
                return NotFound();
            }
            return View(loans);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,BookId,LoanDate,ReturnDate,CreatedAt,UpdatedAt,Active")] Loans loans)
        {
            if (id != loans.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loans);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoansExists(loans.Id))
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
            return View(loans);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loans = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loans == null)
            {
                return NotFound();
            }

            return View(loans);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loans = await _context.Loans.FindAsync(id);
            if (loans != null)
            {
                _context.Loans.Remove(loans);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoansExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
