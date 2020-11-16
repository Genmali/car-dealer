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
    public class CarsControllerTests : IClassFixture<TestBase>
    {
        private CarDealerContext in_memory_context;
        public CarsControllerTests(TestBase testBase)
        {
            in_memory_context = testBase.context;
        }

        [Fact(DisplayName = "Car_Index_Test")]
        public async Task Car_Index_Test()
        {
            var controller = new CarsController(in_memory_context);

            var result = await controller.Index() as ViewResult;
            var processResult = result.Model as List<Car>;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(4, processResult.Count);
        }

        [Fact(DisplayName = "Car_Create_Test")]
        public async Task Car_Create_Test()
        {
            var controller = new CarsController(in_memory_context);

            var carEntity = new Car
            {
                Make = "Audi",
                Model = "A6",
                Color = "Black",
                Extras = "Aircon, Electric Steering, Electric Seats",
                RecommendedPrice = 749999M
            };
            await controller.Create(carEntity);
            var car = from c in in_memory_context.Car
                      where c.Make == "Audi" && c.Model == "A6"
                      select c;

            var result = await in_memory_context.Car.FindAsync(car.FirstOrDefault().CarId);

            Assert.Equal(carEntity.CarId, result.CarId);
            Assert.Equal(carEntity.Make, result.Make);
            Assert.Equal(carEntity.Model, result.Model);
            Assert.Equal(carEntity.Color, result.Color);
            Assert.Equal(carEntity.Extras, result.Extras);
            Assert.Equal(carEntity.RecommendedPrice, result.RecommendedPrice);

            in_memory_context.Remove(car.FirstOrDefault());
            in_memory_context.SaveChanges();
        }

        [Fact(DisplayName = "Car_Details_Test")]
        public async Task Car_Details_Test()
        {
            var controller = new CarsController(in_memory_context);

            var car = from c in in_memory_context.Car
                      where c.Make == "Ford" && c.Model == "Mustang GTR"
                      select c.CarId;

            var result = await controller.Details(car.FirstOrDefault());

            Assert.IsType<ViewResult>(result);
        }

        [Fact(DisplayName = "Car_Details_Invalid_Test")]
        public async Task Car_Details_Invalid_Test()
        {
            var controller = new CarsController(in_memory_context);

            var result = await controller.Details(0);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
