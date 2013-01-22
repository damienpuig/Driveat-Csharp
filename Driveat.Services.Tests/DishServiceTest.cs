using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Driveat.Services.DishService;
using Moq;
using Driveat.data;

namespace Driveat.Services.Tests
{
    [TestClass]
    public class DishServiceTest
    {
        public  Mock<IDishService> dishMock { get; set; }
        public DishServiceTest()
        {
            dishMock = new Mock<IDishService>();
        }


        [TestMethod]
        public void CreateDishTest()
        {
            var dish = new Dish()
            {
                Availability = DateTime.Now,
                Description = "lalal",
                Dishtype = new DishType(){ Name = "entry" },
                Food = "lala",
                Name = "PASTY",
                Picture = "test.png",
                Price = 1.0,
                Seller = new NestedUser()
            };


            dishMock.Setup(action => action.CreateADish(dish)).Returns(dish);
        }

    }
}
