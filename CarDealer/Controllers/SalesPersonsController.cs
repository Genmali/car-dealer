using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarDealer.Data;
using CarDealer.Models;

namespace CarDealer.Controllers
{
    public class SalesPersonsController : Controller
    {
        private readonly CarDealerContext _context;

        public SalesPersonsController(CarDealerContext context)
        {
            _context = context;
        }

        // GET: SalesPersons
        public async Task<IActionResult> Index()
        {
            var salesPersons = from s in _context.SalesPerson
                               select s;

            return View(await salesPersons.ToListAsync());
        }

        // GET: SalesPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesPerson = await _context.SalesPerson
                .FirstOrDefaultAsync(m => m.SalesPersonId == id);
            if (salesPerson == null)
            {
                return NotFound();
            }

            return View(salesPerson);
        }

        // GET: SalesPersons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesPersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesPersonId,Name,JobTitle,Address,Salary")] SalesPerson salesPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesPerson);
        }
    }
}
