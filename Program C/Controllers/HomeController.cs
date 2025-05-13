using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Program_C.Models;

namespace Program_C.Controllers
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
            ViewData["time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewData["link"] = "<a href='https://www.google.com'>Google</a>";
            return View();
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
