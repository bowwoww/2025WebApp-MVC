using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelSysDBContext _context;

        public HomeController(ILogger<HomeController> logger, HotelSysDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            _context.GetMemberWithTelAsync("M0001").ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var memberWithTels = task.Result;
                    // Do something with memberWithTels if needed
                }
                else
                {
                    // Handle the error
                    _logger.LogError(task.Exception, "Error fetching member with tel");
                }
            });
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
