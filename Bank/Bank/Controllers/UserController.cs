using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank.Models;
using Microsoft.Extensions.Configuration;

namespace Bank.Controllers
{
    public class UserController : Controller
    {
        UserDAL userDAL = null;

        public UserController(IConfiguration configuration)
        {
            userDAL = new UserDAL(configuration);
        }

        public IActionResult Index()
        {
            var lstUsers = userDAL.GetAllUsers();
            
            if(lstUsers.Count() == 0)
            {
                return NotFound();
            }

            return View(lstUsers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind]User user)
        {
            if (ModelState.IsValid)
            {
                if(userDAL.LoginNameExists(user.LoginName) != true)
                {
                    userDAL.AddUser(user);
                }
                else
                {
                    ViewData["Message"] = "Login Name " + user.LoginName + " already exists";

                    return View();
                }

                return RedirectToAction("Details/"+user.LoginName);
            }

            return View(user);
        }

        [HttpGet]
        [Route("User/Details/{loginName}")]
        public IActionResult Details(string loginName)
        {
            if (loginName == null)
            {
                return NotFound();
            }
            var user = userDAL.GetUserData(loginName);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}