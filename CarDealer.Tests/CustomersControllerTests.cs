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
    public class CustomersControllerTests : IClassFixture<TestBase>
    {
        private CarDealerContext in_memory_context;
        public CustomersControllerTests(TestBase testBase)
        {
            in_memory_context = testBase.context;
        }

        [Fact(DisplayName = "Customer_Index_Test")]
        public async Task Customer_Index_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var result = await controller.Index("", "") as ViewResult;
            var processResult = result.Model as List<Customer>;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(3, processResult.Count);
        }

        [Fact(DisplayName = "Customer_Index_Search_Name_Validate_Surname_Test")]
        public async Task Customer_Index_Search_Name_Validate_Surname_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var result = await controller.Index("Christopher", "") as ViewResult;
            var processResult = result.Model as List<Customer>;

            Assert.IsType<ViewResult>(result);
            Assert.Single(processResult);
            Assert.Equal("Halloway", processResult.First().Surname);
        }

        [Fact(DisplayName = "Customer_Index_Search_Address_Validate_Names_Test")]
        public async Task Customer_Index_Search_Address_Validate_Names_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var result = await controller.Index("", "Broadway Street 77") as ViewResult;
            var processResult = result.Model as List<Customer>;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(2, processResult.Count);
            Assert.Equal("Christopher", processResult.ElementAt(0).Name);
            Assert.Equal("Jane", processResult.ElementAt(1).Name);
        }

        [Fact(DisplayName = "Customer_Index_Search_All_Filters_Validate_Surname_Test")]
        public async Task Customer_Index_Search_All_Filters_Validate_Surname_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var result = await controller.Index("Christopher", "Broadway Street 77") as ViewResult;
            var processResult = result.Model as List<Customer>;

            Assert.IsType<ViewResult>(result);
            Assert.Single(processResult);
            Assert.Equal("Halloway", processResult.First().Surname);
        }

        [Fact(DisplayName = "Customer_Create_Test")]
        public async Task Customer_Create_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var customerEntity = new Customer
            {
                Name = "John",
                Surname = "Doe",
                Address = "Test Street 1",
                Age = 30,
                Created = true
            };
            await controller.Create(customerEntity);
            var customer = from c in in_memory_context.Customer
                           where c.Name == "John" && c.Surname == "Doe"
                           select c;

            var result = await in_memory_context.Customer.FindAsync(customer.FirstOrDefault().CustomerId);

            Assert.Equal(customerEntity.CustomerId, result.CustomerId);
            Assert.Equal(customerEntity.Name, result.Name);
            Assert.Equal(customerEntity.Surname, result.Surname);
            Assert.Equal(customerEntity.Address, result.Address);
            Assert.Equal(customerEntity.Age, result.Age);
            Assert.Equal(customerEntity.Created, result.Created);

            in_memory_context.Remove(customer.FirstOrDefault());
            in_memory_context.SaveChanges();
        }

        [Fact(DisplayName = "Customer_Details_Test")]
        public async Task Customer_Details_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var customer = from c in in_memory_context.Customer
                           where c.Name == "Christopher" && c.Surname == "Halloway"
                           select c.CustomerId;

            var result = await controller.Details(customer.FirstOrDefault());

            Assert.IsType<ViewResult>(result);
        }

        [Fact(DisplayName = "Customer_Details_Invalid_Test")]
        public async Task Customer_Details_Invalid_Test()
        {
            var controller = new CustomersController(in_memory_context);

            var result = await controller.Details(0);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
