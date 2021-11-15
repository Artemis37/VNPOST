using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNPOSTWebUI.Models;
using VNPOSTWebUILibrary.BussinessLogic;
using VNPOSTWebUILibrary.Model;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace VNPOSTWebUI.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly NewsProcessor _newsProcessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _config;

        public ManageController(UserManager<IdentityUser> userManager, IWebHostEnvironment hostingEnv, IConfiguration config)
        {
            _newsProcessor = new NewsProcessor();
            _userManager = userManager;
            _hostingEnv = hostingEnv;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddNews()
        {
            var newsGroup = await _newsProcessor.LoadNewsGroupAsync();
            var majorNewsGroup = await _newsProcessor.LoadMajorNewsGroupAsync();


            Models.News neww = new Models.News()
            {
                newsGroups = newsGroup.ToDictionary(key => key.Id.ToString(), value => value.Name),
                MajorGroups = majorNewsGroup.ToDictionary(key => key.Id.ToString(), value => value.Name)
            };
            return View(neww);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(VNPOSTWebUI.Models.News news)
        {
            if (ModelState.IsValid)
            {
                var filePath = "/Images/vnpost-logo-short.png";
                if (news.LabelImage != null)
                {
                    filePath = await UploadImage(news.LabelImage);
                }

                VNPOSTWebUILibrary.Model.News convertedNews = new VNPOSTWebUILibrary.Model.News()
                {
                    Title = news.Title,
                    Summary = news.Summary,
                    Content = news.Content,
                    CreatedDate = DateTime.Now,
                    CreatedBy = _userManager.GetUserAsync(User).Result.Id,
                    GroupId = news.GroupId == null ? 0 : Int32.Parse(news.GroupId),
                    MajorGroupId = Int32.Parse(news.MajorGroupId),
                    Views = 0,
                    LabelImage = filePath
                };
                ViewBag.Result = await _newsProcessor.AddNews(convertedNews);
                return RedirectToAction("AddNews");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<string> UploadImage(IFormFile LabelImage)
        {
            string iFormImgExtension = LabelImage.ContentType;
            string imgExtension = "";
            if (iFormImgExtension.Equals("image/jpeg")) imgExtension = ".jpg";
            else if (iFormImgExtension.Equals("image/png")) imgExtension = ".png";
            else if (iFormImgExtension.Equals("image/gif")) imgExtension = ".gif";
            var filePath = Path.Combine(_config["StoredFilesPath"], Guid.NewGuid().ToString() + imgExtension);

            using (var stream = System.IO.File.Create(filePath))
            {
                await LabelImage.CopyToAsync(stream);
            }

            return filePath.Split("wwwroot")[1];
        }

        [HttpPost]
        public async Task<IActionResult> loadNewsGroup(string id)
        {
            var newsGroup = await _newsProcessor.LoadNewsGroupAsync();
            var majorNewsGroup = await _newsProcessor.LoadMajorNewsGroupAsync();

            //int majorNewsGroupId = newsGroup.FirstOrDefault(x => x.Id == Int32.Parse(id)).Id;

            Dictionary<string, string> newsGroupNameDictionary = newsGroup.Where(x => x.majorGroupId == Int32.Parse(id)).ToDictionary(key => key.Id.ToString(), value => value.Name);

            var newsGroupNameJson = JsonConvert.SerializeObject(newsGroupNameDictionary, Formatting.Indented);

            return Json(newsGroupNameJson);
        }

        public IActionResult AddNewsGroup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewsGroup(string MajorGroup, string RelatedMajorGroup, string Group)
        {
            if (MajorGroup != null && Group == null)
            {
                //add major group
                await _newsProcessor.AddMajorNewsGroupAsync(MajorGroup);
            } else if (MajorGroup == null && Group != null)
            {
                //add group
                int temp = Int32.Parse(RelatedMajorGroup);
                await _newsProcessor.AddNewsGroupAsync(Group, Int32.Parse(RelatedMajorGroup));
            }
            return View();
        }

        public async Task<IActionResult> EditNews(string id)
        {
            var newsGroup = await _newsProcessor.LoadNewsGroupAsync();
            var majorNewsGroup = await _newsProcessor.LoadMajorNewsGroupAsync();

            var news = await _newsProcessor.LoadNewsWithIdAsync(Int32.Parse(id));
            string tempGroupId = "";
            if (news.GroupId != null) tempGroupId = news.GroupId.ToString();
            VNPOSTWebUI.Models.News webuiNews = new Models.News()
            {
                Title = news.Title,
                Summary = news.Summary,
                Content = news.Content,
                CreatedDate = news.CreatedDate,
                CreatedBy = news.CreatedBy,
                GroupId = tempGroupId,
                MajorGroupId = news.MajorGroupId.ToString(),
                newsGroups = newsGroup.ToDictionary(key => key.Id.ToString(), value => value.Name),
                MajorGroups = majorNewsGroup.ToDictionary(key => key.Id.ToString(), value => value.Name)
            };
            ViewBag.Id = id;
            return View("AddNews", webuiNews);
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(VNPOSTWebUI.Models.News news)
        {
            if (ModelState.IsValid)
            {
                string id = TempData["id"] + "";

                var foundNews = await _newsProcessor.LoadNewsWithIdAsync(Int32.Parse(id));
                var filePath = foundNews.LabelImage;
                if (news.LabelImage != null)
                {
                    filePath = await UploadImage(news.LabelImage);
                }

                VNPOSTWebUILibrary.Model.News convertedNews = new VNPOSTWebUILibrary.Model.News()
                {
                    Id = Int32.Parse(id),
                    Title = news.Title,
                    Summary = news.Summary,
                    Content = news.Content,
                    CreatedDate = news.CreatedDate,
                    CreatedBy = news.CreatedBy,
                    GroupId = news.GroupId == null ? 0 : Int32.Parse(news.GroupId),
                    MajorGroupId = Int32.Parse(news.MajorGroupId),
                    Views = news.Views,
                    LabelImage = filePath
                };

                await _newsProcessor.UpdateNews(convertedNews);
                return RedirectToAction("DetailNews","Home",new { id = id });
            }
            return View();
        }

        public async Task<IActionResult> DeleteNews(string id)
        {
            await _newsProcessor.DeleteNews(Int32.Parse(id));
            return RedirectToAction("NewsIndex", "Home");
        }

        public IActionResult test()
        {
            return View();
        }
        [HttpPost]
        public IActionResult test(VNPOSTWebUI.Models.News news)
        {
            
            return View();
        }
    }
}
