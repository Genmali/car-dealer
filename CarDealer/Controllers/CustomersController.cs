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
    public class CustomersController : Controller
    {
        private readonly CarDealerContext _context;

        public CustomersController(CarDealerContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string nameSearch, string addressSearch)
        {
            var customers = from c in _context.Customer
                            select c;

            // Filters. Add more here if desired.
            if (!String.IsNullOrEmpty(nameSearch))
            {
                customers = customers.Where(c => c.Name.Contains(nameSearch));
            }
            if (!String.IsNullOrEmpty(addressSearch))
            {
                customers = customers.Where(c => c.Address.Contains(addressSearch));
            }

            return View(await customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,Name,Surname,Age,Address,Created")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}
