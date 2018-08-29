using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank.Models;
using Moq;
using Bank.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
        public async Task LoginSuccess()
        {
            var mockUserDAL = new Mock<IUserDAL>();

            var name = "testuser1";
            var accountNumber = 1;

            mockUserDAL.Setup(x => x.ValidateLogin(It.IsAny<User>())).Returns("Success");
            mockUserDAL.Setup(x => x.GetUserData(It.IsAny<string>())).Returns(new User
            {
                AccountNumber = accountNumber
            });

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            var _userController = new UserController(mockUserDAL.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = serviceProviderMock.Object
                    }
                }
            };

            //var _userController = new UserController(mockUserDAL.Object);

            var result = await _userController.UserLogin(new User
            {
                LoginName = name
            });

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
