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
using System.Drawing.Printing;

namespace librarymenagment.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchTitle, string authorId, string categoryId, int? pageNumber)
        {
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DescriptionSortParam"] = sortOrder == "description" ? "description_desc" : "description";
            ViewData["CopiesSortParam"] = sortOrder == "copies" ? "copies_desc" : "copies";
            ViewData["AuthorSortParam"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["CategorySortParam"] = sortOrder == "category" ? "category_desc" : "category";
            ViewData["ActiveSortParam"] = sortOrder == "active" ? "active_desc" : "active";

            ViewData["CurrentTitleFilter"] = searchTitle;
            ViewData["CurrentAuthorFilter"] = authorId;
            ViewData["CurrentCategoryFilter"] = categoryId;

            var books = from b in _context.Book
                        .Include(b => b.Author)
                        .Include(b => b.Category)
                        where b.Active
                        select b;

            if (!String.IsNullOrEmpty(searchTitle))
            {
                books = books.Where(b => b.Title.Contains(searchTitle));
            }
            if (!String.IsNullOrEmpty(authorId))
            {
                int authorIdInt = int.Parse(authorId);
                books = books.Where(b => b.AuthorId == authorIdInt);
            }
            if (!String.IsNullOrEmpty(categoryId))
            {
                int categoryIdInt = int.Parse(categoryId);
                books = books.Where(b => b.CategoryId == categoryIdInt);
            }

            books = sortOrder switch
            {
                "title_desc" => books.OrderByDescending(b => b.Title),
                "description" => books.OrderBy(b => b.Description),
                "description_desc" => books.OrderByDescending(b => b.Description),
                "copies" => books.OrderBy(b => b.Copies),
                "copies_desc" => books.OrderByDescending(b => b.Copies),
                "author" => books.OrderBy(b => b.Author.Name),
                "author_desc" => books.OrderByDescending(b => b.Author.Name),
                "category" => books.OrderBy(b => b.Category.Name),
                "category_desc" => books.OrderByDescending(b => b.Category.Name),
                "active" => books.OrderBy(b => b.Active),
                "active_desc" => books.OrderByDescending(b => b.Active),
                _ => books.OrderBy(b => b.Title),
            };
            var authors = _context.Author.ToList();
            ViewBag.Author = authors;
            var categories = _context.Category.ToList();
            ViewBag.Category = categories;

            return View(await PaginatedList<Book>.CreateAsync(books, pageNumber ?? 1));
        }   

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            var authors = await _context.Author.Where(a => a.Active).ToListAsync();
            var categories = await _context.Category.Where(c => c.Active).ToListAsync();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Copies,AuthorId,CategoryId,Active")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Active = true;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var authors = await _context.Author.Where(a => a.Active).ToListAsync();
            var categories = await _context.Category.Where(c => c.Active).ToListAsync();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", book.CategoryId);

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var authors = await _context.Author.Where(a => a.Active).ToListAsync();
            var categories = await _context.Category.Where(c => c.Active).ToListAsync();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Copies,AuthorId,CategoryId,Active")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            var authors = await _context.Author.Where(a => a.Active).ToListAsync();
            var categories = await _context.Category.Where(c => c.Active).ToListAsync();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
