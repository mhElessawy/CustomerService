using CustomerServicesSystem.Models;
using CustomerServicesSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServicesSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly UserManager<ApplicationUser>  _userMgr;

        public AccountController(SignInManager<ApplicationUser> signIn,
                                 UserManager<ApplicationUser>  userMgr)
        {
            _signIn  = signIn;
            _userMgr = userMgr;
        }

        // GET /Account/Login
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST /Account/Login
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _signIn.PasswordSignInAsync(vm.Email, vm.Password,
                                                           vm.RememberMe, false);
            if (result.Succeeded)
            {
                var user = await _userMgr.FindByEmailAsync(vm.Email);
                if (user != null && !user.IsActive)
                {
                    await _signIn.SignOutAsync();
                    ModelState.AddModelError("", "Your account is disabled.");
                    return View(vm);
                }
                return LocalRedirect(returnUrl ?? "/");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(vm);
        }

        // POST /Account/Logout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
