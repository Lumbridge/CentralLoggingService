using CLS.Infrastructure.Classes;
using CLS.Infrastructure.Data;
using CLS.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using CLS.Core.Data;
using CLS.Infrastructure.Helpers;

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
