using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NewsProcessor _newsProcessor;
        private readonly RolesProcessor _rolesProcessor;

        public LoginController(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _newsProcessor = new NewsProcessor();
            _rolesProcessor = new RolesProcessor();
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
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                ViewBag.Error = "Invalid login attempt";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction("Index", "Home");
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("Index", "Home");
                //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ListAccount()
        {
            var identityUser = _userManager.Users.ToList();
            return View(identityUser);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Disable(string Id)
        {
            var tempUser = await _userManager.FindByIdAsync(Id);

            tempUser.EmailConfirmed = false;

            await _userManager.UpdateAsync(tempUser);

            return RedirectToAction("ListAccount");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Enable(string Id)
        {
            var tempUser = await _userManager.FindByIdAsync(Id);

            tempUser.EmailConfirmed = true;

            await _userManager.UpdateAsync(tempUser);

            return RedirectToAction("ListAccount");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }

        [HttpGet] 
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ManageRole(string id)
        {
            var result = await _rolesProcessor.LoadRelativeRoles(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRole(VNPOSTWebUILibrary.Model.Roles roles)
        {
            //add news roles
            if(roles.newsAdd && _roleManager.RoleExistsAsync(nameof(roles.newsAdd)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.newsAdd));
            } else if(!roles.newsAdd && _roleManager.RoleExistsAsync(nameof(roles.newsAdd)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.newsAdd));
            }

            if(roles.newsDelete && _roleManager.RoleExistsAsync(nameof(roles.newsDelete)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.newsDelete));
            } else if (!roles.newsDelete && _roleManager.RoleExistsAsync(nameof(roles.newsDelete)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.newsDelete));
            }

            if (roles.newsEdit && _roleManager.RoleExistsAsync(nameof(roles.newsEdit)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.newsEdit));
            } else if (!roles.newsEdit && _roleManager.RoleExistsAsync(nameof(roles.newsEdit)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.newsEdit));
            }



            //add units roles
            if (roles.unitAdd && _roleManager.RoleExistsAsync(nameof(roles.unitAdd)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitAdd));
            } else if (!roles.unitAdd && _roleManager.RoleExistsAsync(nameof(roles.unitAdd)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitAdd));
            }

            if (roles.unitDelete && _roleManager.RoleExistsAsync(nameof(roles.unitDelete)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitDelete));
            } else if (!roles.unitDelete && _roleManager.RoleExistsAsync(nameof(roles.unitDelete)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitDelete));
            }

            if (roles.unitEdit && _roleManager.RoleExistsAsync(nameof(roles.unitEdit)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitEdit));
            } else if (!roles.unitEdit && _roleManager.RoleExistsAsync(nameof(roles.unitEdit)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitEdit));
            }

            if (roles.unitRead && _roleManager.RoleExistsAsync(nameof(roles.unitRead)).Result)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitRead));
            } else if (!roles.unitRead && _roleManager.RoleExistsAsync(nameof(roles.unitRead)).Result)
            {
                await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(roles.userid), nameof(roles.unitRead));
            }

            await _signInManager.RefreshSignInAsync(await _userManager.GetUserAsync(User));

            return View(roles);
        }

        public IActionResult Deny()
        {
            return View();
        }
    }
}
