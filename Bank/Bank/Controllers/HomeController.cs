using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace Bank.Controllers
{
    
    public class HomeController : Controller
    {
        private IUserDAL _userDAL;
        private ITransactionDAL _transactionDAL;

        public HomeController(IUserDAL userDAL, ITransactionDAL transactionDAL)
        {
            _userDAL = userDAL;
            _transactionDAL = transactionDAL;
        }

        [Authorize]
        [Route("Home/Index/{accountNumber}")]
        public IActionResult Index(long accountNumber)
        {
            try
            {

                if (HttpContext.User.Identity.Name != accountNumber.ToString())
                {
                    ViewData["Message"] = "Unauthorized to view this record";

                    return View();
                }

                if (accountNumber == 0)
                {
                    RedirectToAction("UserLogin", "User");
                }

                var user = _userDAL.GetUserData(accountNumber);
                var transactions = _transactionDAL.GetAllTransactions(accountNumber);
                if (transactions.Count() == 0)
                {
                    ViewData["Message"] = "No Records found";
                }

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = accountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                return View(transactions);

            }
            catch(Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }
            
        }

        [Authorize]
        public IActionResult Deposit()
        {
            try
            {
                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                var user = _userDAL.GetUserData(accountNumber);

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = accountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                return View();

            }
            catch(Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }

        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit([Bind]Transaction trans)
        {
            try
            {
                _transactionDAL.AddTransaction(new Transaction
                {
                    AccountNumber = trans.AccountNumber,
                    Amount = trans.Amount,
                    Type = "DEPOSIT"
                });

                ViewData["SuccessMessage"] = "Deposit success";

                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                var user = _userDAL.GetUserData(accountNumber);

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = accountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                return View();
            }
            catch(Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }

        }

        [Authorize]
        public IActionResult Withdraw()
        {
            try
            {
                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                var user = _userDAL.GetUserData(accountNumber);

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = accountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }

        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw([Bind]Transaction trans)
        {
            try
            {
                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                var user = _userDAL.GetUserData(accountNumber);

                if (_transactionDAL.GetBalance(accountNumber) < trans.Amount)
                {
                    ViewData["Message"] = "Not enough funds.";
                }
                else
                {
                    _transactionDAL.AddTransaction(new Transaction
                    {
                        AccountNumber = accountNumber,
                        Amount = -trans.Amount,
                        Type = "WITHDRAW"
                    });

                    ViewData["SuccessMessage"] = "Withdraw success";
                }

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = accountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }
        }

        [Authorize]
        public IActionResult Transfer()
        {
            try
            {
                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                var user = _userDAL.GetUserData(accountNumber);

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = accountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Transfer([Bind]Transaction trans)
        {
            try
            {
                var sourceAccountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                var user = _userDAL.GetUserData(sourceAccountNumber);

                if (_transactionDAL.GetBalance(sourceAccountNumber) < trans.Amount)
                {
                    ViewData["Message"] = "Not enough funds.";
                }
                else
                {
                    _transactionDAL.AddTransaction(new Transaction
                    {
                        AccountNumber = sourceAccountNumber,
                        Amount = -trans.Amount,
                        Type = "TRANSFER",
                        Source = sourceAccountNumber,
                        Destination = trans.AccountNumber
                    });

                    _transactionDAL.AddTransaction(new Transaction
                    {
                        AccountNumber = trans.AccountNumber,
                        Amount = trans.Amount,
                        Type = "TRANSFER",
                        Source = sourceAccountNumber,
                        Destination = trans.AccountNumber
                    });

                    ViewData["SuccessMessage"] = "Transfer success";
                }

                ViewData["LoginName"] = user.LoginName;
                ViewData["AccountNumber"] = sourceAccountNumber;
                ViewData["Balance"] = _transactionDAL.GetBalance(sourceAccountNumber);

                return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }
        }

        public IActionResult About()
        {
            try
            {
                ViewData["Message"] = "Your application description page.";

                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);
                ViewData["AccountNumber"] = accountNumber;

                return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }
        }

        public IActionResult Contact()
        {
            try
            {
                ViewData["Message"] = "Your contact page.";

                var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);
                ViewData["AccountNumber"] = accountNumber;

                return View();
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("UserLogin", "User");
        }
    }
}
