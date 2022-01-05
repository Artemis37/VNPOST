using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VNPOSTWebUI.Models;
using VNPOSTWebUILibrary.BussinessLogic;
using VNPOSTWebUILibrary.Model;
using VNPOSTWebUILibrary.Model.DataSendToClient;

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

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: true, lockoutOnFailure: false);
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

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize("ManageUserRead")]
        public IActionResult ListAccount()
        {
            var identityUser = _userManager.Users.ToList();
            return View(identityUser);
        }

        [HttpGet]
        [Authorize(Policy = "ManageUserAdd")]
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
        [Authorize("ManageUserUpdate")]
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
        [Authorize("ManageUserDetail")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }

        [HttpGet] 
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
        } //deprecated

        [HttpGet]
        //[Authorize("ManageUserGroupRead")]
        public IActionResult ManageRoleGroup()
        {
            return View();
        }

        [HttpGet]
        [Authorize("ManageUserGroupAdd")]
        public IActionResult AddRoleGroup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleGroup(RolesForList roles)
        {
            if (!await _roleManager.RoleExistsAsync(roles.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(roles.Name));
                Response.Cookies.Append("resultAddRoleGroup", "True");
            }else
            {
                Response.Cookies.Append("resultAddRoleGroup", "False");
            }
            return View();
        }

        [HttpGet]
        [Authorize("ManageUserGroupDetail")]
        public async Task<IActionResult> ViewRoleGroup(string id)
        {
            ViewBag.Action = "View";
            var temp = await _roleManager.FindByIdAsync(id);
            return View("AddRoleGroup", new RolesForList() { Id=id, Name=temp.Name });
        }

        [HttpGet]
        [Authorize("ManageUserGroupUpdate")]
        public async Task<IActionResult> UpdateRoleGroup(string id)
        {
            ViewBag.Action = "/Login/UpdateRoleGroup";
            var temp = await _roleManager.FindByIdAsync(id);
            return View("AddRoleGroup", new RolesForList() { Id = id, Name = temp.Name });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoleGroup(VNPOSTWebUILibrary.Model.DataSendToClient.RolesForList roles)
        {
            var tempRole = await _roleManager.FindByIdAsync(roles.Id);
            tempRole.Name = roles.Name;
            var result = await _roleManager.UpdateAsync(tempRole);
            if (result.Succeeded)
            {
                Response.Cookies.Append("updateRoleGroupResult","True");
            }else
            {
                Response.Cookies.Append("updateRoleGroupResult","False");
            }
            return View("AddRoleGroup", new RolesForList() { Id = tempRole.Id, Name = tempRole.Name });
        }

        [HttpPost]
        [Authorize("ManageUserGroupDelete")]
        public async Task<bool> DeleteRoleGroup([FromHeader]string RoleId)
        {
            var result = await _roleManager.DeleteAsync(await _roleManager.FindByIdAsync(RoleId));
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> LoadClaims([FromHeader]string RoleId)
        {
            var result = await _rolesProcessor.loadClaim(RoleId);
            return Json(result);
        }

        [HttpGet]
        public IActionResult AppointRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AppointRole([FromBody] AllClaim allclaims, [FromHeader] string RoleId)
        {
            await _rolesProcessor.removeRoleClaimById(RoleId);
            //Application
            if (allclaims.ManageApplicationRead)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageApplication.Read"));
            }
            if (allclaims.ManageApplicationUpdate)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageApplication.Update"));
            }
            if (allclaims.ManageApplicationUserAdd)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageApplication.User.Add"));
            }
            if (allclaims.ManageApplicationUserUpdate)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageApplication.User.Update"));
            }

            //User Group
            if (allclaims.ManageUserGroupRead)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUserGroup.Read"));
            }
            if (allclaims.ManageUserGroupAdd)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUserGroup.Add"));
            }
            if (allclaims.ManageUserGroupUpdate)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUserGroup.Update"));
            }
            if (allclaims.ManageUserGroupDelete)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUserGroup.Delete"));
            }
            if (allclaims.ManageUserGroupRolesAdd)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUserGroup.Roles.Add"));
            }
            if (allclaims.ManageUserGroupDetail)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUserGroup.Detail"));
            }

            //User
            if (allclaims.ManageUserRead)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUser.Read"));
            }
            if (allclaims.ManageUserDetail)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUser.Detail"));
            }
            if (allclaims.ManageUserAdd)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUser.Add"));
            }
            if (allclaims.ManageUserUpdate)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUser.Update"));
            }
            if (allclaims.ManageUserUpdateUserGroup)
            {
                await _roleManager.AddClaimAsync(await _roleManager.FindByIdAsync(RoleId), new Claim(ClaimTypes.Role, "ManageUser.UpdateUserGroup"));
            }
            Response.Cookies.Append("changeRoleGroupResult","True");
            await _signInManager.RefreshSignInAsync(await _userManager.GetUserAsync(User));
            return View();
        }

        [HttpGet]
        public IActionResult Deny()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> loadRoles()
        {
            return Json(await _rolesProcessor.loadRoles());
        }

        [HttpPost]
        public async Task<IActionResult> loadAppointedRoles([FromHeader]string id)
        {
            return Json(await _rolesProcessor.loadAppoinedRolesById(id));
        }

        [HttpPost]
        [Authorize("ManageUserUpdateUserGroup")]
        public async Task<bool> updateRoleForUser([FromBody]AllRolesOfUser allRoles)
        {
            var targetedUser = await _userManager.FindByIdAsync(allRoles.Id);
            //Add additional roles
            foreach (var item in allRoles.AssignedRoles)
            {
                if(!await _userManager.IsInRoleAsync(targetedUser, item.Name))
                {
                    await _userManager.AddToRoleAsync(targetedUser, item.Name);
                }
            }

            //Remove redundant roles
            foreach (var item in allRoles.AvailableRoles)
            {
                if(await _userManager.IsInRoleAsync(targetedUser, item.Name))
                {
                    await _userManager.RemoveFromRoleAsync(targetedUser, item.Name);
                }
            }

            await _signInManager.RefreshSignInAsync(await _userManager.GetUserAsync(User));

            return true;
        }

        [AllowAnonymous]
        public async Task<IActionResult> test()
        {
            //await _roleManager.CreateAsync(new IdentityRole("ManageApplication"));
            //await _roleManager.CreateAsync(new IdentityRole("ManageUserGroup"));
            //await _roleManager.CreateAsync(new IdentityRole("ManageUser"));

            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageApplication"), new Claim(ClaimTypes.Role, "ManageApplication.Read"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageApplication"), new Claim(ClaimTypes.Role, "ManageApplication.Update"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageApplication"), new Claim(ClaimTypes.Role, "ManageApplication.User.Add"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageApplication"), new Claim(ClaimTypes.Role,"ManageApplication.User.Update"));

            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUserGroup"), new Claim(ClaimTypes.Role, "ManageUserGroup.Read"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUserGroup"), new Claim(ClaimTypes.Role, "ManageUserGroup.Detail"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUserGroup"), new Claim(ClaimTypes.Role, "ManageUserGroup.Add"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUserGroup"), new Claim(ClaimTypes.Role, "ManageUserGroup.Update"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUserGroup"), new Claim(ClaimTypes.Role, "ManageUserGroup.Delete"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUserGroup"), new Claim(ClaimTypes.Role, "ManageUserGroup.Roles.Add"));

            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUser"), new Claim(ClaimTypes.Role, "ManageUser.Read"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUser"), new Claim(ClaimTypes.Role, "ManageUser.Detail"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUser"), new Claim(ClaimTypes.Role, "ManageUser.Add"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUser"), new Claim(ClaimTypes.Role, "ManageUser.Update"));
            //await _roleManager.AddClaimAsync(await _roleManager.FindByNameAsync("ManageUser"), new Claim(ClaimTypes.Role, "ManageUser.UpdateUserGroup"));

            //await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync("2f8a6c97-bfbb-4af4-bffc-6e30ba697f9c"), "ManageApplication");

            //await _userManager.AddToRoleAsync(await _userManager.GetUserAsync(User), "Cirila");

            return View();
        }

    }
}
