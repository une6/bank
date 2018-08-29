using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank.Models;
using Moq;
using Bank.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Bank.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Deposit()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity(accountNumber.ToString());
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);


            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();
            
            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User {
                LoginName = name,
                Balance = balance
            });

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = controllerContext.Object;

            var result = _homeController.Deposit();

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"], accountNumber);
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }

        
    }
}
