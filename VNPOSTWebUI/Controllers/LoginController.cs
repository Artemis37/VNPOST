using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPOSTWebUI.Models;
using VNPOSTWebUILibrary.BussinessLogic;

namespace VNPOSTWebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly NewsProcessor _newsProcessor;

        public LoginController(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _newsProcessor = new NewsProcessor();
        }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if(string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                ViewBag.Error = "Invalid login attempt";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction("Index","Home");
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("Index", "Home");
                //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.Error = "Invalid login attempt";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index","Home");
        }

        public IActionResult ListAccount()
        {
            var identityUser = _userManager.Users.ToList();
            return View(identityUser);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel account)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = account.Email, Email = account.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, account.Password); //add to database
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToAction("Index", "Manage");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityUser receivedUser)
        {
            var user = await _userManager.FindByIdAsync(receivedUser.Id);
            var result = await _userManager.SetUserNameAsync(user,receivedUser.UserName);
            if (!result.Succeeded)
            {
                ViewBag.Result = result;
                return View();
            }
            result = await _userManager.SetEmailAsync(user, receivedUser.Email);
            if (!result.Succeeded)
            {
                ViewBag.Result = result;
                return View();
            }
            result = await _userManager.SetPhoneNumberAsync(user, receivedUser.PhoneNumber);
            if (!result.Succeeded)
            {
                ViewBag.Result = result;
                return View();
            }
            ViewBag.Result = "Saved";
            user = await _userManager.FindByIdAsync(receivedUser.Id);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Disable(string Id)
        {
            var tempUser = await _userManager.FindByIdAsync(Id);

            tempUser.EmailConfirmed = false;

            await _userManager.UpdateAsync(tempUser);

            return RedirectToAction("ListAccount");
        }

        [HttpGet]
        public async Task<IActionResult> Enable(string Id)
        {
            var tempUser = await _userManager.FindByIdAsync(Id);

            tempUser.EmailConfirmed = true;

            await _userManager.UpdateAsync(tempUser);

            return RedirectToAction("ListAccount");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }
    }
}
