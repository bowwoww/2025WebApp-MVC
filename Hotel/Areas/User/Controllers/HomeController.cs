using ClassLibrary;
using DBContextClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hotel.Areas.User.Controllers
{
    [Area("User")]
    [Route("/What/[area]/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelSysDBContext _context;

        public HomeController(ILogger<HomeController> logger, HotelSysDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.GetMemberWithTelAsync("A0001");
            return View(result);
            
        }

        [Route("/{oh}-[action]/What.html")]
        public async  Task<IActionResult> New(string oh)
        {
            var result = await _context.GetMemberWithTelAsync("A0001");
            return View(result);
        }

        [Route("/Privacy")]
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
