﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using librarymenagment.Data;
using librarymenagment.Models;
using Microsoft.AspNetCore.Authorization;
using librarymenagment.Helpers;

namespace librarymenagment.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index(string search, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LastNameSortParam"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewData["CreatedAtSortParam"] = sortOrder == "createdAt" ? "createdAt_desc" : "createdAt";
            ViewData["UpdatedAtSortParam"] = sortOrder == "updatedAt" ? "updatedAt_desc" : "updatedAt";
            ViewData["ActiveSortParam"] = sortOrder == "active" ? "active_desc" : "active";
            
            var authors = from a in _context.Author
                         where a.Active
                         select a;

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



        // GET: Authors/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id && m.Active == true);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
    }
}
