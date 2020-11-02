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

        [TestMethod]
        public void ShouldVerifyLogin()
        {
            // Arrange
            var hashedPassword = EncryptionHelper.ComputeHash("password123");
            
            // Act
            var result = EncryptionHelper.IsValidPassword("password123", hashedPassword);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldDenyLogin()
        {
            // Arrange
            var hashedPassword = EncryptionHelper.ComputeHash("password123");

            // Act
            var result = EncryptionHelper.IsValidPassword("Password123", hashedPassword);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HashedPasswordsShouldBeDifferent()
        {
            // Arrange
            var hashedPassword1 = EncryptionHelper.ComputeHash("password123");
            var hashedPassword2 = EncryptionHelper.ComputeHash("password123");

            // Act
            var result = string.Equals(hashedPassword1, hashedPassword2);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
