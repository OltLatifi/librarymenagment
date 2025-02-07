﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using librarymenagment.Data;
using librarymenagment.Models;
using librarymenagment.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace librarymenagment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string sortOrder, bool? activeOnly, int? pageNumber)
        {
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentActiveFilter"] = activeOnly;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CreatedAtSortParam"] = sortOrder == "createdAt" ? "createdAt_desc" : "createdAt";
            ViewData["UpdatedAtSortParam"] = sortOrder == "updatedAt" ? "updatedAt_desc" : "updatedAt";
            ViewData["ActiveSortParam"] = sortOrder == "active" ? "active_desc" : "active";

            var categories = from a in _context.Category
                    select a;

            if (activeOnly == true)
            {
                categories = categories.Where(a => a.Active);
            }

            if (!String.IsNullOrEmpty(search))
            {
                categories = categories.Where(a =>
                    a.Name.Contains(search)
                );
            }

            categories = sortOrder switch
            {
                "name_desc" => categories.OrderByDescending(c => c.Name),
                "createdAt" => categories.OrderBy(c => c.CreatedAt),
                "createdAt_desc" => categories.OrderByDescending(c => c.CreatedAt),
                "updatedAt" => categories.OrderBy(c => c.UpdatedAt),
                "updatedAt_desc" => categories.OrderByDescending(c => c.UpdatedAt),
                "active" => categories.OrderBy(c => c.Active),
                "active_desc" => categories.OrderByDescending(c => c.Active),
                _ => categories.OrderBy(c => c.Name),
            };

            return View(await PaginatedList<Category>.CreateAsync(categories, pageNumber ?? 1));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Active")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Active")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
