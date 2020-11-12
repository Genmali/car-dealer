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
    public class CarPurchasesController : Controller
    {
        private readonly CarDealerContext _context;

        public CarPurchasesController(CarDealerContext context)
        {
            _context = context;
        }

        // GET: CarPurchases
        public async Task<IActionResult> Index(string carMake, string carModel, string salesPerson)
        {
            var carPurchases = from p in _context.CarPurchase
                               select p;

            // Filters. Add more here if desired.
            if (!String.IsNullOrEmpty(carMake))
            {
                carPurchases = carPurchases.Where(c => c.Car.Make.Contains(carMake));
            }
            if (!String.IsNullOrEmpty(carModel))
            {
                carPurchases = carPurchases.Where(c => c.Car.Model.Contains(carModel));
            }
            if (!String.IsNullOrEmpty(salesPerson))
            {
                carPurchases = carPurchases.Where(c => c.SalesPerson.Name.Contains(salesPerson)).OrderBy(c => c.SalesPerson.Name);
            }

            carPurchases = carPurchases.Include(c => c.Car).Include(c => c.Customer).Include(c => c.SalesPerson);

            return View(await carPurchases.ToListAsync());
        }

        // GET: CarPurchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPurchase = await _context.CarPurchase
                .Include(c => c.Car)
                .Include(c => c.Customer)
                .Include(c => c.SalesPerson)
                .FirstOrDefaultAsync(m => m.CarPurchaseId == id);
            if (carPurchase == null)
            {
                return NotFound();
            }

            return View(carPurchase);
        }

        // Formats the car and customer data for better displaying in the view
        private (IQueryable, IQueryable) GetCarAndCustomerInfo()
        {
            var car = from c in _context.Car
                      orderby c.Make
                      select new
                      {
                          CarId = c.CarId,
                          Name = c.Make + " " + c.Model
                      };
            var customer = from c in _context.Customer
                           orderby c.Name
                           select new
                           {
                               CustomerId = c.CustomerId,
                               Name = c.Name + " " + c.Surname
                           };

            return (car, customer);
        }

        // GET: CarPurchases/Create
        public IActionResult Create()
        {
            var (car, customer) = GetCarAndCustomerInfo();

            ViewBag.CarId = new SelectList(car, "CarId", "Name");
            ViewBag.CustomerId = new SelectList(customer, "CustomerId", "Name");
            ViewBag.SalesPersonId = new SelectList(_context.SalesPerson, "SalesPersonId", "Name");
            return View();
        }

        // POST: CarPurchases/Create
        // To protect from overposting attacks.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarPurchaseId,CustomerId,CarId,OrderDate,PricePaid,SalesPersonId")] CarPurchase carPurchase)
        {
            if (ModelState.IsValid)
            {
                _context.CarPurchase.Add(carPurchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var (car, customer) = GetCarAndCustomerInfo();

            ViewBag.CarId = new SelectList(car, "CarId", "Name", carPurchase.CarId);
            ViewBag.CustomerId = new SelectList(customer, "CustomerId", "Name", carPurchase.CustomerId);
            ViewBag.SalesPersonId = new SelectList(_context.SalesPerson, "SalesPersonId", "Name", carPurchase.SalesPersonId);
            return View(carPurchase);
        }

        // GET: CarPurchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPurchase = await _context.CarPurchase.FindAsync(id);
            if (carPurchase == null)
            {
                return NotFound();
            }
            var (car, customer) = GetCarAndCustomerInfo();

            ViewBag.CarId = new SelectList(car, "CarId", "Name", carPurchase.CarId);
            ViewBag.CustomerId = new SelectList(customer, "CustomerId", "Name", carPurchase.CustomerId);
            ViewBag.SalesPersonId = new SelectList(_context.SalesPerson, "SalesPersonId", "Name", carPurchase.SalesPersonId);
            return View(carPurchase);
        }

        // POST: CarPurchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarPurchaseId,CustomerId,CarId,OrderDate,PricePaid,SalesPersonId")] CarPurchase carPurchase)
        {
            if (id != carPurchase.CarPurchaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carPurchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarPurchaseExists(carPurchase.CarPurchaseId))
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
            var (car, customer) = GetCarAndCustomerInfo();

            ViewBag.CarId = new SelectList(car, "CarId", "Name", carPurchase.CarId);
            ViewBag.CustomerId = new SelectList(customer, "CustomerId", "Name", carPurchase.CustomerId);
            ViewBag.SalesPersonId = new SelectList(_context.SalesPerson, "SalesPersonId", "Name", carPurchase.SalesPersonId);
            return View(carPurchase);
        }

        // GET: CarPurchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPurchase = await _context.CarPurchase
                .Include(c => c.Car)
                .Include(c => c.Customer)
                .Include(c => c.SalesPerson)
                .FirstOrDefaultAsync(m => m.CarPurchaseId == id);
            if (carPurchase == null)
            {
                return NotFound();
            }

            return View(carPurchase);
        }

        // POST: CarPurchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carPurchase = await _context.CarPurchase.FindAsync(id);
            _context.CarPurchase.Remove(carPurchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarPurchaseExists(int id)
        {
            return _context.CarPurchase.Any(e => e.CarPurchaseId == id);
        }
    }
}
