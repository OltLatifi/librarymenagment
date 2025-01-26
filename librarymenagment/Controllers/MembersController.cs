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
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> MembersIndex(string sortOrder, string searchFirstName, string searchLastName, string membershipType, bool? isActive, int? pageNumber)
        {
            
            ViewData["FirstNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "firstName_desc" : "";
            ViewData["LastNameSortParam"] = sortOrder == "lastName" ? "lastName_desc" : "lastName";
            ViewData["DateOfBirthSortParam"] = sortOrder == "dob" ? "dob_desc" : "dob";
            ViewData["MembershipTypeSortParam"] = sortOrder == "membershipType" ? "membershipType_desc" : "membershipType";
            ViewData["IsActiveSortParam"] = sortOrder == "isActive" ? "isActive_desc" : "isActive";

           
            ViewData["CurrentFirstNameFilter"] = searchFirstName;
            ViewData["CurrentLastNameFilter"] = searchLastName;
            ViewData["CurrentMembershipTypeFilter"] = membershipType;
            ViewData["CurrentActiveFilter"] = isActive;

            var members = from m in _context.Member
                          select m;

           
            if (!String.IsNullOrEmpty(searchFirstName))
            {
                members = members.Where(m => m.FirstName.Contains(searchFirstName));
            }

            if (!String.IsNullOrEmpty(searchLastName))
            {
                members = members.Where(m => m.LastName.Contains(searchLastName));
            }

            if (!String.IsNullOrEmpty(membershipType))
            {
                members = members.Where(m => m.MembershipType.Contains(membershipType));
            }

            
            if (isActive.HasValue)
            {
                members = members.Where(m => m.IsActive == isActive.Value);
            }

            
            members = sortOrder switch
            {
                "firstName_desc" => members.OrderByDescending(m => m.FirstName),
                "lastName" => members.OrderBy(m => m.LastName),
                "lastName_desc" => members.OrderByDescending(m => m.LastName),
                "dob" => members.OrderBy(m => m.DateOfBirth),
                "dob_desc" => members.OrderByDescending(m => m.DateOfBirth),
                "membershipType" => members.OrderBy(m => m.MembershipType),
                "membershipType_desc" => members.OrderByDescending(m => m.MembershipType),
                "isActive" => members.OrderBy(m => m.IsActive),
                "isActive_desc" => members.OrderByDescending(m => m.IsActive),
                _ => members.OrderBy(m => m.FirstName),
            };

            
            return View(await PaginatedList<Member>.CreateAsync(members, pageNumber ?? 1));
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Address,MembershipDate,MembershipType,IsActive,MaxBorrowLimit,CreatedAt,UpdatedAt,Active")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,Email,PhoneNumber,Address,MembershipDate,MembershipType,IsActive,MaxBorrowLimit,CreatedAt,UpdatedAt,Active")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }
    }
}
