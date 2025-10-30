using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel accountView)
        {
            if(!ModelState.IsValid) return View(accountView);
            

            var user = _accountService.Login(accountView);

            if(user is null)
            {
                ModelState.AddModelError("InvalidLogin", "InValid Email or Password");
                return View(accountView);
            }

            var Result = _signInManager.PasswordSignInAsync(user, accountView.Password, accountView.RememberMe, false).Result;
            
            if(Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Not Allowed");
            if(Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Locked out");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(accountView);


        }


        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();

            return RedirectToAction(nameof(Login));
        }


        public ActionResult AccessDenied()
        {
            return View(); 
        }
    }
}
