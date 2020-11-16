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
    public class SalesPersonsControllerTests : IClassFixture<TestBase>
    {
        private CarDealerContext in_memory_context;
        public SalesPersonsControllerTests(TestBase testBase)
        {
            in_memory_context = testBase.context;
        }

        [Fact(DisplayName = "SalesPerson_Index_Test")]
        public async Task SalesPerson_Index_Test()
        {
            var controller = new SalesPersonsController(in_memory_context);

            var result = await controller.Index() as ViewResult;
            var processResult = result.Model as List<SalesPerson>;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(3, processResult.Count);
        }

        [Fact(DisplayName = "SalesPerson_Create_Test")]
        public async Task SalesPerson_Create_Test()
        {
            var controller = new SalesPersonsController(in_memory_context);

            var carEntity = new SalesPerson
            {
                Name = "John Doe",
                Address = "Test Street 1",
                JobTitle = "Title",
                Salary = 10000M
            };
            await controller.Create(carEntity);
            var salesPerson = from s in in_memory_context.SalesPerson
                              where s.Name == "John Doe" && s.JobTitle == "Title"
                              select s;

            var result = await in_memory_context.SalesPerson.FindAsync(salesPerson.FirstOrDefault().SalesPersonId);

            Assert.Equal(carEntity.SalesPersonId, result.SalesPersonId);
            Assert.Equal(carEntity.Name, result.Name);
            Assert.Equal(carEntity.Address, result.Address);
            Assert.Equal(carEntity.JobTitle, result.JobTitle);
            Assert.Equal(carEntity.Salary, result.Salary);

            in_memory_context.Remove(salesPerson.FirstOrDefault());
            in_memory_context.SaveChanges();
        }

        [Fact(DisplayName = "SalesPerson_Details_Test")]
        public async Task SalesPerson_Details_Test()
        {
            var controller = new SalesPersonsController(in_memory_context);

            var salesPerson = from s in in_memory_context.SalesPerson
                              where s.Name == "Michael Takoiu" && s.JobTitle == "Sales Assistant"
                              select s.SalesPersonId;

            var result = await controller.Details(salesPerson.FirstOrDefault());

            Assert.IsType<ViewResult>(result);
        }

        [Fact(DisplayName = "SalesPerson_Details_Invalid_Test")]
        public async Task SalesPerson_Details_Invalid_Test()
        {
            var controller = new SalesPersonsController(in_memory_context);

            var result = await controller.Details(0);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
