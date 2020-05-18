using System;
using System.Collections.Generic;
using TestProject.Controllers;
using TestProject.Models;
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
            var mock = new Mock<DiaryDBContext>();
            mock.Setup(db => db.Classes.ToList()).Returns(GetTestClasses());
            var controller = new HomeController(mock.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Classes>>(viewResult.Model);
            Assert.Equal(GetTestClasses().Count, model.Count());
        }
        private List<Classes> GetTestClasses()
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
