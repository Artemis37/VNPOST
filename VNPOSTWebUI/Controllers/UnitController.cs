using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VNPOSTWebUILibrary.BussinessLogic;
using VNPOSTWebUILibrary.Model;

namespace VNPOSTWebUI.Controllers
{
    [Authorize]
    public class UnitController : Controller
    {
        private readonly UnitsProcessor _unitsProcessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UnitController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _unitsProcessor = new UnitsProcessor();
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UnitRead")]
        public IActionResult ListCategoryUnit()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UnitRead")]
        public IActionResult ListUnit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCategoryUnitList()
        {
            var result = await _unitsProcessor.getAllCategoryUnit();
            return Json(result);
        }

        [HttpGet]
        [Authorize(Roles = "UnitAdd")]
        public IActionResult CategoryUnitAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UnitAdd")]
        public async Task<IActionResult> CategoryUnitAdd([FromForm] string id, string name, string status)
        {
            if (ModelState.IsValid)
            {
                if (!await _unitsProcessor.isCategoryUnitIdExisted(id))
                {
                    bool result = await _unitsProcessor.addCategoryUnit(new CategoryUnit()
                    {
                        id = id,
                        name = name,
                        status = status == "1" ? true : false,
                        userid = _userManager.GetUserId(User)
                    });
                    if (result)
                    {
                        Response.Cookies.Append("addResult","True");
                    }else
                    {
                        Response.Cookies.Append("addResult","False");
                    }
                }else
                {
                    Response.Cookies.Append("addResult","False");
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UnitAdd")]
        public IActionResult UnitAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UnitAdd")]
        public async Task<IActionResult> UnitAdd([FromForm]Unit unit, string statuss)
        {
            if (ModelState.IsValid)
            {
                unit.Status = statuss == "1" ? true : false;
                unit.UserId = _userManager.GetUserId(User);
                bool result = await _unitsProcessor.addUnit(unit);
                if (result)
                {
                    Response.Cookies.Append("unitAddResult","True");
                }else
                {
                    Response.Cookies.Append("unitAddResult","False");
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UnitRead")]
        public async Task<IActionResult> CategoryDetail(string id)
        {
            var temp = await _unitsProcessor.getCategoryUnitWithId(id);
            if(temp != null)
            {
                CategoryUnit categoryUnit = new CategoryUnit() { 
                    id = temp.id,
                    name = temp.name,
                    status = temp.status,
                    userid = temp.userid
                };
                ViewBag.Action = "View";
                return View("CategoryUnitAdd", categoryUnit);
            }
            return View("ListCategoryUnit");
        }

        [HttpGet]
        [Authorize(Roles = "UnitEdit")]
        public async Task<IActionResult> CategoryEdit(string id)
        {
            var temp = await _unitsProcessor.getCategoryUnitWithId(id);
            if (temp != null)
            {
                CategoryUnit categoryUnit = new CategoryUnit()
                {
                    id = temp.id,
                    name = temp.name,
                    status = temp.status,
                    userid = temp.userid
                };
                ViewBag.ToAction = "/Unit/CategoryEdit";
                return View("CategoryUnitAdd", categoryUnit);
            }
            return View("ListCategoryUnit");
        }

        [HttpPost]
        [Authorize(Roles = "UnitEdit")]
        public async Task<IActionResult> CategoryEdit([FromForm] string id, string name, string status)
        {
            var categoryUnit = new CategoryUnit() {
                id = id,
                name = name,
                status = status == "1" ? true : false
            };
            bool result = await _unitsProcessor.updateCategoryUnit(categoryUnit);
            Response.Cookies.Append("updateResult", result.ToString());
            return RedirectToAction("CategoryEdit", "Unit", new { id = categoryUnit.id });
        }

        [HttpPost]
        [Authorize(Roles = "UnitDelete")]
        public async Task<IActionResult> CategoryUnitDelete([FromHeader(Name = "id")] string id)
        {
            bool result = await _unitsProcessor.deleteCategoryUnit(id);
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "UnitDelete")]
        public async Task<IActionResult> UnitDelete([FromHeader(Name = "id")] string id)
        {
            bool result = await _unitsProcessor.deleteUnit(id);
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "UnitRead")]
        public async Task<IActionResult> GetDataForListUnit()
        {
            var result = await _unitsProcessor.fillDataForListUnit();
            return Json(result);
        }

        [HttpGet]
        [Authorize(Roles = "UnitRead")]
        public async Task<IActionResult> UnitDetail(string id)
        {
            var unit = await _unitsProcessor.getUnitById(id);
            ViewBag.Action = "View";
            return View("UnitAdd", unit);
        }

        [HttpGet]
        [Authorize(Roles = "UnitEdit")]
        public async Task<IActionResult> UnitEdit(string id)
        {
            var unit = await _unitsProcessor.getUnitById(id);
            ViewBag.Action = "Edit";
            return View("UnitAdd", unit);
        }

        [HttpPost]
        [Authorize(Roles = "UnitEdit")]
        public async Task<IActionResult> UnitEdit([FromForm]Unit unit, string statuss)
        {
            unit.Status = statuss == "1" ? true : false;
            bool result = await _unitsProcessor.updateUnit(unit);
            Response.Cookies.Append("unitUpdateResult", result.ToString());
            ViewBag.Action = "Edit";
            return View("UnitAdd", unit);
        }

        public IActionResult test()
        {
            var temp = _unitsProcessor.fillDataForListUnit();
            return View();
        }
    }
}
