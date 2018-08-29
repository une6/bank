using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank.Models;
using Moq;
using Bank.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using System.Security.Claims;

namespace Bank.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void DepositNoValues()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;

            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, accountNumber.ToString())
            }));
            
            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();
            
            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User {
                LoginName = name
            });

            mockTransactionDAL.Setup(x => x.GetBalance(It.IsAny<long>())).Returns(balance);

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _homeController.Deposit();

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"].ToString(), accountNumber.ToString());
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }

        [TestMethod]
        public void WithdrawNoValues()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;


            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, accountNumber.ToString())
            }));

            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();

            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User
            {
                LoginName = name
            });

            mockTransactionDAL.Setup(x => x.GetBalance(It.IsAny<long>())).Returns(balance);

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _homeController.Withdraw();

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"].ToString(), accountNumber.ToString());
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }

        [TestMethod]
        public void TransferNoValues()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;


            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, accountNumber.ToString())
            }));

            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();

            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User
            {
                LoginName = name
            });

            mockTransactionDAL.Setup(x => x.GetBalance(It.IsAny<long>())).Returns(balance);

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _homeController.Transfer();

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"].ToString(), accountNumber.ToString());
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }

        [TestMethod]
        public void DepositWithValues()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;


            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, accountNumber.ToString())
            }));

            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();

            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User
            {
                LoginName = name
            });

            mockTransactionDAL.Setup(x => x.GetBalance(It.IsAny<long>())).Returns(balance);

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _homeController.Deposit(new Transaction
            {
                Amount = 1.1m,
                AccountNumber = accountNumber
            });

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"].ToString(), accountNumber.ToString());
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }

        [TestMethod]
        public void WithdrawWithValues()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;


            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, accountNumber.ToString())
            }));

            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();

            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User
            {
                LoginName = name
            });

            mockTransactionDAL.Setup(x => x.GetBalance(It.IsAny<long>())).Returns(balance);

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _homeController.Withdraw(new Transaction
            {
                Amount = 1.1m,
                AccountNumber = accountNumber
            });

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"].ToString(), accountNumber.ToString());
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }

        [TestMethod]
        public void TransferWithValues()
        {
            var name = "testuser1";
            var balance = 1.1m;
            var accountNumber = 1;


            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, accountNumber.ToString())
            }));

            var mockUserDAL = new Mock<IUserDAL>();
            var mockTransactionDAL = new Mock<ITransactionDAL>();

            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<long>())).Returns(new User
            {
                LoginName = name
            });

            mockTransactionDAL.Setup(x => x.GetBalance(It.IsAny<long>())).Returns(balance);

            var _homeController = new HomeController(mockUserDAL.Object, mockTransactionDAL.Object);
            _homeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _homeController.Transfer(new Transaction
            {
                Amount = 1.1m,
                AccountNumber = accountNumber
            });

            var r = result as ViewResult;

            Assert.AreEqual(r.ViewData["LoginName"], name);
            Assert.AreEqual(r.ViewData["AccountNumber"].ToString(), accountNumber.ToString());
            Assert.AreEqual(r.ViewData["Balance"], balance);
        }
    }
}
