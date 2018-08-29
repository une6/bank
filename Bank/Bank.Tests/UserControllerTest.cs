using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank.Models;
using Moq;
using Bank.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bank.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void CreateUserNotExist()
        {
            var mockUserDAL = new Mock<IUserDAL>();

            var name = "testuser1";

            mockUserDAL.Setup(x => x.LoginNameExists(It.IsAny<string>())).Returns(false);
            
            var _userController = new UserController(mockUserDAL.Object);

            var result = _userController.Create(new User
            {
                LoginName = name
            });

            var r = result as ViewResult;
            Assert.AreEqual(r.ViewData["SuccessMessage"], "User has been created. Kindly ");
        }

        [TestMethod]
        public void CreateExistingUser()
        {
            var mockUserDAL = new Mock<IUserDAL>();

            var name = "testuser1";

            mockUserDAL.Setup(x => x.LoginNameExists(It.IsAny<string>())).Returns(true);

            var _userController = new UserController(mockUserDAL.Object);

            var result = _userController.Create(new User
            {
                LoginName = name
            });

            var r = result as ViewResult;
            Assert.AreEqual(r.ViewData["Message"], "Login Name " + name + " already exists");
        }

        /*
        [TestMethod]
        public void LoginSuccess()
        {
            var mockUserDAL = new Mock<IUserDAL>();

            var name = "testuser1";
            var accountNumber = 1;

            mockUserDAL.Setup(x => x.ValidateLogin(It.IsAny<User>())).Returns("Success");

            var _userController = new UserController(mockUserDAL.Object);

            var result = _userController.UserLogin(new User
            {
                LoginName = name
            }).Result;

            var r = result as ViewResult;
            Assert.AreEqual(r.ViewData["UserLoginFailed"], String.Empty);
        }
        */

        [TestMethod]
        public void LoginFailed()
        {
            var mockUserDAL = new Mock<IUserDAL>();

            var name = "testuser1";

            mockUserDAL.Setup(x => x.ValidateLogin(It.IsAny<User>())).Returns("Failed");

            var _userController = new UserController(mockUserDAL.Object);

            var result = _userController.UserLogin(new User
            {
                LoginName = name
            }).Result;

            var r = result as ViewResult;
            Assert.AreEqual(r.ViewData["UserLoginFailed"], "Login Failed.Please enter correct credentials");
        }
    }
}
