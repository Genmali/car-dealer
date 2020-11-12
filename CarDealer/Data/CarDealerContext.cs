using Microsoft.EntityFrameworkCore;
using CarDealer.Models;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext(DbContextOptions<CarDealerContext> options) : base(options)
        {
        }

        public DbSet<Car> Car { get; set; }
        public DbSet<CarPurchase> CarPurchase { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<SalesPerson> SalesPerson { get; set; }
    }
}
