using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tra.DotNetBatch5.MvcApp.Models;

namespace Tra.DotNetBatch5.MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            ViewBag.Message = "View bag message Say HEllo";
            HomeResponseModel homeResponsemodel = new HomeResponseModel() {Message = "Home Reposnse MMessage",Level = 2 };
            return View(homeResponsemodel);
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
    }
}
