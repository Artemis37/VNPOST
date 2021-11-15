using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VNPOSTWebUI.Models;
using VNPOSTWebUILibrary.BussinessLogic;

namespace VNPOSTWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsProcessor _newsProcessor;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _newsProcessor = new NewsProcessor();
        }

        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> NewsIndex()
        {
            AllGroups allGroups = new AllGroups()
            {
                majorNewsGroup = await _newsProcessor.LoadMajorNewsGroupAsync(),
                newsGroup = await _newsProcessor.LoadNewsGroupAsync()
            };
            return View(allGroups);
        }

        [HttpGet]
        public async Task<IActionResult> LoadNewsWithMajorGroup(string id)
        {
            var tempList = await _newsProcessor.LoadNewsWithMajorGroupIdAsync(Int32.Parse(id));
            return View("ListNews", tempList);
            //return RedirectToAction("ListNews", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LoadNewsWithGroup(string id)
        {
            var tempList = await _newsProcessor.LoadNewsWithGroupIdAsync(Int32.Parse(id));
            return View("ListNews", tempList);
        }

        public async Task<IActionResult> DetailNews(string id)
        {
            if (string.IsNullOrEmpty(id)) RedirectToAction("Index");

            var tempNews = await _newsProcessor.LoadNewsWithIdAsync(Int32.Parse(id));
            tempNews.Views++;
            await _newsProcessor.UpdateNews(tempNews);
            return View(tempNews);
        }
    }
}
