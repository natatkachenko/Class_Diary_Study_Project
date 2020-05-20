using System;
using System.Collections.Generic;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Services;
using Xunit;
using Moq;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Tests
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfClasses()
        {
            //Arrange
            var service = new Mock<IHomeService>();
            var classes = GetTestClasses();
            service.Setup(s => s.AllClasses()).Returns(classes);
            var controller = new HomeController();

            //Act
            var result = controller.GetClasses();
            var count = result.Count();

            //Assert
            Assert.Equal(count, classes.Count());
        }
        private IEnumerable<Classes> GetTestClasses()
        {
            var classes = new List<Classes>
            {
                new Classes{ Name="1-A"},
                new Classes{ Name="2-A"},
                new Classes{ Name="3-A"}
            };
            return classes;
        }
    }
}
