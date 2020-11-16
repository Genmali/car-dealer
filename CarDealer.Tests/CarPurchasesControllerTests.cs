using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CarDealer.Data;
using CarDealer.Models;
using CarDealer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarDealer.Tests
{
    public class CarPurchasesControllerTests : IClassFixture<TestBase>
    {
        private CarDealerContext in_memory_context;

        public CarPurchasesControllerTests(TestBase testBase)
        {
            in_memory_context = testBase.context;
        }

        [Fact(DisplayName = "CarPurchase_Index_Test")]
        public async Task CarPurchase_Index_Test()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var result = await controller.Index("", "", "") as ViewResult;
            var processResult = result.Model as List<CarPurchase>;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(3, processResult.Count);
        }

        [Fact(DisplayName = "CarPurchase_Index_Search_CarMake_Validate_CustomeName_Test")]
        public async Task CarPurchase_Index_Test_Search_CarMake_Validate_CustomeName()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var result = await controller.Index("Ford", "", "") as ViewResult;
            var processResult = result.Model as List<CarPurchase>;

            Assert.IsType<ViewResult>(result);
            Assert.Single(processResult);
            Assert.Equal("Christopher", processResult.First().Customer.Name);
        }

        [Fact(DisplayName = "CarPurchase_Index_Search_CarModel_Validate_CustomeNames_Test")]
        public async Task CarPurchase_Index_Test_Search_CarModel_Validate_CustomeNames()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var result = await controller.Index("", "GTR", "") as ViewResult;
            var processResult = result.Model as List<CarPurchase>;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(2, processResult.Count);
            Assert.Equal("Jane", processResult.ElementAt(0).Customer.Name);
            Assert.Equal("Christopher", processResult.ElementAt(1).Customer.Name);
        }

        [Fact(DisplayName = "CarPurchase_Index_Search_SalesPerson_Validate_CustomeName_Test")]
        public async Task CarPurchase_Index_Test_Search_SalesPerson_Validate_CustomeName()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var result = await controller.Index("", "", "Brian Tenning") as ViewResult;
            var processResult = result.Model as List<CarPurchase>;

            Assert.IsType<ViewResult>(result);
            Assert.Single(processResult);
            Assert.Equal("Jane", processResult.ElementAt(0).Customer.Name);
        }

        [Fact(DisplayName = "CarPurchase_Index_Search_All_Filters_Validate_CustomerName_Test")]
        public async Task CarPurchase_Index_Search_All_Filters_Validate_CustomerName_Test()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var result = await controller.Index("Ford", "GTR", "Michael Takoiu") as ViewResult;
            var processResult = result.Model as List<CarPurchase>;

            Assert.IsType<ViewResult>(result);
            Assert.Single(processResult);
            Assert.Equal("Christopher", processResult.ElementAt(0).Customer.Name);
        }

        [Fact(DisplayName = "CarPurchase_Create_Test")]
        public async Task CarPurchase_Create_Test()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var customer1 = from c in in_memory_context.Customer
                            where c.Name == "Christopher"
                            select c.CustomerId;
            var car1 = from c in in_memory_context.Car
                       where c.Make == "Ford"
                       select c.CarId;
            var sales1 = from s in in_memory_context.SalesPerson
                         where s.Name == "Michael Takoiu"
                         select s.SalesPersonId;

            var purchase = new CarPurchase
            {
                CustomerId = customer1.FirstOrDefault(),
                CarId = car1.FirstOrDefault(),
                OrderDate = DateTime.Parse("2020-1-1"),
                PricePaid = 10000M,
                SalesPersonId = sales1.FirstOrDefault()
            };
            await controller.Create(purchase);
            var sale = from s in in_memory_context.CarPurchase
                       where s.PricePaid == 10000M
                       select s;

            var result = await in_memory_context.CarPurchase.FindAsync(sale.FirstOrDefault().CarPurchaseId);

            Assert.Equal(purchase.CarPurchaseId, result.CarPurchaseId);
            Assert.Equal(purchase.CustomerId, result.CustomerId);
            Assert.Equal(purchase.CarId, result.CarId);
            Assert.Equal(purchase.OrderDate, result.OrderDate);
            Assert.Equal(purchase.PricePaid, result.PricePaid);
            Assert.Equal(purchase.SalesPersonId, result.SalesPersonId);

            in_memory_context.Remove(sale.FirstOrDefault());
            in_memory_context.SaveChanges();
        }

        [Fact(DisplayName = "CarPurchase_Details_Test")]
        public async Task CarPurchase_Details_Test()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var purchase = from p in in_memory_context.CarPurchase
                           where p.Customer.Name == "Christopher" && p.Car.Make == "Ford" && p.SalesPerson.Name == "Michael Takoiu"
                           select p.CarPurchaseId;

            var result = await controller.Details(purchase.FirstOrDefault());

            Assert.IsType<ViewResult>(result);
        }

        [Fact(DisplayName = "CarPurchase_Details_Invalid_Test")]
        public async Task CarPurchase_Details_Invalid_Test()
        {
            var controller = new CarPurchasesController(in_memory_context);

            var result = await controller.Details(0);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
