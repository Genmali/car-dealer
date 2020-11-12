using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using CarDealer.Models;

namespace CarDealer.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CarDealerContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CarDealerContext>>()))
            {
                // Look for existing data.
                if (!context.Car.Any())
                {
                    context.Car.AddRange(
                        new Car
                        {
                            Make = "Ford",
                            Model = "Mustang GTR",
                            Color = "Red",
                            Extras = "Aircon, Electric Steering, Electric Seats",
                            RecommendedPrice = 519999.95M
                        },

                        new Car
                        {
                            Make = "Nissan",
                            Model = "GTR",
                            Color = "Silver",
                            Extras = "Aircon, Electric Steering, Twin-Turbo",
                            RecommendedPrice = 439999.95M
                        },

                        new Car
                        {
                            Make = "Dodge",
                            Model = "Challenger",
                            Color = "Matte Black",
                            Extras = "Aircon, Supercharger",
                            RecommendedPrice = 559999.95M
                        },

                        new Car
                        {
                            Make = "Chevrolet",
                            Model = "Corvette Stingray",
                            Color = "White",
                            Extras = "Aircon, Electric Steering, Electric Seats, Supercharger",
                            RecommendedPrice = 809999.95M
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Customer.Any())
                {
                    context.Customer.AddRange(
                        new Customer
                        {
                            Name = "Jane",
                            Surname = "Halloway",
                            Age = 33,
                            Address = "Broadway Street 77, 10001, New York",
                            Created = true
                        },

                        new Customer
                        {
                            Name = "Christopher",
                            Surname = "Halloway",
                            Age = 34,
                            Address = "Broadway Street 77, 10001, New York",
                            Created = true
                        },

                        new Customer
                        {
                            Name = "Tom",
                            Surname = "Jackson",
                            Age = 51,
                            Address = "Broadway Street 104, 10001, New York",
                            Created = false
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.SalesPerson.Any())
                {
                    context.SalesPerson.AddRange(
                        new SalesPerson
                        {
                            Name = "Michael Takoiu",
                            JobTitle = "Sales Assistant",
                            Address = "Broadway Street 111, 10001, New York",
                            Salary = 10592M
                        },

                        new SalesPerson
                        {
                            Name = "Brian Tenning",
                            JobTitle = "Supervisor",
                            Address = "Broadway Street 63, 10001, New York",
                            Salary = 20020M
                        },

                        new SalesPerson
                        {
                            Name = "Louise York",
                            JobTitle = "CEO",
                            Address = "Wall Street 272, 10002, New York",
                            Salary = 100371M
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.CarPurchase.Any())
                {
                    var customer1 = from c in context.Customer
                                    where c.Name == "Christopher"
                                    select c.CustomerId;
                    var car1 = from c in context.Car
                               where c.Make == "Ford"
                               select c.CarId;
                    var sales1 = from s in context.SalesPerson
                                 where s.Name == "Michael Takoiu"
                                 select s.SalesPersonId;

                    var customer2 = from c in context.Customer
                                    where c.Name == "Jane"
                                    select c.CustomerId;
                    var car2 = from c in context.Car
                               where c.Make == "Nissan"
                               select c.CarId;
                    var sales2 = from s in context.SalesPerson
                                 where s.Name == "Brian Tenning"
                                 select s.SalesPersonId;

                    var customer3 = from c in context.Customer
                                    where c.Name == "Christopher"
                                    select c.CustomerId;
                    var car3 = from c in context.Car
                               where c.Make == "Chevrolet"
                               select c.CarId;
                    var sales3 = from s in context.SalesPerson
                                 where s.Name == "Michael Takoiu"
                                 select s.SalesPersonId;

                    context.CarPurchase.AddRange(
                    new CarPurchase
                        {
                            CustomerId = customer1.FirstOrDefault(),
                            CarId = car1.FirstOrDefault(),
                            OrderDate = DateTime.Parse("2019-2-12"),
                            PricePaid = 519999.95M,
                            SalesPersonId = sales1.FirstOrDefault()
                        },

                        new CarPurchase
                        {
                            CustomerId = customer2.FirstOrDefault(),
                            CarId = car2.FirstOrDefault(),
                            OrderDate = DateTime.Parse("2020-3-4"),
                            PricePaid = 439999.95M,
                            SalesPersonId = sales2.FirstOrDefault()
                        },

                        new CarPurchase
                        {
                            CustomerId = customer3.FirstOrDefault(),
                            CarId = car3.FirstOrDefault(),
                            OrderDate = DateTime.Parse("2020-3-4"),
                            PricePaid = 709999.95M,
                            SalesPersonId = sales3.FirstOrDefault()
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}