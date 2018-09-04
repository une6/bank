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
using System.Transactions;

namespace Bank.Controllers
{
    
    public class HomeController : Controller
    {
        private IUserDAL _userDAL;
        private ITransactionDAL _transactionDAL;

        private readonly object transactionLock = new object();

        public HomeController(IUserDAL userDAL, ITransactionDAL transactionDAL)
        {
            _userDAL = userDAL;
            _transactionDAL = transactionDAL;
        }

        [Authorize]
        [Route("Home/Index/{accountNumber}")]
        public IActionResult Index(long accountNumber)
        {
            using (var ts = new TransactionScope())
            {
                try
                {
                    lock(transactionLock)
                    {
                        if (HttpContext.User.Identity.Name != accountNumber.ToString())
                        {
                            ViewData["Message"] = "Unauthorized to view this record";

                            ts.Complete();
                            return View();
                        }

                        if (accountNumber == 0)
                        {
                            ts.Complete();
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

                        ts.Complete();
                        return View(transactions);
                    }
                }
                catch (Exception e)
                {
                    ViewData["Message"] = e.Message;
                    ts.Dispose();
                    return View();
                }
            }
            
        }

        [Authorize]
        public IActionResult Deposit()
        {
            lock(transactionLock)
            {
                using (var ts = new TransactionScope())
                {
                    try
                    {
                        var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                        var user = _userDAL.GetUserData(accountNumber);

                        ViewData["LoginName"] = user.LoginName;
                        ViewData["AccountNumber"] = accountNumber;
                        ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                        ts.Complete();
                        return View();

                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = e.Message;
                        ts.Dispose();
                        return View();
                    }
                }
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit([Bind]Models.Transaction trans)
        {
            lock(transactionLock)
            {
                using (var ts = new TransactionScope())
                {
                    try
                    {
                        _transactionDAL.AddTransaction(new Models.Transaction
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

                        ts.Complete();
                        return View();
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = e.Message;
                        ts.Dispose();
                        return View();
                    }
                }
            }
        }

        [Authorize]
        public IActionResult Withdraw()
        {
            lock(transactionLock)
            {
                using (var ts = new TransactionScope())
                {
                    try
                    {
                        var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                        var user = _userDAL.GetUserData(accountNumber);

                        ViewData["LoginName"] = user.LoginName;
                        ViewData["AccountNumber"] = accountNumber;
                        ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                        ts.Complete();
                        return View();
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = e.Message;
                        ts.Dispose();
                        return View();
                    }
                }
            }

        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw([Bind]Models.Transaction trans)
        {
            lock(transactionLock)
            {
                using (var ts = new TransactionScope())
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
                            _transactionDAL.AddTransaction(new Models.Transaction
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

                        ts.Complete();
                        return View();
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = e.Message;
                        ts.Dispose();
                        return View();
                    }
                }
            }
        }

        [Authorize]
        public IActionResult Transfer()
        {
            lock(transactionLock)
            {
                using (var ts = new TransactionScope())
                {
                    try
                    {
                        var accountNumber = Convert.ToInt64(HttpContext.User.Identity.Name);

                        var user = _userDAL.GetUserData(accountNumber);

                        ViewData["LoginName"] = user.LoginName;
                        ViewData["AccountNumber"] = accountNumber;
                        ViewData["Balance"] = _transactionDAL.GetBalance(accountNumber);

                        ts.Complete();
                        return View();
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = e.Message;
                        ts.Dispose();
                        return View();
                    }
                }
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Transfer([Bind]Models.Transaction trans)
        {
            lock(transactionLock)
            {
                using (var ts = new TransactionScope())
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
                            _transactionDAL.AddTransaction(new Models.Transaction
                            {
                                AccountNumber = sourceAccountNumber,
                                Amount = -trans.Amount,
                                Type = "TRANSFER",
                                Source = sourceAccountNumber,
                                Destination = trans.AccountNumber
                            });

                            _transactionDAL.AddTransaction(new Models.Transaction
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

                        ts.Complete();
                        return View();
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = e.Message;
                        ts.Dispose();
                        return View();
                    }
                }
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
