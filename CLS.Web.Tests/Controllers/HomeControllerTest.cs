﻿using CLS.Infrastructure.Classes;
using CLS.Web.Controllers;
using CLS.Web.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace CLS.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(new UnitOfWork(new DBEntities()));

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
