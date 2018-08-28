using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Bank.Controllers
{
    public class UserController : Controller
    {
        private IUserDAL _userDAL;

        public UserController(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public IActionResult Index()
        {
            try
            {
                var lstUsers = _userDAL.GetAllUsers();

                if (lstUsers.Count() == 0)
                {
                    return NotFound();
                }

                return View(lstUsers);
            }
            catch(Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userDAL.LoginNameExists(user.LoginName) != true)
                    {
                        _userDAL.AddUser(user);

                        ViewData["SuccessMessage"] = "User has been created. Kindly ";
                    }
                    else
                    {
                        ViewData["Message"] = "Login Name " + user.LoginName + " already exists";

                        return View();
                    }
                }

                return View(user);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }

        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin([Bind] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string LoginStatus = _userDAL.ValidateLogin(user);

                    if (LoginStatus == "Success")
                    {
                        var accountNumber = _userDAL.GetUserData(user.LoginName).AccountNumber;

                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, accountNumber.ToString())
                    };
                        ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                        await HttpContext.SignInAsync(principal);
                        return RedirectToAction("Index/" + accountNumber, "Home");
                    }
                    else
                    {
                        ViewData["UserLoginFailed"] = "Login Failed.Please enter correct credentials";
                        return View();
                    }
                }
                else
                    return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }

        }

        
    }
}